using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour
{
    public int lvlChoice = 1;

    public void CHoiceLvl() 
    {
        Debug.Log("Tu as choisi le lvl " + lvlChoice + ". Bravo !");
    }
}
