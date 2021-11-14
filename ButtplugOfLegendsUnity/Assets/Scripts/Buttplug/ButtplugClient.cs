using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttplug;
using ButtplugUnity;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ButtplugClient : MonoBehaviour
{
    [SerializeField] string clientDisplayName;
    [SerializeField] Text connectionStatusLabel;
    ButtplugUnityClient client = null;
    Queue<Message> messageQueue = new Queue<Message>();
    [SerializeField] float minDelayBetweenMessages = .2f;
    float waitForNextMessageDequeue = 0f;
    
    class Message
    {
        float value;
        public float delay { get; private set; }
        ServerMessage.Types.MessageAttributeType messageAttributeType;

        public Message(float _value, float _delay, ServerMessage.Types.MessageAttributeType messageAttributeType)
        {
            value = _value;
            delay = _delay;
        }

        public void Send(ButtplugUnityClient client)
        {
            if (client == null || client.Connected == false)
                return;

            foreach (var device in client.Devices)
            {
                if (device.AllowedMessages.ContainsKey(messageAttributeType))
                {
                    if (messageAttributeType == ServerMessage.Types.MessageAttributeType.VibrateCmd)
                        device.SendVibrateCmd(value);
                }
            }
        }
    }

    private void Start()
    {
        client = new ButtplugUnityClient(clientDisplayName);
        UpdateUIDisconnected(null, null);
        client.ServerDisconnect += UpdateUIDisconnected;
        ButtplugAntiCrash.clientList.Add(client);
    }

    private void Update()
    {
        if (waitForNextMessageDequeue < 0)
        {
            if (messageQueue.Count > 0)
            {
                var message = messageQueue.Dequeue();
                message.Send(client);
                waitForNextMessageDequeue = message.delay;
            }
        }
        else
        {
            waitForNextMessageDequeue -= Time.deltaTime;
        }            
    }

    private void UpdateUIDisconnected(object sender, EventArgs e)
    {
        connectionStatusLabel.text = "Status: <color=red>Disconnected</color>";
    }
    
    public async void TryConnect()
    {
        if(client.Connected == false)
        {
            var connector = new ButtplugWebsocketConnectorOptions(new Uri("ws://localhost:12345/buttplug"));
            await client.ConnectAsync(connector);
            if (client.Connected)
            {
                connectionStatusLabel.text = "Status: <color=green>Connected</color>";
                await client.StartScanningAsync();
            }
        }
    }
    
    public void TestDevices()
    {
        QueueMenssage(1f, 1f, ServerMessage.Types.MessageAttributeType.VibrateCmd);
    }

    public void QueueMenssage(float value, float delay, ServerMessage.Types.MessageAttributeType messageType)
    {
        if(delay < minDelayBetweenMessages)
        {
            if (messageQueue.Count > 0)
                return;
            delay = minDelayBetweenMessages;
        }
        var message = new Message(value, delay, messageType);
        messageQueue.Enqueue(message);
    }

}