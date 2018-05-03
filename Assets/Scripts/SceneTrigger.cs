using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public Object[] ScenesToLoad;
    public Object[] ScenesToUnload;

    Scene[] activeScenes = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerOne")
        {
            StartCoroutine(DoLoadingStuff());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "playerOne")
        {
            activeScenes = new Scene[SceneManager.sceneCount];

            for (int t = 0; t < SceneManager.sceneCount; t++)
            {
                activeScenes[t] = SceneManager.GetSceneAt(t);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "PlayerOne")
        {
            StartCoroutine(DoUnloadingStuff());
            Destroy(gameObject);
        }
    }
    

    IEnumerator DoLoadingStuff()
    {
        for (int i = 0; i < ScenesToLoad.Length; i++)
        {
            SceneHandler.instance.LoadScene(ScenesToLoad[i]);
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator DoUnloadingStuff()
    {
        for (int i = 0; i < ScenesToUnload.Length; i++)
        {
                if (SceneManager.Equals(ScenesToUnload[i], activeScenes[i]))
                    SceneHandler.instance.UnloadScene(ScenesToUnload[i]);
                else
                    break;
            
        }

        yield return new WaitForEndOfFrame();
    }
}
