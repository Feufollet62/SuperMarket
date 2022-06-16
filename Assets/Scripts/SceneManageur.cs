using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageur : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void SceneRunLevel1()
    {
        SceneManager.LoadScene("Niveau_1");
    }
    public void SceneRunLevel2()
    {
        SceneManager.LoadScene("Niveau_2");
    }
}
