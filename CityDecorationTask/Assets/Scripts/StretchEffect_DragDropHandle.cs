using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StretchEffect_DragDropHandle : AbstractDragDrop
{
    public float scaleSpeed = 0.001f; // Speed at which the object scales
    public float minScale = 0.1f;    // Minimum scale limit
    public float maxScale = 2.0f;    // Maximum scale limit

    private Vector3 initialScale = Vector3.one;    // The initial scale of the object
    private Vector3 startDragPosition; // The starting position of the drag
    public override void OnDrag(Transform obj, PointerEventData eventData)
    {
        obj.position = Input.mousePosition;
        Vector3 currentDragPosition = Input.mousePosition; // Get the current mouse position
        float dragDistance = Vector3.Distance( currentDragPosition , startDragPosition) * scaleSpeed; // Calculate drag distance
        // Calculate new scale based on drag distance
        float scaleFactor = Mathf.Clamp(initialScale.x - dragDistance, minScale, initialScale.x);
        Vector3 newScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // Apply the new scale to the object
        obj.localScale = newScale;
    }

    public override void OnDragBegin(Transform obj, PointerEventData eventData)
    {
        startDragPosition = Input.mousePosition;
        obj.position = Input.mousePosition;
        initialScale = obj.localScale;
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
                GameObject go = itemFactory.CreateObject(obj, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("No train detected.");
            }
        }
    }
}
