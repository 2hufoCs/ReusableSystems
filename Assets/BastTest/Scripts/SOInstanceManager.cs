using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Used to reset some values after runtime, since scriptable objects keep same values because they're assets
/// </summary>
public class SOInstanceManager : MonoBehaviour
{
    public List<ThumbnailData> _originalData;
    List<ChoiceData> _instantiatedData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSoData();
    }

    void OnDestroy()
    {
        ResetSoData();
    }

    void ResetSoData()
    {
        foreach (ThumbnailData data in _originalData)
        {
            foreach (ChoiceData choice in data.choicesData)
            {
                choice.triggered = false;
            }
        }  
    }
}
