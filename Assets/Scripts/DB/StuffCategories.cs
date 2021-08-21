using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stuff Category", menuName = "Database/New Stuff Category", order = 0)]
public class StuffCategories : ScriptableObject
{
    [SerializeField]
    public string Name;

    [SerializeField]
    [TextArea(6, 12)]
    public string Description;

    [SerializeField]
    public List<Stuff> stuffs;
}
