using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWhenUnfocused : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToControl;

    private void OnApplicationFocus(bool focus)
    {
        ApplyFocus(focus);
    }

    void ApplyFocus(bool focus)
    {
        foreach (var obj in objectsToControl)
        {
            obj.SetActive(focus);
        }
    }
}
