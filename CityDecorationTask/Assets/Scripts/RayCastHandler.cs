using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHandler : MonoBehaviour
{
    public static event Action<RaycastHit> OnClickRaycastHit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect objects
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                OnClickRaycastHit?.Invoke(hit);
            }
        }
     
    }
    
}
