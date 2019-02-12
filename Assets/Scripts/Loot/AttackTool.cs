using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTool : Tool
{
	[SerializeField]
	private float damage;

	[SerializeField]
	private float attackSpeed;

	[SerializeField]
	private float attackRange;

	[SerializeField]
	private int angle;

	[SerializeField]
	private int numOfRays = 1;

	[SerializeField]
	private WeaponType type;

	public float Damage { get => damage; set => damage = value; }
	public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
	public float AttackRange { get => attackRange; set => attackRange = value; }
	public int Angle { get => angle; set => angle = value; }
	public WeaponType Type { get => type; set => type = value; }
	public int NumOfRays { get => numOfRays; set => numOfRays = value; }
}
