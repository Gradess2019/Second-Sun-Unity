using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisionTrigger : MonoBehaviour
{
	[SerializeField]
	private float forgettingTime = 2f;

	[SerializeField]
	private float forgettingDistance = 10f;

	private bool isForgettingRoutineRunning;
	private NPC owner;

	private MeshCollider trigger;

	private List<Character> overlappedCharacters;

	private Character lastCharacter;
	
	private IEnumerator forgettingRoutine;

	private void Awake()
	{
		trigger = GetComponent<MeshCollider>();
		overlappedCharacters = new List<Character>();
		owner = GetComponentInParent<NPC>();
	}

	private void Update() {
		if (!owner.IsDead)
		{
			FindNearestCharacter();
		}
	}

	private void FindNearestCharacter()
	{
		owner.NavMeshAgent.enabled = true;
		Character nearestCharacter = null;

		float smallestDistance = -1f;

		for (int id = 0; id < overlappedCharacters.Count; id++)
		{
			RaycastHit hit;
			Ray ray = new Ray(transform.position, overlappedCharacters[id].transform.position - transform.position);
			LayerMask mask = LayerMask.GetMask("Raycast Block");
			
			if (Physics.Raycast(ray, out hit, 40f, mask))
			{				
				if (hit.collider.GetComponent<Character>())
				{
					float distance = -1f;
					NavMeshPath path = new NavMeshPath();
					owner.NavMeshAgent.CalculatePath(overlappedCharacters[id].transform.position, path);

					if (path.corners.Length > 0)
					{
						distance = Vector3.Distance(transform.position, path.corners[0]);
						for (int i = 1; i < path.corners.Length; i++)
						{
							distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
						}
					}

					if (smallestDistance == -1f || nearestCharacter == null || smallestDistance > distance)
					{
						smallestDistance = distance;
						nearestCharacter = overlappedCharacters[id];
					} 
				}
			}
		}

		if (nearestCharacter)
		{
			if (isForgettingRoutineRunning)
			{
				isForgettingRoutineRunning = false;
				StopCoroutine(forgettingRoutine);
			}

			lastCharacter = nearestCharacter;
		} else if (!isForgettingRoutineRunning && lastCharacter != null && Helper.Distance(owner, lastCharacter) > forgettingDistance)
		{
			forgettingRoutine = ForgetTarget();
			StartCoroutine(forgettingRoutine);
		}

		if (lastCharacter && lastCharacter.IsDead)
		{
			if (overlappedCharacters.Contains(lastCharacter))
			{
				overlappedCharacters.Remove(lastCharacter);;
			}
			lastCharacter = null;
			if (isForgettingRoutineRunning)
			{
				StopCoroutine(forgettingRoutine);
			}
		}
		owner.SeeCharacter(lastCharacter);
	}

	private void OnTriggerEnter(Collider other)
	{
		Character newCharacter = other.GetComponent<Character>();

		if (newCharacter is Character && newCharacter.GetType() != owner.GetType())
		{
			overlappedCharacters.Add(newCharacter);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Character leavedCharacter = other.GetComponent<Character>();
		if (leavedCharacter is Character && leavedCharacter.GetType() != owner.GetType())
		{
			overlappedCharacters.Remove(leavedCharacter);
		}
	}

	private IEnumerator ForgetTarget()
	{
		isForgettingRoutineRunning = true;
		yield return new WaitForSeconds(forgettingTime);
		lastCharacter = null;
		isForgettingRoutineRunning = false;
	}
}
