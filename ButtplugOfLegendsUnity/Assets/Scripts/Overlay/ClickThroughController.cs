using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickThroughController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] OverlayController overlayController = null;

    private void OnDisable()
    {
        overlayController.SetClickThrough(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        overlayController.SetClickThrough(false);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        overlayController.SetClickThrough(true);
    }
}
