using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConvCheck : MonoBehaviour
{
	/// This script checks and loads conversation when you click submit near them.
	/// This might not be useful at all if you're making for instance a visual novel, 
	/// where you just trigger the conversation when pressing enter.

	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private ConversationManager convManager;

	[SerializeField] private LayerMask convTriggerLayer;


	void Update()
	{
		if (Input.GetButtonDown("Submit") && !dialogueBox.activeSelf)
		{
			// Make overlap circle and see if any conversation trigger was hit
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1, convTriggerLayer);
			if (hits.Length == 0)
				return;

			// Take only the first one if there are many
			Collider2D colChosen = hits[0];
			ConvTrigger convTrigger = colChosen.GetComponent<ConvTrigger>();
			if (convTrigger.triggered)
			{
				return;
			}

			Debug.Log("Successfully loading conversation");

			convTrigger.triggered = true;
			convManager.LoadConversation(convTrigger.conversationName);
		}
	}
}
