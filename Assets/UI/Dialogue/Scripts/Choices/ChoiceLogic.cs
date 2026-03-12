
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(VerticalLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class ChoiceLogic : MonoBehaviour
{
    public List<Choice> choices;
    [SerializeField] private GameObject choiceUIPrefab;
    [SerializeField] private GameObject choiceUIPrefabEmpty;

    [SerializeField] private float lowestChoiceUI = 2f / 3f;
    [SerializeField] private Camera mainCam;
    [SerializeField] private ConversationManager convManager;
    public bool hasCoat = true;
    private VerticalLayoutGroup choiceLayout;

    List<Action> choiceActions = new();

    void Start()
    {
        choiceLayout = GetComponent<VerticalLayoutGroup>();
    }

    public void LoadChoice(string name)
    {
        Debug.Log("loading choice");
        Choice choiceToLoad = FindChoice(name);

        if (choiceToLoad == null)
        {
            Debug.LogError($"Choice \"{name}\" doesn't exist in the choiceLogic script. Please check whether it's a typo or you forgot to add it");
            return;
        }

        // Get all available options based on conditions
        // List<string> options = new();
        // List<string> responses = new();
        // for (int i = 0; i < choiceToLoad.options.Length; i++)
        // {
        //     if (choiceToLoad.conditions[i])
        //     {
        //         options.Add(choiceToLoad.options[i]);
        //         responses.Add(choiceToLoad.responses[i]);
        //     }
        // }

        // Show those options to the screen
        ShowChoice(choiceToLoad);
        convManager.selectingChoice = true;

        // Set responses
        ResponseManager.SetChoiceResponse(choiceToLoad);
    }

    void ShowChoice(Choice choice)
    {
        float buttonHeight = choiceUIPrefab.GetComponent<RectTransform>().rect.height;
        int spacing = (int)(lowestChoiceUI * mainCam.pixelHeight / (choice.options.Length + 1) - buttonHeight);
        choiceLayout.spacing = spacing;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GameObject emptyObj1 = Instantiate(choiceUIPrefabEmpty);
        emptyObj1.transform.SetParent(transform, false);

        for (int i = 0; i < choice.options.Length; i++)
        {
            // Make new choice button and set its parent
            GameObject optionText = Instantiate(choiceUIPrefab);
            optionText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choice.options[i];
            optionText.transform.SetParent(transform, false);

            // If conditions not met, can't interact with button
            if (!choice.conditions[i])
            {
                optionText.GetComponent<Button>().interactable = false;
            }

            // Assign a few variables so it'll load the right conversation when clicked
            ChoiceBtn choiceBtn = optionText.GetComponent<ChoiceBtn>();
            choiceBtn.response = choice.responses[i];
            choiceBtn.convManager = convManager;

            choiceBtn.parentChoice = choice;
            Debug.Log(choiceBtn.parentChoice);
        }

        GameObject emptyObj2 = Instantiate(choiceUIPrefabEmpty);
        emptyObj2.transform.SetParent(transform, false);
    }

    Choice FindChoice(string name)
    {
        foreach (Choice choice in choices)
        {
            if (choice.name == name)
            {
                return choice;
            }
        }
        return null;
    }

    #region Responses

    public void ClickChoice(string response)
    {
        convManager.LoadConversation(response);
    }

    // public void TestChoice1()
    // {
    //     convManager.LoadConversation("Scene2Bedroom");
    // }

    // public void TestChoice2()
    // {
    //     convManager.LoadConversation("Scene2Mirror");
    // }

    // public void TestChoice3()
    // {
    //     convManager.LoadConversation("Scene3Library");
    // }



    #endregion
}