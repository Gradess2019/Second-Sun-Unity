using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField]
	private Player owner;

	[SerializeField]
	private Vector3 cameraOffset;

	private Vector3 deltaZoomPosition;

	[SerializeField]
	private float smoothing = 1f;

	[SerializeField]
	private float zoomPower = 1f;

	[SerializeField]
	private float minZoomOffset = 0f;

	[SerializeField]
	private float maxZoomOffset = 30f;

	[SerializeField]
	private float maxFarClipPlane = 200f;

	private float zoomOffset = 10f;

	private Camera playerCamera;

	private void Start() {
		playerCamera = GetComponent<Camera>();
	}

	private void FixedUpdate()
	{
		MoveCamera();
	}

	private void MoveCamera()
	{
		Vector3 targetCameraPosition = new Vector3
		(
			owner.transform.position.x + cameraOffset.x + deltaZoomPosition.x,
			owner.transform.position.y + cameraOffset.y + deltaZoomPosition.y,
			owner.transform.position.z + cameraOffset.z + deltaZoomPosition.z
		);

		transform.position = Vector3.Lerp
		(
			transform.position,
			targetCameraPosition,
			smoothing * Time.deltaTime
		);

		SetFar();
	}

	public void ZoomIn()
	{
		
		if (zoomOffset < minZoomOffset)
		{
			return;
		}

		float zoomPowerDelta = zoomPower * Time.deltaTime;
		zoomOffset -= zoomPowerDelta;

		deltaZoomPosition = new Vector3
		(
			deltaZoomPosition.x + Mathf.Sin(Helper.DegreeToRadian(transform.eulerAngles.x)) * zoomPowerDelta,
			deltaZoomPosition.y - zoomPowerDelta,
			deltaZoomPosition.z + Mathf.Sin(Helper.DegreeToRadian(transform.eulerAngles.y)) * zoomPowerDelta
		);
	}

	public void ZoomOut()
	{
		if (zoomOffset > maxZoomOffset)
		{
			return;
		}

		float zoomPowerDelta = zoomPower * Time.deltaTime;
		zoomOffset += zoomPowerDelta;

		deltaZoomPosition = new Vector3
		(
			deltaZoomPosition.x - Mathf.Sin(Helper.DegreeToRadian(transform.eulerAngles.x)) * zoomPowerDelta,
			deltaZoomPosition.y + zoomPowerDelta,
			deltaZoomPosition.z - Mathf.Sin(Helper.DegreeToRadian(transform.eulerAngles.y)) * zoomPowerDelta
		);
	}

	private void SetFar()
	{
		const float ratio = 2.5f;
		float newFarClipPlane = transform.position.y * ratio;
		if (newFarClipPlane < maxFarClipPlane)
		{
			playerCamera.farClipPlane = newFarClipPlane;
		} else
		{
			playerCamera.farClipPlane = maxFarClipPlane;
		}
	}
}