using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : NPC
{
	[SerializeField]
	private float rotationSpeed = 20f;

	[SerializeField]
	private bool infectionEnabled;

	private bool isSetDefault;

	public bool InfectionEnabled { get => infectionEnabled; set => infectionEnabled = value; }

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!isDead)
		{
			if (targetCharacter && !targetCharacter.IsDead)
			{
				isSetDefault = false;
				float distance = DistanceToTarget();
				if (distance < (maxAttackRange - deltaAttackRange) && (!isAttacking || !isGrab))
				{
					navMeshAgent.enabled = false;
					if (isGrab)
					{
						attackRoutine = Attack();
						StartCoroutine(attackRoutine);
					}
					else if (!isGrabRoutineRunning)
					{
						grabRoutine = Grab();
						StartCoroutine(grabRoutine);
					}
				}
				else if ((maxAttackRange - deltaAttackRange) < distance && distance < maxAttackRange)
				{
					Move();
				}
				else if (distance >= maxAttackRange)
				{
					if (isGrab)
					{
						Ungrab();
					}

					if (isGrabRoutineRunning)
					{
						isGrabRoutineRunning = false;
						StopCoroutine(grabRoutine);
					}

					if (isAttacking)
					{
						isAttacking = false;

						if (attackRoutine != null)
						{
							StopCoroutine(attackRoutine);
						}
					}

					Move();
				}
				if (distance < maxAttackRange)
				{
					Rotate();
				}
			} else if (isSetDefault)
			{
				isSetDefault = true;
				StopAllCoroutines();
				isGrab = false;
				isGrabRoutineRunning = false;
				isAttacking = false;
				targetCharacter = null;
				navMeshAgent.isStopped = true;
				navMeshAgent.enabled = false;
			}
		} 
	}

	private void Rotate()
	{
		Helper.RotateToObject(this, targetCharacter);
		// Vector3 direction = Helper.GetDirection(this, targetCharacter);
		// Vector3 targetRotation = Helper.GetLookRotationY(this, direction);

		// transform.eulerAngles = Vector3.Lerp
		// (
		// 	transform.eulerAngles,
		// 	targetRotation,
		// 	rotationSpeed * Time.deltaTime
		// );
	}

	public override void FeelSmell(Vector3 smellLocaton)
	{
		if (targetCharacter == null || targetCharacter.IsDead ||
				Helper.IsFirstFarther(targetCharacter.transform.position, smellLocaton, transform.position))
		{
			SetNewDestination(smellLocaton);
			targetCharacter = null;
		}
	}

	public override void HearNoise(Vector3 noiseLocation)
	{
		if (targetCharacter == null || targetCharacter.IsDead ||
				Helper.IsFirstFarther(targetCharacter.transform.position, noiseLocation, transform.position))
		{
			SetNewDestination(noiseLocation);
			targetCharacter = null;
		}
	}

	public override void SeeCharacter(Character newCharacter)
	{
		if (!(newCharacter is Zombie) && newCharacter && !newCharacter.IsDead)
		{
			targetCharacter = newCharacter;
		}
	}

	protected override void Move()
	{
		if (targetCharacter)
		{
			SetNewDestination(targetCharacter.transform.position);
		}
	}

	protected override void Die()
	{
		base.Die();
		Destroy(gameObject);
	}

	protected override void OnDestroy()
	{
		Debug.Log("Zombie was destroyed");
	}

	public override IEnumerator Attack()
	{
		isAttacking = true;
		yield return new WaitForSeconds(attackSpeed);

		if (targetCharacter)
		{
			if (IsTargetNear())
			{
				targetCharacter.DecreaseHealth(10f);
			}
		}
		isAttacking = false;
	}
}
