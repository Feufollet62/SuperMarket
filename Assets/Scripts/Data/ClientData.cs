using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientData", menuName = "Data/Client Data")]
public class ClientData : ScriptableObject
{
    public GameObject modelClient;
    public string Name;
    public string Description;
    public float Time;
    public int IdObject;
    public Sprite imageObjet;

    public void Awake()
    {
        Time = Random.Range(0, 10);
    }
}
