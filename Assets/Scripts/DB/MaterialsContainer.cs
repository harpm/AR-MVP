using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialsContainer : Object
{
    [SerializeReference] public List<Material> Cloth;
    [SerializeField] public List<Material> Cloth2;
    [SerializeField] public List<Material> Wood;
    [SerializeField] public List<Material> Wood2;
    [SerializeField] public List<Material> Metal;
    [SerializeField] public List<Material> Plastic;
    [SerializeField] public List<Material> Color;
    [SerializeField] public List<Material> Other;
    [SerializeField] public List<Material> Other2;
    [SerializeField] public List<Material> Other3;
}
