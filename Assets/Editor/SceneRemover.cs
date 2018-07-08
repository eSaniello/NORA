using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneRemover : Editor
{
    //Create a menu item to remove all scenes in the hierarchy except the player.
    //This is when I want to start the game.

	[MenuItem("Scene/Remove All From Hierarchy except Player")]
 	static void remove()
	{
        List<Scene> loadedScenes = new List<Scene>();
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            loadedScenes.Add(EditorSceneManager.GetSceneAt(i));
        }

        foreach (Scene s in loadedScenes)
        {
            if (s.name == "Player")
                continue;
            else
                EditorSceneManager.CloseScene(s, true);
        }
    }
}
