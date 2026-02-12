using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [SerializeField] List<Stat> stats;
    [SerializeField] List<TextMeshProUGUI> statTexts;

    void Start()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].statText = statTexts[i];
            stats[i].statText.text = $"{stats[i].name}: {stats[i].value}";
        }
    }
}
