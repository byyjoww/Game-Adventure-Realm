using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SceneReference scene;

    public void LoadSceneByReference()
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
