using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageur : MonoBehaviour
{
    public void SceneRunLevel1()
    {
        SceneManager.LoadScene("Niveau_1");
    }
}
