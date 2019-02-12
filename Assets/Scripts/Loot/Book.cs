using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Loot
{
	private int thom = 0;
	private int pages = 0;
	private int readingPages = 0;
	private int buffPercent = 0;

	private string theme;

	public int Thom { get => thom; }
	public int Pages { get => pages; }
	public int ReadingPages { get => readingPages; set => readingPages = value; }

	public int BuffPercent { get => buffPercent; }
	public string Theme { get => theme; set => theme = value; }

	private enum BookTheme
	{
		Craft,
		Survival,
		Medicine,
		Protection,
		HandCombat,
		LongRangeCombat,
		Shooting,
		Farming,
		Cooking,
		Building,
		Journal
	}

	public Book()
	{
		thom = Random.Range(1, 6);
		
		var themes = System.Enum.GetNames(typeof(BookTheme));
		theme = themes[Random.Range(0, themes.Length)];

		name = "Book of " + theme.ToString() + ". Thom " + thom.ToString();

		SetPagesAndBuff(theme);
	}

	private void SetPagesAndBuff(string theme)
	{
		if (theme != "Journal")
		{
			switch (thom)
			{
				case 1:
					pages = Random.Range(70, 171);
					switch (CalculateBuff(pages, 170))
					{
						case 1:
							buffPercent = Random.Range(10, 13);
							break;
						case 2:
							buffPercent = Random.Range(12, 15);
							break;
						case 3:
							buffPercent = Random.Range(14, 17);
							break;
						case 4:
							buffPercent = Random.Range(16, 19);
							break;
						case 5:
							buffPercent = Random.Range(18, 21);
							break;
					}
					break;
				case 2:
					pages = Random.Range(170, 271);
					switch (CalculateBuff(pages, 270))
					{
						case 1:
							buffPercent = Random.Range(20, 23);
							break;
						case 2:
							buffPercent = Random.Range(22, 25);
							break;
						case 3:
							buffPercent = Random.Range(24, 27);
							break;
						case 4:
							buffPercent = Random.Range(26, 29);
							break;
						case 5:
							buffPercent = Random.Range(28, 31);
							break;
					}
					break;
				case 3:
					pages = Random.Range(270, 371);
					switch (CalculateBuff(pages, 370))
					{
						case 1:
							buffPercent = Random.Range(30, 33);
							break;
						case 2:
							buffPercent = Random.Range(32, 35);
							break;
						case 3:
							buffPercent = Random.Range(34, 37);
							break;
						case 4:
							buffPercent = Random.Range(36, 39);
							break;
						case 5:
							buffPercent = Random.Range(38, 41);
							break;
					}
					break;
				case 4:
					pages = Random.Range(370, 471);
					switch (CalculateBuff(pages, 470))
					{
						case 1:
							buffPercent = Random.Range(40, 43);
							break;
						case 2:
							buffPercent = Random.Range(42, 45);
							break;
						case 3:
							buffPercent = Random.Range(44, 47);
							break;
						case 4:
							buffPercent = Random.Range(46, 49);
							break;
						case 5:
							buffPercent = Random.Range(48, 51);
							break;
					}
					break;
				case 5:
					pages = Random.Range(470, 571);
					switch (CalculateBuff(pages, 570))
					{
						case 1:
							buffPercent = Random.Range(50, 53);
							break;
						case 2:
							buffPercent = Random.Range(52, 55);
							break;
						case 3:
							buffPercent = Random.Range(54, 57);
							break;
						case 4:
							buffPercent = Random.Range(56, 59);
							break;
						case 5:
							buffPercent = Random.Range(58, 61);
							break;
					}
					break;
				default:
					Debug.LogError("invalid thom in Book.Book(): " + thom);
					break;
			}
		}
		else
		{
			buffPercent = Random.Range(15, 26);
			pages = Random.Range(50, 76);
		}
	}

	private int CalculateBuff(int curPages, int maxPages)
	{
		int difference = maxPages - curPages;
		if (difference < 21)
		{
			return 1;
		}
		else if (difference > 20 && difference < 41)
		{
			return 2;
		}
		else if (difference > 40 && difference < 61)
		{
			return 3;
		}
		else if (difference > 60 && difference < 81)
		{
			return 4;
		}
		else
		{
			return 5;
		}
	}
}
