using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private MonoBehaviour categoryMenuItemBehaviour; // MonoBehaviour field to hold the implementing object
    [SerializeField]
    Transform categoryParentTransform;

    IMenuItem categoryMenuItem;
    IDropableItemFactory factory;
    // Start is called before the first frame update
    void Start()
    {
        categoryMenuItem = (IMenuItem)categoryMenuItemBehaviour;
        factory = new DropableItemFactory();
        InitializeUI();

    }
    void InitializeUI()
    {
        var dropableItemlist = DataHandler.Instance.GetDropableItemsDAta();
        List<DropableItemModel> distinctItemsTypes = dropableItemlist
             .GroupBy(item => item.Type)
             .Select(group => group.First())
             .ToList();

        foreach (var item in distinctItemsTypes)
        {
            GameObject.Instantiate(categoryMenuItem.GetGameObject(), categoryParentTransform).TryGetComponent<IMenuItem>(out var categoryMenuEntry);
            categoryMenuEntry.Init(dropableItemlist.Where(x => x.Type == item.Type).ToList(), item.Type.ToString(), factory);
        }
    }

}
