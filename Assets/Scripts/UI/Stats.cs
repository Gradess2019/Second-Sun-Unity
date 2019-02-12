using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
	[SerializeField]
	private Player player;

	[SerializeField]
	private Text staminaText;

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Text energyText;

	private void FixedUpdate()
	{
		staminaText.text = "Stamina = " + player.Stamina.ToString();
		healthText.text = "Health = " + player.Health.ToString();
		energyText.text = "Energy = " + player.Energy.ToString();
	}

}
