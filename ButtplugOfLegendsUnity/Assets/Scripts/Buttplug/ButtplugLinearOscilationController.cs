using Buttplug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtplugLinearOscilationController : ButtplugController
{
    [SerializeField] InputField maxBPM;
    float desiredLinearPosition = 0f;
    float linearDirectionMultiplier = 1;
    float CurrentOscillationFrequency
    {
        get
        {
            return value * float.Parse(maxBPM.text) / 60f;
        }
    }

    private void Update()
    {
        desiredLinearPosition += linearDirectionMultiplier * CurrentOscillationFrequency * Time.deltaTime;
    }

    public override void SendValue(ButtplugClientDevice[] devices)
    {
        if (desiredLinearPosition < 1f && desiredLinearPosition > 0f)
            return;
        linearDirectionMultiplier *= -1f;
        desiredLinearPosition = Mathf.Clamp(desiredLinearPosition, 0f, 1f);
        uint duration = uint.MaxValue;
        try
        {
            duration = System.Convert.ToUInt32(1000f / CurrentOscillationFrequency / 2f);
        }
        catch { }

        Debug.Log($"{desiredLinearPosition} - {duration}");

        foreach (var device in devices)
            if (device.AllowedMessages.ContainsKey(ServerMessage.Types.MessageAttributeType.LinearCmd))
                device.SendLinearCmd(duration, desiredLinearPosition);
    }
}