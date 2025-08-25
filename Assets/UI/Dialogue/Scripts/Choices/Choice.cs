using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Choice")]
public class Choice : ScriptableObject
{
    public new string name;
    public string[] options = new string[3];
    [HideInInspector] public bool[] conditions = new bool[3];
    public string[] responses = new string[3];

    public Choice(string name, string[] options, bool[] conditions, string[] responses)
    {
        if (options.Length != conditions.Length || options.Length != responses.Length)
        {
            Debug.LogError($"Couldn't load choice named \"{name}\" because there aren't the same number of options, conditions and/or responses\n{options.Length} options: {options}\n{conditions.Length} conditions: {conditions}\n{responses.Length} responses: {responses}");
        }

        this.name = name;
        this.options = options;
        this.conditions = conditions;
        this.responses = responses;
    }

    public Choice(string name, string[] options, string[] responses)
    {
        if (options.Length != responses.Length)
        {
            Debug.LogError($"Couldn't load choice named \"{name}\" because there aren't the same number of options, conditions and/or responses\n{options.Length} options: {options}\n{responses.Length} responses: {responses}");
        }

        this.name = name;
        this.options = options;
        this.responses = responses;
    }
}
