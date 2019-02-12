using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomButton : Button
{
	[SerializeField]
	private CameraScript playerCamera;

	[SerializeField]
	private bool isZoomIn;

	private void FixedUpdate()
	{
		if (isPressed)
		{
			if (isZoomIn)
			{
				playerCamera.ZoomIn();
			}
			else
			{
				playerCamera.ZoomOut();
			}
		}
	}
}
