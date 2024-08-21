using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropableItemFactory :AbstractItemFactory
{
    public DropableItemFactory()
    {
        world = GameObject.FindGameObjectWithTag("World").transform;
    }
    public override GameObject CreateObject(GameObject obj, Vector3 position  , Quaternion rotation )
    {
        GameObject go = GameObject.Instantiate(obj, world);
        go.transform.SetPositionAndRotation(position, rotation);
        if (go.TryGetComponent(out IDropableItem dropableItem))
        {
            dropableItem.PlayDropEffect();
        }
        return go;
    }
}
