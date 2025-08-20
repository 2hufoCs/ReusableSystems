using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
	// REPLACE THIS PATH WITH WHATEVER PATH YOUR TXT FILE IS LOCATED
	public string textPath = "/Assets/Dialogue/TextFiles/DialogueContent.txt";

	private InputDecoder inputDecoder;
	[SerializeField] private TextAsset dialogue;
	// [SerializeField] private Image fadeScreen;

	private List<string> dialogueLines = new();
	private List<string> conversationList = new();
	private string convName;
	private List<int> convIndexList = new();
	private List<string> convToOutput = new();

	private bool dialogueTriggered = false;


	// Start is called before the first frame update
	void Start()
	{
		inputDecoder = GetComponent<InputDecoder>();

		// Get the text file that contains all of the dialogue
		string textFilePath = @$"{Directory.GetCurrentDirectory()}" + textPath;

		// Making sure the file path is actually correct
		try
		{
			dialogueLines.AddRange(File.ReadAllLines(textFilePath));
		}
		catch (Exception e)
		{
			Debug.LogError($"There was an error reading the lines of the dialogue file. Either the file path is wrong, or there are forbidden characters in the file \n file path: {textFilePath} \nOriginal exception message: {e}");
		}

		// Keeping track of all conversations
		for (int i = 0; i < dialogueLines.Count; i++)
		{
			// Ignore empty lines and comments
			if (IsEmpty(dialogueLines[i]) || (dialogueLines[i][0] == '/' && dialogueLines[i][1] == '/'))
			{
				dialogueLines.RemoveAt(i);
				i -= 1;
				continue;
			}


			// Conversations start with the hashtag symbol, we keep track of them
			if (dialogueLines[i][0] == '#')
			{
				convIndexList.Add(i);
				conversationList.Add(dialogueLines[i]);
			}
		}
	}

	public void LoadConversation(string targetConversation)
	{
		// Looking for every conversation in the text file
		for (int i = 0; i < conversationList.Count; i++)
		{
			string conversation = conversationList[i];

			// Whenever we find a new one, we compare its name to the one we want to initiate
			if (GetConversationName(conversation) != targetConversation)
			{
				continue;
			}
			convName = GetConversationName(conversation);

			// Getting the bounds of the conversation
			int convStartIndex = convIndexList[i] + 1;
			bool reachedLastConversation = convIndexList[i] == convIndexList.Max();
			int convEndIndex = reachedLastConversation ? dialogueLines.Count - 1 : convIndexList[i + 1] - 1;

			// Exctracting the conversation from the dialogue content
			convToOutput = dialogueLines.GetRange(convStartIndex, convEndIndex - convStartIndex + 1);

			// Show the first line of the dialogue
			gameObject.SetActive(true);
			dialogueTriggered = true;

			ShowNextLine();
			return;
		}

		// If we haven't hit a return after the for loop, then the conversation doesn't exist
		Debug.LogError($"Tried to load conversation \"{targetConversation}\" but it wasn't found in the conversation list. There is most likely a typo somewhere.");
		foreach (string conv in conversationList)
		{
			Debug.Log(conv);
		}
	}

	private string GetConversationName(string conversation)
	{
		// Remove the hashtag and the empty spaces to get only the name of the conv
		conversation = conversation.Replace("#", "");
		conversation = conversation.Replace(" ", "");
		return conversation;
	}

	private bool IsEmpty(string str)
	{
		// Will return true if the string doesn't have any characters or only spaces
		if (str.Length == 0)
			return true;

		str.Replace(" ", "");
		if (str.Length == 0)
			return true;

		return false;
	}

	void Update()
	{
		if (!dialogueTriggered)
		{
			return;
		}

		// Show next line of dialogue once the player hits the confirm button
		if (Input.GetButtonDown("Submit"))
		{
			ShowNextLine();
		}
	}

	private void ShowNextLine()
	{
		// If we've reached the end of the conversation, remove text, sprite and dialogue box from view
		if (convToOutput.Count == 0)
		{
			inputDecoder.dialogueText.text = "";
			inputDecoder.characterImage = null;
			gameObject.SetActive(false);

			dialogueTriggered = false;

			return;
		}

		string textLine = convToOutput[0];
		convToOutput.RemoveAt(0);
		inputDecoder.ParseInputLine(textLine);
	}

	// Fadeins and fadeouts, not useful rn but maybe later

	// private IEnumerator Fadeout(float timer)
	// {
	// 	float secondsPassed = 0;
	// 	Color fadeColor = fadeScreen.color;
	// 	while (secondsPassed < timer)
	// 	{
	// 		fadeColor = fadeScreen.color;
	// 		fadeScreen.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, secondsPassed / timer);

	// 		secondsPassed += Time.deltaTime;
	// 		yield return null;
	// 	}
	// 	fadeScreen.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);

	// }

	// private IEnumerator FadeIn(float timer)
	// {
	// 	float secondsPassed = 0;
	// 	while (secondsPassed < timer)
	// 	{
	// 		Color fadeColor = fadeScreen.color;
	// 		fadeScreen.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1 - (secondsPassed / timer));

	// 		secondsPassed += Time.deltaTime;
	// 		yield return null;
	// 	}
	// }
}