using UnityEngine;

public class Loot : WorldObject
{
	protected new string name = "";

	public string LootName { get => name; }

	public static Loot CreateLootObject(string className)
	{
		switch (className)
		{
			case "AttackTool":
				Loot attackTool = new AttackTool();
				return attackTool;
			case "Book":
				Loot book = new Book();
				return book;
			case "Clothes":
				Loot clothes = new Clothes();
				return clothes;
			case "Detail":
				Loot detail = new Detail();
				return detail;
			case "Food":
				Loot food = new Food();
				return food;
			case "Garbage":
				Loot garbage = new Garbage();
				return garbage;
			case "OrdinaryTool":
				Loot ordinaryTool = new OrdinaryTool();
				return ordinaryTool;
			default:
				Debug.LogError("invalid className in Loot.createLootObject");
				return null;
		}
	}

	protected override void OnDestroy()
	{
		Debug.Log(this.GetType() + " was destroyed");
	}
}
