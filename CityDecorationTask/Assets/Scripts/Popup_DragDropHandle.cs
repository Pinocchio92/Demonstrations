using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Popup_DragDropHandle : StretchEffect_DragDropHandle
{

    float jumpHeight = 3;// height untill drobable onject will jump
    float punchScale = .5f; //punch scale size
    float rotationAngle = 360f;
    public override void OnDrop(GameObject obj, PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast to detect objects
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the object hit has a specific tag or layer
            if (hit.collider.CompareTag("Train"))
            {
                GameObject go = itemFactory?.CreateObject(obj, hit.point, Quaternion.identity);
                if (go != null)
                {
                    go.transform.DOPunchPosition(new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y + jumpHeight, obj.transform.localPosition.z), 2, 1, 5);
                    go.transform.DORotate(new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y + rotationAngle, obj.transform.localPosition.z), 1, RotateMode.LocalAxisAdd);
                    go.transform.DOPunchScale(new Vector3(punchScale, punchScale, punchScale), 2, 1, 5);
                }
            }
            else
            {
                Debug.Log("No train detected.");
            }
        }
    }
}
