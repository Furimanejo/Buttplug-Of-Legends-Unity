using Buttplug;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtplugController : MonoBehaviour
{
    protected float value;
    public virtual void SetValue(float _value)
    {
        value = _value;
        value = Mathf.Clamp(value, 0f, 1f);
    }

    public abstract void SendValue(ButtplugClientDevice[] devices);
}
