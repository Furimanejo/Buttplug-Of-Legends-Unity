using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtplugController : MonoBehaviour
{
    [SerializeField] protected ButtplugClient client;
    [SerializeField] float transmissionTimerPeriod = .2f;
    float transmissionTimer = 0;
    protected float value;

    protected abstract void SendValueToClient();

    public virtual void SetValue(float _value)
    {
        value = _value;
        value = Mathf.Clamp(value, 0f, 1f);
    }

    private void Update()
    {
        transmissionTimer += Time.deltaTime;
        if (transmissionTimer > transmissionTimerPeriod)
        {
            transmissionTimer = 0;
            SendValueToClient();
        }
    }
}
