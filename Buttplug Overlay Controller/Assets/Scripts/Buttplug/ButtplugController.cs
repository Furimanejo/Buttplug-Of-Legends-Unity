using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtplugController : MonoBehaviour
{
    [SerializeField] ButtplugClient client = null;
    [SerializeField] Slider sensitivity = null;
    [SerializeField] Slider frequency = null;    

    // Update is called once per frame
    void Update()
    {
        client.SetValue(sensitivity.value);
    }
}
