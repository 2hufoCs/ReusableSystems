using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character", fileName = "NewCharacter")]
public class Character : ScriptableObject
{
	new public string name;
	public List<Sprite> sideImages = new();

	public Character(string nameInput, List<Sprite> sideImagesInput)
	{
		this.name = nameInput;
		this.sideImages = sideImagesInput;

		CheckName();
	}

	public Character(string nameInput)
	{
		this.name = nameInput;
		this.sideImages = null;

		CheckName();
	}

	public void CheckName()
	{
		if (this.name == null)
		{
			throw new System.Exception("name must contain a string!");
		}
	}
}
