using System;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
//using UnityEngine.Rendering;
using AYellowpaper.SerializedCollections;

[Flags] public enum ChoiceBehaviours 
{ 
    AddItem = 1 << 0, 
    RemoveItem = 1 << 1, 
    MustHaveItem = 1 << 2, 
    MustNotHaveItem = 1 << 3,
    ProbabilityEvent = 1 << 4,
    ReputationChange = 1 << 5,
    ReputationRequirement = 1 << 6,
    ChoiceInventoryChange = 1 << 7
}

[Serializable]
public class ChoiceData
{
    public string choice;
    public ThumbnailData linkedThumbnail;


    public bool triggerOnce;

    [Header("Behaviours")] [OnValueChanged(nameof(OnRequirementsChanged))] [AllowNesting]
    public ChoiceBehaviours behaviours;


    [ShowIf(nameof(behaviours), ChoiceBehaviours.AddItem)] [AllowNesting]
    public ItemsNames itemToAdd;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.RemoveItem)] [AllowNesting]
    public ItemsNames itemToRemove;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.MustHaveItem)] [AllowNesting]
    public ItemsNames itemsToHave;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.MustNotHaveItem)] [AllowNesting]
    public ItemsNames itemsToNotHave;

    [ShowIf(nameof(_showHideOnDisabled))] [AllowNesting]
    public bool hideOnDisabled;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.ProbabilityEvent)] [AllowNesting]
    public int winRate;
    [ShowIf(nameof(behaviours), ChoiceBehaviours.ProbabilityEvent)] [AllowNesting] [SerializeReference]
    public ChoiceData loseScene;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.ReputationChange)] [AllowNesting]
    public int reputationPoints;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.ReputationRequirement)] [AllowNesting]
    public int reputationThreshold;
    [ShowIf(nameof(behaviours), ChoiceBehaviours.ReputationRequirement)] [AllowNesting] [SerializeReference]
    public ChoiceData badReputationScene;

    [ShowIf(nameof(behaviours), ChoiceBehaviours.ChoiceInventoryChange)] [AllowNesting]
    [SerializedDictionary("Item to have", "Alternate text")]
    public SerializedDictionary<ItemsNames, ChoiceData> alternateChoices = new();

    [HideInInspector] public bool triggered = false;
    bool _showHideOnDisabled;

    public void OnRequirementsChanged()
    {
        if (behaviours.HasFlag(ChoiceBehaviours.ProbabilityEvent)) loseScene = new();
        if (behaviours.HasFlag(ChoiceBehaviours.ReputationRequirement)) badReputationScene = new();
        _showHideOnDisabled = behaviours.HasFlag(ChoiceBehaviours.MustHaveItem) || behaviours.HasFlag(ChoiceBehaviours.MustNotHaveItem);
    }
}
