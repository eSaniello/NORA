using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public Object[] ScenesToLoad;
    public Object[] ScenesToUnload;
    
    List<string> activeScenes = null;


    private void Start()
    {
        activeScenes = new List<string>(SceneManager.sceneCount);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DoLoadingStuff();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetActiveScenesInHierarchy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DoUnloadingStuff();
        }
    }


    void DoLoadingStuff()
    {
        foreach(Object scene in ScenesToLoad)
        {
            if (!SceneManager.GetSceneByName(scene.name).isLoaded)
                SceneHandler.instance.LoadScene(scene.name);
        }
    }

    void DoUnloadingStuff()
    {
        foreach(Object scene in ScenesToUnload)
        {
            if (activeScenes.Contains(scene.name) && SceneManager.GetSceneByName(scene.name).isLoaded)
                SceneHandler.instance.UnloadScene(scene.name);
        }
    }

    void GetActiveScenesInHierarchy()
    {
        for (int t = 0; t < SceneManager.sceneCount; t++)
        {
            activeScenes.Add(SceneManager.GetSceneAt(t).name);
        }
    }
}
