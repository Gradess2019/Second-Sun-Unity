using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
	//Returns true if the first distance is farther than the second
	public static bool IsFirstFarther(Vector3 firstPosition, Vector3 secondPosition, Vector3 mainPointPosition)
	{
		return Vector3.Distance(firstPosition, mainPointPosition) >
			Vector3.Distance(secondPosition, mainPointPosition);
	}

	public static float RadianToDegree(float angleInRadian)
	{
		return angleInRadian * 180f / Mathf.PI;
	}

	public static float DegreeToRadian(float angleInDegree)
	{
		return angleInDegree * Mathf.PI / 180f;
	}

	public static float Distance(WorldObject first, WorldObject second)
	{
		return Vector3.Distance(first.transform.position, second.transform.position);
	}

	public static Vector3 GetDirection(WorldObject current, WorldObject target)
	{
		return target.transform.position - current.transform.position;
	}

	public static Vector3 GetLookRotationY(WorldObject obj, Vector3 direction)
	{
		return new Vector3
		(
			obj.transform.eulerAngles.x,
			Quaternion.LookRotation(direction).eulerAngles.y,
			obj.transform.eulerAngles.z
		);
	}

	public static void RotateToDirection(WorldObject obj, Vector3 direction)
	{
		obj.transform.eulerAngles = GetLookRotationY(obj, direction);
	}

	public static void RotateToObject(WorldObject current, WorldObject target)
	{
		Vector3 direction = GetDirection(current, target);
		RotateToDirection(current, direction);
	}
}