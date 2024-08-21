using UnityEngine;
public interface IMenuItem
{
    public void Init(params object[] P);

    public GameObject GetGameObject();
}
