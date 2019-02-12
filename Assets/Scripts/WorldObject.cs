using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
	protected float mass;

	protected abstract void OnDestroy();
}

public struct Skill
{
	string name;
	int points;
}

public enum WeaponType 
{
	Melee,
	Ranged
}