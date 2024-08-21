using UnityEngine;

public interface IDropableItemFactory
{
    GameObject CreateObject(GameObject obj, Vector3 position, Quaternion rotation);
}
