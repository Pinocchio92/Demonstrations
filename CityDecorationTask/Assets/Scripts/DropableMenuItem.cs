using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropableMenuItem : MonoBehaviour, IMenuItem, IDragHandler, IEndDragHandler,IBeginDragHandler
{
    [SerializeField]
    TMP_Text itemDispayName;
    [SerializeField]
    Image itemDispayImage;

    DropableItemModel data;
    IDragDropHandle dragHandle;

    public void Init(params object[] P)
    {
        data = P[0] as DropableItemModel;
        itemDispayName.text = data.DisplayName;
        itemDispayImage.sprite = data.Sprite;
        gameObject.SetActive(true);
        dragHandle = new Popup_DragDropHandle().SetFactory(P[1] as IDropableItemFactory);
    }
    public GameObject GetGameObject() => gameObject;

    public void OnDrag(PointerEventData eventData) => dragHandle.OnDrag(itemDispayImage.transform, eventData);
    public void OnEndDrag(PointerEventData eventData) 
    {
        itemDispayImage.transform.localPosition = Vector3.zero;
        itemDispayImage.transform.localScale = Vector3.one;
        dragHandle.OnDrop(data.Prefab, eventData);
    }
    public void OnBeginDrag(PointerEventData eventData) => dragHandle.OnDragBegin(itemDispayImage.transform, eventData);
}
