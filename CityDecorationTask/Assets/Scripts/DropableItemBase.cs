using mattatz.TransformControl;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DropableItemBase : MonoBehaviour, IDropableItem 
{
	bool isSelected = false;
	TransformControl control;

	private void Start()
	{
		if(TryGetComponent<TransformControl>(out control));
			control.mode = TransformControl.TransformMode.None;
        RayCastHandler.OnClickRaycastHit += RayCastHandler_OnClickRaycastHit;
	}
    private void OnDisable()
    {
		RayCastHandler.OnClickRaycastHit -= RayCastHandler_OnClickRaycastHit;
	}

    private void RayCastHandler_OnClickRaycastHit(RaycastHit hit)
    {
		if (hit.transform == transform )
		{
            if (!isSelected)
            {
				isSelected = true;
				control.mode = TransformControl.TransformMode.Translate;
			}
			
		}
		else if (isSelected && !control.Pick())
		{
			isSelected = false;
			control.mode = TransformControl.TransformMode.None;
		}
	}

    void Update()
	{
		
		if (isSelected)
		{
			control?.Control();
			if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Mouse1))
			{
				control.mode = TransformControl.TransformMode.Rotate;
			}

			if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.Mouse1))
			{
				control.mode = TransformControl.TransformMode.Translate;
			}

			if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Mouse1))
			{
				control.mode = TransformControl.TransformMode.Scale;
			}
		}
	}

    public void PlayDropEffect()
    {
		//control.mode = TransformControl.TransformMode.Translate;
		//drop effect
	}
}
