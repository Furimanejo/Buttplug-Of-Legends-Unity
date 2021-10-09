using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Buttplug;
using ButtplugUnity;
using UnityEngine.UI;

public class ButtplugClient : MonoBehaviour
{
    [SerializeField] string clientDisplayName;    
    ButtplugUnityClient client = null;
    float transmissionTimer = 0;
    [SerializeField] float transmissionTimerPeriod = .2f;
    float value = 0f;

    [SerializeField] InputField maxDeviceStrength;

    private void Start()
    {
        client = new ButtplugUnityClient(clientDisplayName);
        ButtplugAntiCrash.clientList.Add(client);        
    }

    public async void TryConnect()
    {
        if(client.Connected == false)
        {
            var connector = new ButtplugWebsocketConnectorOptions(new Uri("ws://localhost:12345/buttplug"));
            await client.ConnectAsync(connector);
            if(client.Connected)
                await client.StartScanningAsync();
        }
    }

    void Update()
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

        var valueToBeSent = value * float.Parse(maxDeviceStrength.text) / 100;

        foreach (var device in client.Devices)
        {
            if (device.AllowedMessages.ContainsKey(ServerMessage.Types.MessageAttributeType.VibrateCmd))
            {
                device.SendVibrateCmd(valueToBeSent);
            }
        }
    }
}