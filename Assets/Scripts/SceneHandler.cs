using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance = null;
    public Object SceneToLoad;

    private void Start()
    {
        instance = this;
        LoadScene(SceneToLoad);
    }

    public void LoadScene(Object scene)
    {
        SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
    }

    public void UnloadScene(Object scene)
    {
        SceneManager.UnloadSceneAsync(scene.name);
    }
}
