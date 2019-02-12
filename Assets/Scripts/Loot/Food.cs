using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Loot
{
	private float hungerBuff = 0f;
	private float waterBuff = 0f;

	public float HungerBuff { get => hungerBuff; }
	public float WaterBuff { get => waterBuff; }

	public Food()
	{
		name = "Some food";
		hungerBuff = Random.Range(10, 650);
		waterBuff = Random.Range(10, 650);
	}
}
