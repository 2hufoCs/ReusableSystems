using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvTrigger : MonoBehaviour
{
	public string conversationName;
	[SerializeField] ConversationManager convManager;
	// For now, conversations can only be triggered once. change that later
	[HideInInspector] public bool triggered = false;

	void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}

		if (triggered)
		{
			return;
		}

		Debug.Log("Successfully loading conversation");

		triggered = true;
		convManager.LoadConversation(conversationName);
	}
}
