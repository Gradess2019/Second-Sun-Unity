using System.Collections.Generic;
using UnityEngine;

public class NoiseTrigger : BaseTrigger
{
	public override void CreateEvent()
	{
		for(int npcID = 0; npcID < overlappedNPCs.Count; npcID++) {
			if (!overlappedNPCs[npcID].IsDead)
			{
				overlappedNPCs[npcID].HearNoise(transform.position);
			}
		}
	}
}
