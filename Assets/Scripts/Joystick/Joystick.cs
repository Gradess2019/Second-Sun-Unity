using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Joystick : MonoBehaviour
{
	[SerializeField]
	private float planeDistance = 0.5f;

	[SerializeField]
	private float maxPower = 50f;

	private float powerMultiplier;

	[SerializeField]
	private Image background;

	private Image thumbstick;

	private bool isPressed;

	private Touch touch;	

	private void Awake()
	{
		thumbstick = GetComponent<Image>();
	}

	public bool IsPressed { get => isPressed; set => isPressed = value; }
	public float MaxPower { get => maxPower; set => maxPower = value; }

	public void StartHandling(Touch touch)
	{
		this.touch = touch;

		if (background)
		{
			background.gameObject.transform.position = GetTouchWorldLocation();
			powerMultiplier = MaxPower / (background.rectTransform.sizeDelta.x / 2f);
		}
		else
		{
			throw new System.NullReferenceException("background is null!");
		}

		StartCoroutine("JoystickMove");
		isPressed = true;
	}

	public void StopHandling()
	{
		StopCoroutine("JoystickMove");
		isPressed = false;
		thumbstick.rectTransform.localPosition = Vector3.zero;
	}

	IEnumerator JoystickMove()
	{
		while (Application.isPlaying)
		{
			try 
			{
				touch = Input.GetTouch(touch.fingerId);
			} 
			catch(System.ArgumentException) 
			{
				if (Input.touchCount > 0 && touch.fingerId > 0) 
				{
					touch = Input.GetTouch(touch.fingerId - 1);
				} else 
				{
					StopHandling();
					yield break;
				}
			}
			
			if (touch.phase != TouchPhase.Stationary)
			{
				//Distance between zero vector and current thumbstick position 
				float distance = Vector3.Distance
				(
					new Vector3(touch.position.x, touch.position.y, planeDistance),
					Camera.main.WorldToScreenPoint(background.transform.position)
				);

				float halfSizeX = background.rectTransform.sizeDelta.x / 2f;
				float halfSizeY = background.rectTransform.sizeDelta.y / 2f;

				if (distance < halfSizeX && distance < halfSizeY)
				{
					thumbstick.rectTransform.position = GetTouchWorldLocation();
				}
				else
				{
					Vector3 direction = GetDirection2D();
					thumbstick.rectTransform.localPosition =
						new Vector3(direction.x * Mathf.Abs(halfSizeX), direction.y * Mathf.Abs(halfSizeY));
				}
			}
			yield return 0;
		}
	}

	public Vector3 GetInput()
	{
		if (touch.phase == TouchPhase.Ended)
		{
			return Vector3.zero;
		}

		Vector3 thumbStickDirection = GetDirection2D();
		Vector3 startDirection = Vector2.up;
		Camera camera = Camera.main;

		if (thumbStickDirection != Vector3.zero)
		{
			float power = Vector3.Distance
			(
				camera.WorldToScreenPoint(background.transform.position),
				camera.WorldToScreenPoint(thumbstick.transform.position)
			) * powerMultiplier;

			Vector3 movement = Vector3.Normalize
			(
				Quaternion.Euler(0, Vector3.Angle(startDirection, thumbStickDirection) * Mathf.Sign(thumbStickDirection.x), 0) * camera.transform.forward
			) * power;
				
			return movement;
		}
		return Vector3.zero;
	}

	private Vector3 GetTouchWorldLocation()
	{
		return Camera.main.ScreenToWorldPoint(TouchTo3DVector());
	}

	private Vector3 GetDirection2D()
	{
		return Vector3.Normalize(
			TouchTo3DVector() - Camera.main.WorldToScreenPoint(background.transform.position)
		);
	}

	private Vector3 TouchTo3DVector()
	{
		return (touch.phase != TouchPhase.Ended) ?
			new Vector3(touch.position.x, touch.position.y, planeDistance) :
			Vector3.zero;
	}

	private Vector3 TouchTo3DVector(Touch touch)
	{
		return (touch.phase != TouchPhase.Ended) ?
			new Vector3(touch.position.x, touch.position.y, planeDistance) :
			Vector3.zero;
	}
}
