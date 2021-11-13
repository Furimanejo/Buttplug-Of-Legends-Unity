using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtplugVibrationController : ButtplugController
{
    [SerializeField] InputField maxDeviceStrength;
    int pattern = 0;
    [SerializeField] InputField frequencyBPM;
    const float BPM_TO_RADS = 0.10471975499997f;

    protected override void UpdateValueToClient()
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
        client.QueueMenssage(sendValue, 0,
            Buttplug.ServerMessage.Types.MessageAttributeType.VibrateCmd);
    }

    public void OnChangePattern(int newPattern)
    {
        pattern = newPattern;
    }
}
