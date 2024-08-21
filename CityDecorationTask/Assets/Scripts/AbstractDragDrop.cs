using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractDragDrop :  IDragDropHandle
{
    protected IDropableItemFactory itemFactory;
    public abstract void OnDrag(Transform obj, PointerEventData eventData);

    public abstract void OnDragBegin(Transform obj, PointerEventData eventData);

    public abstract void OnDrop(GameObject obj, PointerEventData eventData);

    public IDragDropHandle SetFactory(IDropableItemFactory factory)
    {
        itemFactory = factory;
        return this;
    }

}
