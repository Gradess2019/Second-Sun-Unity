using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : WorldObject
{
	[SerializeField]
	protected float health;

	[SerializeField]
	protected float speed;

	[SerializeField]
	protected float maxNoiseRadius;

	[SerializeField]
	protected float maxSmellRadius;

	[SerializeField]
	protected float maxAttackRange = 3f;

	[SerializeField]
	protected float deltaAttackRange = 2f;

	[SerializeField]
	protected float attackSpeed = 1f;

	protected float noiseRadius;
	protected float smellRadius;

	protected CharacterController controller;

	protected bool isAttacking;
	protected bool isDead;
	protected bool isGrab;
	protected int grabNum;

	protected IEnumerator attackRoutine;

	public float Health { get => health; set => health = value; }

	public bool IsDead { get => isDead; set => isDead = value; }
	public bool IsGrab { get => isGrab; set => isGrab = value; }
	public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

	public int GrabNum { get => grabNum; set => grabNum = value; }
	
	public IEnumerator AttackRoutine { get => attackRoutine; set => attackRoutine = value; }

	protected virtual void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	protected virtual void Update()
	{
		if (!isDead && health <= 0)
		{
			Die();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!controller.isGrounded)
		{
			controller.Move(Vector3.up * -9.8f * Time.deltaTime);
		}
	}

	public void DecreaseHealth(float healthDecrement)
	{
		if (health > 0f)
		{
			health -= healthDecrement;
		} else if (health < 0f)
		{
			health = 0f;
		}
	}

	protected virtual void Die()
	{
		isDead = true;
	}

	protected bool IsCaught()
	{
		return grabNum > 0;
	}

	protected abstract void Move();

	public abstract IEnumerator Attack();
}
