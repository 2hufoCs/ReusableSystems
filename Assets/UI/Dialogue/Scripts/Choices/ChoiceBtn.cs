using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBtn : MonoBehaviour
{
    public string response;
    [HideInInspector] public ConversationManager convManager;

    public void OnClick()
    {
        convManager.LoadConversation(response);
        convManager.selectingChoice = false;
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
