using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public Object[] ScenesToLoad;
    public Object[] ScenesToUnload;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerOne")
        {
            for(int i = 0; i < ScenesToLoad.Length; i++)
            {
                SceneHandler.instance.LoadScene(ScenesToLoad[i]);

                for (int t = 0; i < ScenesToUnload.Length; i++)
                {
                    SceneHandler.instance.UnloadScene(ScenesToUnload[t]);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
