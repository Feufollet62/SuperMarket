using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUI : MonoBehaviour
{
    [SerializeField] ClientData clientData;
    [SerializeField] Text textNameClient;
    [SerializeField] Text timeClient;

    private void Start()
    {
        textNameClient.text = clientData.Name;
        timeClient.text = clientData.Time.ToString();
    }
}
