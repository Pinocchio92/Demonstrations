using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropableItemDataContainer", menuName = "City Decorator/DropableItemDataContainer")]
public class DropableItemData : ScriptableObject
{
    public List<DropableItemModel> Data;
}

