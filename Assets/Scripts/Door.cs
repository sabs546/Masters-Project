using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string destination;

    private void Start()
    {
        
    }

    private void Update()
    {
        LoadNextLevel(destination);
    }

    private void LoadNextLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
