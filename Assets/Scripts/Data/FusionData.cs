using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionData", menuName = "Data/Fusion Data")]
public class FusionData : ScriptableObject
{
    [SerializeField] public ObjectData objetFusion1;
    [SerializeField] public ObjectData objetFusion2;
    [SerializeField] public ObjectData objetResult;

    public bool IsCraftable(ObjectData obj)
    {
        if (obj == objetFusion1) return true;
        else if (obj == objetFusion2) return true;
        else return false;
    }

    public ObjectData Craft(ObjectData obj1, ObjectData obj2)
    {
        if (obj1 == objetFusion1 && obj2 == objetFusion2) return objetResult;
        return null;
    }
}
