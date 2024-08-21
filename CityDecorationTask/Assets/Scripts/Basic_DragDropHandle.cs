using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Basic_DragDropHandle : AbstractDragDrop
{
    public override void OnDrag(Transform obj, PointerEventData eventData)
    {
        obj.position = Input.mousePosition;
    }

    public override void OnDragBegin(Transform obj, PointerEventData eventData)
    {
        obj.position = Input.mousePosition;
    }

    public override void OnDrop(GameObject obj, PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast to detect objects
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Check if the object hit has a specific tag or layer
            if (hit.collider.CompareTag("Train"))
            {
                Debug.Log("Train detected under mouse!");
                GameObject go =itemFactory.CreateObject(obj, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("No train detected.");
            }
        }
    }
}
