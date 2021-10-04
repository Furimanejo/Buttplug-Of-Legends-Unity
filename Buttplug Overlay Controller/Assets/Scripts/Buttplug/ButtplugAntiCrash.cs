using System.Collections.Generic;
using ButtplugUnity;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public static class ButtplugAntiCrash
{
    //maybe get a list of clients from thin air somehow so the user doesn't need to register the clients in this list
    public static List<ButtplugUnityClient> clientList = new List<ButtplugUnityClient>();

    static ButtplugAntiCrash()
    {
        Application.quitting += KillButtplugServer;
#if !UNITY_EDITOR
        Application.quitting += System.Diagnostics.Process.GetCurrentProcess().Kill;
#endif
    }

    static void KillButtplugServer()
    {
        Debug.LogWarning($"Killing buttplug server and clients");
        foreach (ButtplugUnityClient client in clientList)
            if (client != null)
            {
                if (client.Connected)
                    client.DisconnectAsync();
                client.Dispose();
            }
        ButtplugUnityHelper.StopServer();
    }
}