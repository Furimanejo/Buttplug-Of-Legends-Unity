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
    ButtplugUnityClient client = null;
    [SerializeField] string clientDisplayName;
    [SerializeField] Text connectionStatusLabel;
    [SerializeField] float minDelayBetweenMessages = .2f;
    float messageTimer = 0f;
    [SerializeField] List<ButtplugController> controllers = new List<ButtplugController>();
    
    private void Start()
    {
        client = new ButtplugUnityClient(clientDisplayName);
        UpdateUIDisconnected(null, null);
        client.ServerDisconnect += UpdateUIDisconnected;
        ButtplugAntiCrash.clientList.Add(client);
    }

    private void Update()
    {
        if (client.Connected == false)
            return;
        messageTimer += Time.deltaTime;
        if(messageTimer >= minDelayBetweenMessages)
        {
            messageTimer = 0;
            foreach (var controller in controllers)
                controller.SendValue(client.Devices);
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
}