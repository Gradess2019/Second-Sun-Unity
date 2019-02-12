using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootViewer : MonoBehaviour
{
	[SerializeField]
	private Text viewerText;

	private void Start()
	{
		viewerText = GetComponent<Text>();
		viewerText.text = "";
	}

	public void AddContainerToView(Container container)
	{
		viewerText.text += container.name + ":";
		foreach (var item in container.containerLoot)
		{
			var itemType = item.GetType();
			viewerText.text += "\n- " + item.LootName;

			switch (itemType.ToString())
			{
				case "AttackTool":
					var attackTool = (AttackTool) item;
					break;
				case "OrdinaryTool":
					var ordinaryTool = (OrdinaryTool) item;
					break;
				case "Book":
					var book = (Book) item;
					viewerText.text +=
						"\n\t Pages: " + book.Pages +
						"\n\t Reading pages: " + book.ReadingPages +
						"\n\t Buff percent: " + book.BuffPercent;
					break;
				case "Clothes":
					var clothes = (Clothes) item;
					break;
				case "Detail":
					var detail = (Detail) item;
					break;
				case "Food":
					var food = (Food) item;
					viewerText.text +=
						"\n\t Hunger buff: " + food.HungerBuff + 
						"\n\t Water buff: " + food.WaterBuff;
					break;
				case "Garbage":
					var garbage = (Garbage) item;
					break;
				default:
					Debug.LogError("Invalid item type in LootViewer.addContainerToView()" + itemType);
					break;
			}
		}
	}

	public void RemoveContainerFromView(Container container)
	{
		viewerText.text = "";
	}
}
