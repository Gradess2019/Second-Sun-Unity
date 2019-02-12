using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC : Character
{

	[SerializeField]
	protected float grabSpeed = 0.2f;

	protected NavMeshAgent navMeshAgent;
	protected Character targetCharacter;
	protected IEnumerator grabRoutine;
	protected bool isGrabRoutineRunning;

	public NavMeshAgent NavMeshAgent { get => navMeshAgent; }
	public Character TargetCharacter { get => targetCharacter; set => targetCharacter = value; }

	protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

	protected override void FixedUpdate() 
	{
		base.FixedUpdate();

		if (!isDead)
		{
			if (!navMeshAgent.hasPath)
			{
				navMeshAgent.enabled = false;
			}
		}
	}

    public bool SetNewDestination(Vector3 target)
    {
		navMeshAgent.enabled = true;
        return navMeshAgent.SetDestination(target);
    }

	public bool IsTargetNear()
	{
		return DistanceToTarget() < maxAttackRange;
	}

	public float DistanceToTarget()
	{
		return Helper.Distance(this, targetCharacter);
	}

    public abstract void HearNoise(Vector3 noiseLocation);

    public abstract void FeelSmell(Vector3 smellLocaton);

    public abstract void SeeCharacter(Character character);

	protected virtual IEnumerator Grab()
	{
		isGrabRoutineRunning = true;
		yield return new WaitForSeconds(grabSpeed);
		isGrab = true;
		targetCharacter.GrabNum++;
		isGrabRoutineRunning = false;
	}

	protected virtual void Ungrab()
	{
		if (isGrabRoutineRunning)
		{
			StopCoroutine(grabRoutine);
			isGrabRoutineRunning = false;
		}
		isGrab = false;

		if (targetCharacter) 
		{
			targetCharacter.GrabNum--;
		}
	}

	protected override void Die()
	{
		base.Die();
		if (isGrab || isGrabRoutineRunning)
		{
			Ungrab();
		}
		
		navMeshAgent.enabled = false;
	}

}
