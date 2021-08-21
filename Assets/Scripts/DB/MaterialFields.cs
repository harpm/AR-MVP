using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material Fields", menuName = "Database/New Material Fields", order = 2)]
public class MaterialFields : ScriptableObject
{
    [SerializeField]
    public List<Material> Cloth;

    [SerializeField]
    public List<Material> Cloth2;

    [SerializeField]
    public List<Material> Wood;

    [SerializeField]
    public List<Material> Wood2;

    [SerializeField]
    public List<Material> Metal;

    [SerializeField]
    public List<Material> Plastic;

    [SerializeField]
    public List<Material> Color;

    [SerializeField]
    public List<Material> Other;

    [SerializeField]
    public List<Material> Other2;

    [SerializeField]
    public List<Material> Other3;
}
