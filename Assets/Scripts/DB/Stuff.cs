using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stuff", menuName = "Database/New Stuff", order = 1)]
public class Stuff : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public ArStuff Model;
    [SerializeField] [TextArea(3, 6)] public string Desc;
}
