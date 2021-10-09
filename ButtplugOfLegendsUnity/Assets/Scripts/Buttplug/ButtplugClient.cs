using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttplug;
using ButtplugUnity;
using UnityEngine.UI;

public class ButtplugClient : MonoBehaviour
{
    [SerializeField] string clientDisplayName;
    [SerializeField] Text connectionStatusLabel;
    ButtplugUnityClient client = null;
    
    private void Start()
    {
        client = new ButtplugUnityClient(clientDisplayName);
        client.ServerDisconnect += Client_ServerDisconnect;
        ButtplugAntiCrash.clientList.Add(client);   
    }

    private void Client_ServerDisconnect(object sender, EventArgs e)
    {
        connectionStatusLabel.text = "Status: Disconnected";
    }

    public async void TryConnect()
    {
        if(client.Connected == false)
        {
            var connector = new ButtplugWebsocketConnectorOptions(new Uri("ws://localhost:12345/buttplug"));
            await client.ConnectAsync(connector);
            if (client.Connected)
            {
                connectionStatusLabel.text = "Status: Connected";
                await client.StartScanningAsync();
            }
        }
    }

    public void SendValue(float value,ServerMessage.Types.MessageAttributeType messageAttributeType)
    {
        if (client == null || client.Connected == false)
            return;

        foreach (var device in client.Devices)
        {
            if (device.AllowedMessages.ContainsKey(messageAttributeType))
            {
                device.SendVibrateCmd(value);
            }
        }
    }
}