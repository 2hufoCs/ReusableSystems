using System;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using NaughtyAttributes;

[Flags] public enum ChoiceBehaviours { AddItem = 1 << 0, RemoveItem = 1 << 1, MustHaveItem = 1 << 2, MustNotHaveItem = 1 << 3}

[Serializable]
public struct ChoiceData
{
    public string choice;
    public ThumbnailData linkedThumbnail;
    public ChoiceBehaviours behaviours;

    [ShowIf("behaviours", ChoiceBehaviours.AddItem)] [AllowNesting]
    public Items itemToAdd;
    [ShowIf("behaviours", ChoiceBehaviours.RemoveItem)] [AllowNesting]
    public Items itemToRemove;
    [ShowIf("behaviours", ChoiceBehaviours.MustHaveItem)] [AllowNesting]
    public Items itemToHave;
    [ShowIf("behaviours", ChoiceBehaviours.MustNotHaveItem)] [AllowNesting]
    public Items itemToNotHave;
}
