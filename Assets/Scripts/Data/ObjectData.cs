using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Data/Object Data")]
public class ObjectData : ScriptableObject
{
    public Mesh model;
    public Material material;
    public int iD = 0;

    public Sprite image;
    public string name;
}
