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
    [SerializeField] Text connectButtonText;
    [SerializeField] InputField addressInputField = null;
    [SerializeField] float minDelayBetweenMessages = .2f;
    float messageTimer = 0f;
    [SerializeField] List<ButtplugController> controllers = new List<ButtplugController>();
    
    private void Update()
    {
        if (client == null || client.Connected == false)
            return;
        messageTimer += Time.deltaTime;
        if(messageTimer >= minDelayBetweenMessages)
        {
            messageTimer = 0;
            foreach (var controller in controllers)
                controller.SendValue(client.Devices);
        }
    }

    void CreateClient()
    {
        if (client != null)
            client.Dispose();
        client = new ButtplugUnityClient(clientDisplayName);
        client.ServerDisconnect += UpdateUI;
        ButtplugAntiCrash.clientList.Add(client);
    }

    private void UpdateUI(object sender, EventArgs e)
    {
        Debug.Log($"Update {client.Connected}");
        if (client.Connected)
        {
            connectionStatusLabel.text = "<color=green>Connected</color>";
            connectButtonText.text = "Disconnect";
        }
        else
        {
            connectionStatusLabel.text = "<color=red>Disconnected</color>";
            connectButtonText.text = "Connect";
        }
        Canvas.ForceUpdateCanvases();
    }
    
    public async void ToggleConnect()
    {
        if(client == null || client.Connected == false)
        {
            try
            {
                CreateClient();
                var address = new Uri(addressInputField.text);
                var connector = new ButtplugWebsocketConnectorOptions(address);
                connectionStatusLabel.text = "<color=yellow>Connecting...</color>";
                await client.ConnectAsync(connector);
                if (client.Connected)
                {
                    UpdateUI(null, null);
                    await client.StartScanningAsync();
                }
                else
                    connectionStatusLabel.text = $"<color=red>Connection Failed</color>";
            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                connectionStatusLabel.text = $"<color=red>{ex.Message}</color>";
            }

            //catch (ButtplugConnectorException ex) when (ex.Message.Contains("HTTP format error: invalid format"))
            //{
            //    connectionStatusLabel.text = $"<color=red>Invalid Address</color>";
            //}
            //catch (ButtplugConnectorException ex) when (ex.Message.Contains("URL scheme not supported"))
            //{
            //    connectionStatusLabel.text = $"<color=red>Invalid Address</color>";
            //}
            //catch (UriFormatException)
            //{
            //    connectionStatusLabel.text = $"<color=red>Invalid Address</color>";
            //}
        }
        else
        {
            if (client.Connected)
                await client.DisconnectAsync();
            client.Dispose();
        }
    }
}