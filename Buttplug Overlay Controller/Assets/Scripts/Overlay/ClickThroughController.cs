using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickThroughController : MonoBehaviour
{
    [SerializeField] OverlayController overlayController = null;
    [SerializeField] EventSystem eventSystem = null;
    [SerializeField] GraphicRaycaster graphicRaycaster = null;

    void Update()
    {
        //Set up the new Pointer Event
        var m_PointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        graphicRaycaster.Raycast(m_PointerEventData, results);

        bool hit = results.Count > 0;
        overlayController.SetClickThrough(!hit);
    }
}
