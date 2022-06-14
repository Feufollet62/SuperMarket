using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDefinition : MonoBehaviour
{
    public ObjectData dataObject;
    public float iD;

    private void Start()
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();
        MeshRenderer mRender = GetComponent<MeshRenderer>();

        mFilter.sharedMesh = dataObject.model;
        mRender.sharedMaterial = dataObject.material;

        gameObject.name = dataObject.name;

        iD = dataObject.iD;
    }
    

}


