using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.ComponentModel;
using TMPro;

public class InputDecoder : MonoBehaviour
{
	/// <summary>
	///  Reads the given line of conversation, print it to the dialogue box and executes commands if there are any
	///  ie changing backgrounds, changing sprite of person who's speaking, etc
	/// </summary>

	public List<Character> characterList = new();
	public TextMeshProUGUI dialogueText;
	public Image characterImage;


	public void ParseInputLine(string stringToParse)
	{
		// We check if there's text between brackets in the string. 
		int openBracketIndex = stringToParse.IndexOf("[");
		int closeBracketIndex = stringToParse.IndexOf("]");
		if (openBracketIndex == -1 || closeBracketIndex == -1)
		{
			throw new Exception($"Conversation line failed to load because the brackets are missing. Make sure they are located at the beginning of the dialogue line.");
		}

		// Extract the string inside the brackets
		string bracketsString = stringToParse.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex);
		string[] bracketWords = bracketsString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
		bracketWords[^1] = bracketWords[^1].Replace("]", "");

		// If there's nothing inside the brackets, it means it's just the protagonist's thoughts. Just say the line
		if (bracketWords[0] == "")
		{
			stringToParse = stringToParse.Remove(0, stringToParse.IndexOf("]") + 1);
			Say(stringToParse);
			return;
		}

		// There should be 2 words inside the brackets: the character's name and the sprite to use. If there aren't 2 words, throw an error
		if (bracketWords.Length != 2)
		{
			throw new Exception($"Dialogue error: the info given in the brackets of the string to parse is wrong! Number of words is {bracketWords.Length} instead of 2. \nContent: \"{bracketsString}\" \nstringToParse: {stringToParse}");
		}
		string characterName = bracketWords[0];
		string imgName = bracketWords[1];

		// Checking if input character is valid (throw an error if false)
		if (!IsCharacterExisting(characterName))
		{
			throw new Exception($"The character {characterName} isn't in the character list! Either it's a typo or you forgot to add them in the list.");
		}
		Character character = GetCharacter(characterName);

		// Checking if input sprite is valid (throw an error if false)
		if (!IsImageExisting(imgName, character))
		{
			throw new Exception($"The sprite {imgName} doesn't exist among the sprites of the character {characterName}. Have you made a typo or forgotten to add it to the sprite list?");
		}
		Sprite sprite = GetImage(imgName, character);

		stringToParse = stringToParse.Remove(0, stringToParse.IndexOf("]") + 1);
		SplitToSay(stringToParse, sprite);

	}

	private bool IsCharacterExisting(string characterName)
	{
		foreach (Character character in characterList)
		{
			if (characterName == character.name)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsImageExisting(string imgName, Character character)
	{
		foreach (Sprite sideImage in character.sideImages)
		{
			if (imgName == sideImage.name)
			{
				return true;
			}
		}
		return false;
	}

	private Character GetCharacter(string characterName)

	{
		foreach (Character character in characterList)
		{
			if (characterName == character.name)
			{
				return character;
			}
		}
		return null;
	}

	private Sprite GetImage(string imgName, Character character)
	{
		foreach (Sprite sideImage in character.sideImages)
		{
			if (imgName == sideImage.name)
			{
				return sideImage;
			}
		}
		return null;
	}


	#region Say Stuff

	public void SplitToSay(string stringToParse, Sprite sprite)
	{
		int startOfQuote = stringToParse[0] == ' ' ? 1 : 0;
		int endOfQuote = stringToParse.Length;
		string stringToOutput = stringToParse.Substring(startOfQuote, endOfQuote - startOfQuote);

		Say(stringToOutput, sprite);
	}

	public void Say(string what, Sprite sprite)
	{
		characterImage = gameObject.transform.Find("CharacterSprite").GetComponent<Image>();
		characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 1);

		dialogueText.text = what;
		characterImage.sprite = sprite;
	}

	public void Say(string what)
	{
		characterImage = gameObject.transform.Find("CharacterSprite").GetComponent<Image>();
		characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 0);

		dialogueText.text = what;
		characterImage.sprite = null;
	}

	#endregion
}