using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Joystick joystick;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!joystick.IsPressed) {
			try
			{
				joystick.StartHandling(Input.GetTouch(eventData.pointerId));
			} catch (System.ArgumentException)
			{
				Debug.LogError("Wrong pointer id");
			}
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.StopHandling();
    }
}
