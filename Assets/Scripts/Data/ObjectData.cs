using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Data/Object Data")]
public class ObjectData : ScriptableObject
{
    public Mesh model; //<-- penser a faire des models en un seul objet
    public Material material;
    public GameObject prefabModel;
    public int iD = 0;

    public Sprite image;
    public string name;
    public bool craftable;
    public bool weapon;
}
