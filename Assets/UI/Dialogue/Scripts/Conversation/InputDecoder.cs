using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.ComponentModel;
using TMPro;
using UnityEngine.Lumin;

public class InputDecoder : MonoBehaviour
{
	/// <summary>
	///  Reads the given line of conversation, print it to the dialogue box and executes commands if there are any
	///  ie changing backgrounds, changing sprite of person who's speaking, etc
	/// </summary>

	public List<Character> characterList = new();
	[SerializeField] float textSpeed;
	[SerializeField] float skipSpeed;
	[SerializeField] ChoiceLogic choiceLogic;
	public TextMeshProUGUI dialogueText;
	public Image characterImage;

	public bool writingLine = false;
	bool pausingLine = false;
	float pausingTime = .1f;


	public void ParseInputLine(string stringToParse)
	{
		// Check if the line has brackets at all
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

	public void Say(string text, Sprite sprite)
	{
		characterImage = gameObject.transform.Find("CharacterSprite").GetComponent<Image>();
		characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 1);

		StartCoroutine(PrintLine(text));
		characterImage.sprite = sprite;
	}

	public void Say(string text)
	{
		characterImage = gameObject.transform.Find("CharacterSprite").GetComponent<Image>();
		characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 0);

		StartCoroutine(PrintLine(text));
		characterImage.sprite = null;
	}

	IEnumerator PrintLine(string text)
	{
		writingLine = true;
		float cps = 1 / textSpeed; // characters per second

		string currentText = "";
		for (int i = 0; i < text.Length; i++)
		{
			// If player clicks when line is writing, just show the rest
			if (!writingLine)
			{
				// cps = 1 / skipSpeed; // just go super fast
				currentText += text.Substring(i, text.Length - i);

				dialogueText.text = currentText;
				break;
			}

			if (text[i] == '{')
			{
				i = ParseCurlyBrackets(text, i);
				continue;
			}

			if (pausingLine && writingLine)
			{
				yield return new WaitForSeconds(pausingTime);
				pausingLine = false;
			}

			currentText += text[i];
			if (writingLine)
			{
				dialogueText.text = currentText;
			}
			yield return new WaitForSeconds(cps);
		}
		dialogueText.text = currentText;

		writingLine = false;
	}

	int ParseCurlyBrackets(string text, int openingIndex)
	{
		int closingIndex = text.IndexOf("}", openingIndex);

		if (closingIndex < 0)
		{
			Debug.LogError("Opening curly bracket was not closed; please fix typo in dialogue text file!!");
		}

		string command = text[(openingIndex + 1)..closingIndex];

		// If command is a value, pause dialogue for that value
		if (float.TryParse(command, out float value))
		{
			pausingLine = true;
			pausingTime = value;
		}


		string[] splitCommand = command.Split(' ');

		// Two-word commands
		if (splitCommand.Length == 2)
		{
			// If command is a choice, try to load choice
			if (splitCommand[0] == "choice")
			{
				choiceLogic.LoadChoice(splitCommand[1]);
			}
		}



			return closingIndex;
	}

	string RemoveCurlyBrackets(string text)
	{
		string formattedText = "";
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == '{')
			{
				int closingIndex = text.IndexOf("}", i);

				if (closingIndex < 0)
				{
					Debug.LogError("Opening curly bracket was not closed; please fix typo in dialogue text file!!");
				}

				i = closingIndex;
				continue;
			}
			formattedText += text[i];
		}
		return formattedText;
	}

	#endregion
}