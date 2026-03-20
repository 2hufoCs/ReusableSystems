using System;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public struct ChoiceData
{
    public string choice;
    public ThumbnailData linkedThumbnail;
    public Items itemToAdd;
    public Items itemToRemove;
    public Items itemToHave;
    public Items itemToNotHave;
}
