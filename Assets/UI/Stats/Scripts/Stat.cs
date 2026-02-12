using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStat", menuName = "Stat")]
public class Stat : ScriptableObject
{
    public new string name;
    public int value;
    [HideInInspector] public TextMeshProUGUI statText; // assigned with StatsManager script
}
