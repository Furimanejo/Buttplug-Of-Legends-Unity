using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttplug;
using ButtplugUnity;

public class ButtplugClient : MonoBehaviour
{
    ButtplugUnityClient client = null;

    async void Start()
    {
        client = new ButtplugUnityClient("Overlay");
        ButtplugAntiCrash.clientList.Add(client);
        var connector = new ButtplugWebsocketConnectorOptions(new Uri("ws://localhost:12345/buttplug"));
        await client.ConnectAsync(connector);
        await client.StartScanningAsync();
    }

    public void SendValue(float value)
    {
        if (client == null || client.Connected == false)
            return;

        foreach (var device in client.Devices)
        {
            if (device.AllowedMessages.ContainsKey(ServerMessage.Types.MessageAttributeType.VibrateCmd))
            {
                device.SendVibrateCmd(value);
                Debug.Log($"{device.Name} : {value}");
            }
        }
    }
}