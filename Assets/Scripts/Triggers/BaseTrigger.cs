using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseTrigger : MonoBehaviour
{
	protected List<NPC> overlappedNPCs;
	protected SphereCollider trigger;

	private void Awake() {
		trigger = GetComponent<SphereCollider>();
	}

	protected void Start() {
		overlappedNPCs = new List<NPC>();
	}

	public void ClearList()
	{
		overlappedNPCs.Clear();
	}

	public void SetTriggerRadius(float newRadius) {
		if (trigger) {
			trigger.radius = newRadius;
		} else {
			throw new System.NullReferenceException("BaseEvent trigger is null!");
		}
	}

	public abstract void CreateEvent();

	protected void OnTriggerEnter(Collider other) {
		NPC overlappedNPC = other.GetComponent<NPC>();
		if (overlappedNPC is NPC) {
			overlappedNPCs.Add(overlappedNPC);
		}
		
	}

	protected void OnTriggerExit(Collider other) {
		NPC leavedNPC = other.GetComponent<NPC>();
		if (leavedNPC is NPC) {
			overlappedNPCs.Remove(leavedNPC);
		}
	}

	
}
