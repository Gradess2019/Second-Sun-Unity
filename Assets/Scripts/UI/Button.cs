using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	protected bool isPressed;

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		isPressed = true;
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		isPressed = false;
	}
}
