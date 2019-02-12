using System.Collections.Generic;
using UnityEngine;

public class SmellTrigger : BaseTrigger
{
	public override void CreateEvent()
	{
		for(int npcID = 0; npcID < overlappedNPCs.Count; npcID++) {
			if (!overlappedNPCs[npcID].IsDead)
			{
				overlappedNPCs[npcID].FeelSmell(transform.position);
			}
		}
	}
}
