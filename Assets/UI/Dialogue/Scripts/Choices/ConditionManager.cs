using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ChoiceLogic))]
public class ConditionManager : MonoBehaviour
{
    private List<Choice> _choices;
    private Dictionary<string, Choice> _choicesAndNames = new();

    // Conditions linked to items
    public enum ItemIDs { PianoCat, Rabbit, Rue, YuyukoBody, YuyukoHead }

    void Start()
    {
        _choices = GetComponent<ChoiceLogic>().choices;
        SetConditions();
    }

    void SetConditions()
    {
        foreach (Choice choice in _choices)
        {
            _choicesAndNames[choice.name] = choice;
        }

        // PUT ALL YOUR CHOICE CONDITIONS HERE BY CALLING SETCONDITION
        SetCondition("testChoice", new[] { true, true, true });
        SetCondition("choice1", new[] {true, true, true});
        SetCondition("pinChoice", new[] {true, true});
    }

    void SetCondition(string name, bool[] conditions)
    {
        try
        {
            KeyValuePair<string, bool[]> choiceConditions = new(name, conditions);
            _choicesAndNames[choiceConditions.Key].conditions = choiceConditions.Value;
        }
        catch (Exception e)
        {
            Debug.LogError($"Choice \"{name}\" wasn't found in choices list. Either there's a typo or the choice wasn't assigned to the choices list in ChoiceLogic script\n{e}");
        }
    }
}
