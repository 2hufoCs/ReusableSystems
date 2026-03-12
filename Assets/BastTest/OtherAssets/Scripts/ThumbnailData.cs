using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newThumbnail", menuName = "Thumbnail")]
public class ThumbnailData : ScriptableObject
{
    public Sprite thumbnailImage;
    public string description;
    public List<ChoiceData> choicesData;
}
