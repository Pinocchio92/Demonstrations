using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{

    const string DataContainerPath = "DropableItemDataContainer";
    #region Initializers
    private static DataHandler instance = null;
    public static DataHandler Instance
    {
        get
        {
            if (instance == null)
            {
                InitializeDataHandler();
            }
            return instance;
        }
    }

    private static void InitializeDataHandler()
    {
        GameObject CityDacoratorDataHandler = new GameObject("DataHandler");
        instance = CityDacoratorDataHandler.AddComponent<DataHandler>();
        instance.Container = Resources.Load<DropableItemData>(DataContainerPath);
    }
    #endregion

    [SerializeField]
    DropableItemData Container;
    public List<DropableItemModel> GetDropableItemsDAta()
    {
        return Container.Data;
    }
}
