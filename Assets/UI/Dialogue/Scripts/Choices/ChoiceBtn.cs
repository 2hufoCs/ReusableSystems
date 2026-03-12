using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBtn : MonoBehaviour
{
    [HideInInspector] public Choice parentChoice;
    public string response;
    [HideInInspector] public ConversationManager convManager;

    public void OnClick()
    {
        convManager.LoadConversation(response);
        convManager.selectingChoice = false;

        Debug.Log(parentChoice.name);
        Debug.Log(response);
        parentChoice.OnChoose(response);
        DestroySiblings();
    }

    void DestroySiblings()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == this)
                continue;
            Destroy(transform.parent.GetChild(i).gameObject);
        }
        Destroy(this);
    }
}
