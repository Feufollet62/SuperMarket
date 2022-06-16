using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDefinition : MonoBehaviour
{
    public ObjectData dataObject;
    public float iD;
    public Sprite imageObjet;

    private void Start()
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();
        MeshRenderer mRender = GetComponent<MeshRenderer>();

        mFilter.sharedMesh = dataObject.model;
        mRender.sharedMaterial = dataObject.material;

        gameObject.name = dataObject.name;
        imageObjet = dataObject.image;

        iD = dataObject.iD;
    }
    

}


