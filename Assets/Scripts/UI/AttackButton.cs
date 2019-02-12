using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : Button
{
	[SerializeField]
	private Character owner;

	private void FixedUpdate() 
	{
		if (isPressed)
		{
			if (!owner.IsAttacking)
			{
				owner.AttackRoutine = owner.Attack();
				StartCoroutine(owner.AttackRoutine);
			}
		} else
		{
			if (owner.IsAttacking)
			{
				owner.IsAttacking = false;
				StopCoroutine(owner.AttackRoutine);
			}
		}
	}
}
