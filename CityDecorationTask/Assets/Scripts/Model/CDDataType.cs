using System;
using UnityEngine;

[Serializable]
public enum DropableObjectType
{
		Undefined = 0,
		Buildings,
		Nature,
		Road,
		Treasure,
		Vehicles,
		Props ,
		Custom
}

[Serializable]
public class DropableItemModel
{
	public string Name;
	public string DisplayName;
	public DropableObjectType Type;
	public Sprite Sprite;
	public GameObject Prefab;
}
