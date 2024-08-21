
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryMenuItem : MonoBehaviour , IMenuItem
{
    [SerializeField]
    Text categoryName;
    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    GameObject dropableMenuItemPrefab;

    public void Init(params object[] P)
    {
        categoryName.text = P[1] as string;
        var catogoryItems = P[0] as List<DropableItemModel>;
        foreach (var item in catogoryItems)
        {
            GameObject.Instantiate(dropableMenuItemPrefab, contentRoot).TryGetComponent<IMenuItem>(out var categoryMenuEntry);
            categoryMenuEntry.Init(item,P[2] as IDropableItemFactory);
        }
    }
    public GameObject GetGameObject() => gameObject;
}
