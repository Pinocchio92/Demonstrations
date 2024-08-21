using UnityEngine;
using UnityEngine.EventSystems;

public interface IDragDropHandle
{
    public abstract void OnDragBegin(Transform obj , PointerEventData eventData);

    public abstract void OnDrag(Transform obj, PointerEventData eventData);

    public abstract void OnDrop(GameObject obj, PointerEventData eventData);
}
