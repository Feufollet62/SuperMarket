using UnityEngine;

[CreateAssetMenu(fileName = "ClientData", menuName = "Data/Client Data")]
public class ClientData : ScriptableObject
{
    public ClientInfo[] clientInfos;
}

[System.Serializable]
public class ClientInfo
{
    public ClientType type = ClientType.Normal;

    public Mesh model;
    public Material material;

    public Sprite sprite;

    public string name;
    public string description;
    public float time;
}