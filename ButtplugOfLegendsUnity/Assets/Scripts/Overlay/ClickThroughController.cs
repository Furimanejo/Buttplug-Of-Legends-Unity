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
    bool status = true;

    private void Start()
    {
        StartCoroutine(CheckRaycast());
    }

    IEnumerator CheckRaycast()
    {
        //Set up the new Pointer Event
        var m_PointerEventData = new PointerEventData(eventSystem);
        while (true)
        {
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;
            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            //Raycast using the Graphics Raycaster and mouse click position
            graphicRaycaster.Raycast(m_PointerEventData, results);

            bool hit = results.Count > 0;
            if(hit != status)
            {
                status = hit;
                overlayController.SetClickThrough(!hit);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
