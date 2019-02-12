using System.Collections.Generic;
using UnityEngine;

public class Container : WorldObject
{
	public int containerCapacity;

	[SerializeField]
	private ContainerType type;

	[SerializeField]
	private List<string> containerCanSpawn = new List<string>();

	private List<Loot> lootList = new List<Loot>();

	public List<Loot> containerLoot { get => lootList; }
	public ContainerType Type { get => type; set => type = value; }

	public enum ContainerType
	{
		Floor,
		Room,
		Kithchen,
		Bathroom,
		Garage,
		Office,
		Workshop,
		ToolsShop,
		WeaponShop,
		ClothingStore,
		HardwareStore,
		StoreHouse
	}

	private void Awake()
	{
		spawnLoot();
	}

	public void spawnLoot()
	{
		switch (type)
		{
			case ContainerType.Room:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.Kithchen:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.Bathroom:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.Garage:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.Office:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.Workshop:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.ToolsShop:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.WeaponShop:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.ClothingStore:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.HardwareStore:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
			case ContainerType.StoreHouse:
				foreach (var item in containerCanSpawn)
				{
					containerLoot.Add(Loot.CreateLootObject(item));
				}
				break;
		}
	}

	protected override void OnDestroy()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();

		if (player is Player)
		{
			player.AddContainer(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Player player = other.GetComponent<Player>();

		if (player is Player)
		{
			player.RemoveContainer(this);
		}
	}
}
