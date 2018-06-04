using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [Header("Scenes to be loaded")]
    public Object[] ScenesToLoad;

    [Space(10)]
    [Header("Scenes to be unloaded")]
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


    //editor scripting
    private void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + new Vector3(col.offset.x , col.offset.y , 0f) , col.size);

        Gizmos.color = new Color(0f, 1f, 0f, .3f);
        Gizmos.DrawCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        Gizmos.color = new Color(1f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);

        Gizmos.color = new Color(1f, 1f, 0f, .3f);
        Gizmos.DrawCube(transform.position + new Vector3(col.offset.x, col.offset.y, 0f), col.size);
    }
}
