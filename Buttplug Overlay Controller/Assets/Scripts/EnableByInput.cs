using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;

public class EnableByInput : MonoBehaviour
{
    [SerializeField] RawKey enableKey = RawKey.RightShift;
    [SerializeField] GameObject objectToEnable = null;

    void OnEnable()
    {
        RawKeyInput.Start(true);
        RawKeyInput.OnKeyDown += RawKeyInput_OnKeyDown;
        //RawKeyInput.OnKeyUp += RawKeyInput_OnKeyUp;
        //objectToEnable.SetActive(false);
    }
    private void OnDisable()
    {
        RawKeyInput.OnKeyDown -= RawKeyInput_OnKeyDown;
        //RawKeyInput.OnKeyUp -= RawKeyInput_OnKeyUp;
        RawKeyInput.Stop();
    }

    private void RawKeyInput_OnKeyDown(RawKey key)
    {
        if (key == enableKey)
            objectToEnable.SetActive(!objectToEnable.activeSelf);
    }

    //private void RawKeyInput_OnKeyUp(RawKey key)
    //{
    //    if (key == enableKey)
    //        objectToEnable.SetActive(false);
    //}
}
