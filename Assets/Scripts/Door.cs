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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LoadNextLevel(destination);
    }

    private void LoadNextLevel(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
}
