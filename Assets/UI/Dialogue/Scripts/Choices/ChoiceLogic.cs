
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
    private Dictionary<string, Choice> choicesAndNames = new();
    public bool hasCoat = true;
    private VerticalLayoutGroup choiceLayout;

    void Start()
    {
        choiceLayout = GetComponent<VerticalLayoutGroup>();
        SetConditions();
    }

    void SetConditions()
    {
        foreach (Choice choice in choices)
        {
            choicesAndNames[choice.name] = choice;
        }

        // PUT ALL YOUR CHOICE CONDITIONS HERE BY CALLING SETCONDITION
        SetCondition("testChoice", new[] { true, true, hasCoat });
    }

    public void LoadChoice(string name)
    {
        Choice choiceToLoad = FindChoice(name);

        if (choiceToLoad == null)
        {
            Debug.LogError($"Choice \"{name}\" doesn't exist in the choiceLogic script. Please check whether it's a typo or you forgot to add it");
            return;
        }

        // Get all available options based on conditions
        List<string> options = new();
        List<string> responses = new();
        for (int i = 0; i < choiceToLoad.options.Length; i++)
        {
            if (choiceToLoad.conditions[i])
            {
                options.Add(choiceToLoad.options[i]);
                responses.Add(choiceToLoad.responses[i]);
            }
        }

        // Show those options to the screen
        ShowChoice(options, responses);
        convManager.selectingChoice = true;
    }

    void ShowChoice(List<string> options, List<string> responses)
    {
        float buttonHeight = choiceUIPrefab.GetComponent<RectTransform>().rect.height;
        int spacing = (int)(lowestChoiceUI * mainCam.pixelHeight / (options.Count + 1) - buttonHeight);
        choiceLayout.spacing = spacing;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GameObject emptyObj1 = Instantiate(choiceUIPrefabEmpty);
        emptyObj1.transform.SetParent(transform, false);

        for (int i = 0; i < options.Count; i++)
        {
            // Make new choice button and set its parent
            GameObject optionText = Instantiate(choiceUIPrefab);
            optionText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = options[i];
            optionText.transform.SetParent(transform, false);

            // Assign a few variables so it'll load the right conversation when clicked
            ChoiceBtn choiceBtn = optionText.GetComponent<ChoiceBtn>();
            choiceBtn.response = responses[i];
            choiceBtn.convManager = convManager;
        }

        GameObject emptyObj2 = Instantiate(choiceUIPrefabEmpty);
        emptyObj2.transform.SetParent(transform, false);
    }

    void SetCondition(string name, bool[] conditions)
    {
        try
        {
            KeyValuePair<string, bool[]> choiceConditions = new(name, conditions);
            choicesAndNames[choiceConditions.Key].conditions = choiceConditions.Value;
        }
        catch (Exception e)
        {
            Debug.LogError($"Choice \"{name}\" wasn't found in choices list. Either there's a typo or the choice wasn't assigned to the choices list in ChoiceLogic script\n{e}");
        }
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