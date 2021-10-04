using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtplugController : MonoBehaviour
{
    [SerializeField] ButtplugClient client = null;
    [SerializeField] Slider sensitivity = null;
    [SerializeField] Slider frequency = null;

    float transmissionTimer = 0;
    float transmissionTimerPeriod = .2f;

    // Update is called once per frame
    void Update()
    {
        transmissionTimer += Time.deltaTime;
        if(transmissionTimer > transmissionTimerPeriod)
        {
            transmissionTimer = 0;
            client.SendValue(sensitivity.value);
        }
    }
}
