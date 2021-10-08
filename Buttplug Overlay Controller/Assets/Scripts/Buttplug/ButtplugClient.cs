using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttplug;
using ButtplugUnity;

public class ButtplugClient : MonoBehaviour
{
    ButtplugUnityClient client = null;
    float transmissionTimer = 0;
    float transmissionTimerPeriod = .2f;
    float value = 0f;

    async void Start()
    {
        client = new ButtplugUnityClient("Overlay");
        ButtplugAntiCrash.clientList.Add(client);
        var connector = new ButtplugWebsocketConnectorOptions(new Uri("ws://localhost:12345/buttplug"));
        await client.ConnectAsync(connector);
        await client.StartScanningAsync();
    }

    private void Update()
    {
        transmissionTimer += Time.deltaTime;
        if (transmissionTimer > transmissionTimerPeriod)
        {
            transmissionTimer = 0;
            SendValue();
        }
    }

    public void SetValue(float _value)
    {
        value = _value;
        value = Mathf.Clamp(value, 0f, 1f);
    }

    void SendValue()
    {
        if (client == null || client.Connected == false)
            return;

        foreach (var device in client.Devices)
        {
            if (device.AllowedMessages.ContainsKey(ServerMessage.Types.MessageAttributeType.VibrateCmd))
            {
                device.SendVibrateCmd(value);
            }
        }
    }
}