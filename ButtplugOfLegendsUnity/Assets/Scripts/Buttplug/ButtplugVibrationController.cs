using Buttplug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtplugVibrationController : ButtplugController
{
    [SerializeField] InputField maxDeviceStrength;
    [SerializeField] InputField frequencyBPM;
    int pattern = 0;
    const float BPM_TO_RADS = 0.10471975499997f;

    public override void SendValue(ButtplugClientDevice[] devices)
    {
        var sendValue = value * float.Parse(maxDeviceStrength.text) / 100;
        switch (pattern)
        {
            case 1:
                sendValue *= .5f + .5f * Mathf.Sin(float.Parse(frequencyBPM.text) * Time.realtimeSinceStartup * BPM_TO_RADS);
                break;
            default:
                break;
        }
        foreach (var device in devices)
            if (device.AllowedMessages.ContainsKey(ServerMessage.Types.MessageAttributeType.VibrateCmd))
                device.SendVibrateCmd(sendValue);
    }

    public void OnChangePattern(int newPattern)
    {
        pattern = newPattern;
    }
}
