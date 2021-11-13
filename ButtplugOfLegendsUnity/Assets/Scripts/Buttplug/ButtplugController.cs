using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtplugController : MonoBehaviour
{
    [SerializeField] protected ButtplugClient client;
    protected float value;

    protected abstract void UpdateValueToClient();

    public virtual void SetValue(float _value)
    {
        value = _value;
        value = Mathf.Clamp(value, 0f, 1f);
    }

    private void Update()
    {
        UpdateValueToClient();
    }
}
