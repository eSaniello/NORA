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
            StartCoroutine(DoLoadingStuff());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int t = 0; t < SceneManager.sceneCount; t++)
            {
                activeScenes.Add(SceneManager.GetSceneAt(t).name);
                Debug.Log(activeScenes[t]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(DoUnloadingStuff());
            Destroy(gameObject);
        }
    }


    IEnumerator DoLoadingStuff()
    {
        for (int i = 0; i < ScenesToLoad.Length; i++)
        {
            Debug.Log(ScenesToLoad[i]);
            if (SceneManager.GetSceneByName(ScenesToLoad[i].name).isLoaded)
                break;
            else
                SceneHandler.instance.LoadScene(ScenesToLoad[i].name);
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator DoUnloadingStuff()
    {
        for (int i = 0; i < ScenesToUnload.Length; i++)
        {
            if (activeScenes.Contains(ScenesToUnload[i].name) && SceneManager.GetSceneByName(ScenesToUnload[i].name).isLoaded)
            {
                SceneHandler.instance.UnloadScene(ScenesToUnload[i].name);
            }
            else
                break;
        }

        yield return new WaitForEndOfFrame();
    }
}
