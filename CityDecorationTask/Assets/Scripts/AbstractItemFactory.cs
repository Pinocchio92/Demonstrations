using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractItemFactory : IDropableItemFactory
{
    protected Transform world =null;
    public abstract GameObject CreateObject(GameObject obj, Vector3 position, Quaternion rotation);
    
}
