using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionData", menuName = "Data/Fusion Data")]
public class FusionData : ScriptableObject
{
    [SerializeField] public ObjectData objetFusion1;
    [SerializeField] public ObjectData objetFusion2;
    [SerializeField] public GameObject objetResult;
}
