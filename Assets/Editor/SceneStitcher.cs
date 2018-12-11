using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneTrigger))]
public class SceneStitcher : Editor
{
    SceneTrigger trigger;

    private void OnEnable()
    {
        trigger = (SceneTrigger)target;

        SceneView.onSceneGUIDelegate += (SceneView.OnSceneFunc)System.Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(CustomOnSceneGUI));
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10f);

        GUIStyle style = new GUIStyle();

        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        GUILayout.Label("Dynamic Scene Stitcher" , style);


        //Scenes to be loaded part

        GUILayout.Space(15f);

        style.fontSize = 18;
        GUILayout.Label("SCENES TO LOAD" , style);
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("Amount scenes to load: " + trigger.ScenesToLoad.Count, style, GUILayout.Width(200), GUILayout.Height(30));
        GUILayout.Space(10f);
        if(GUILayout.Button("ADD" , GUILayout.Height(30) , GUILayout.Width(50)))
        {
            Undo.RecordObject(trigger, "Add new scene to load");
            
            trigger.ScenesToLoad.Add(null);

            if(trigger.ScenesToLoad.Count > 3)
            {
                trigger.ScenesToLoad.RemoveAt(trigger.ScenesToLoad.Count - 1);
                EditorApplication.Beep();
            }

            EditorUtility.SetDirty(trigger);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);
        for (int i = 0; i < trigger.ScenesToLoad.Count; i++)
        {
            DrawScenesToLoadList(i);
        }


        //Scenes to be unloaded part

        GUILayout.Space(15f);

        style.fontSize = 18;
        style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("SCENES TO UNLOAD", style);
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("Amount scenes to unload: " + trigger.ScenesToUnload.Count, style, GUILayout.Width(220), GUILayout.Height(30));
        GUILayout.Space(10f);
        if (GUILayout.Button("ADD", GUILayout.Height(30), GUILayout.Width(50)))
        {
            Undo.RecordObject(trigger, "Add new scene to unload");

            trigger.ScenesToUnload.Add(null);
            if (trigger.ScenesToUnload.Count > 3)
            {
                trigger.ScenesToUnload.RemoveAt(trigger.ScenesToUnload.Count - 1);
                EditorApplication.Beep();
            }

            EditorUtility.SetDirty(trigger);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);
        for (int i = 0; i < trigger.ScenesToUnload.Count; i++)
        {
            DrawScenesToUnloadList(i);
        }


        //debug purposes
        DrawDefaultInspector();
    }

    void DrawScenesToLoadList(int index)
    {
        if (index < 0 || index >= trigger.ScenesToLoad.Count)
            return;

        GUILayout.BeginHorizontal();
        GUIStyle style = new GUIStyle();

        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        GUILayout.Label("Scene: ", style, GUILayout.Width(50));

        //make a box where i can drop in scene files that have to be loaded or unloaded
        trigger.ScenesToLoad[index] = EditorGUILayout.ObjectField(trigger.ScenesToLoad[index], typeof(Object), true);

        if (GUILayout.Button("X", GUILayout.Height(20), GUILayout.Width(25)))
        {
            Undo.RecordObject(trigger , "Undo delete load list");
            trigger.ScenesToLoad.RemoveAt(index);

            EditorUtility.SetDirty(trigger);
        }
        GUILayout.EndHorizontal();
    }

    void DrawScenesToUnloadList(int index)
    {
        if (index < 0 || index >= trigger.ScenesToUnload.Count)
            return;

        GUILayout.BeginHorizontal();
        GUIStyle style = new GUIStyle();

        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        GUILayout.Label("Scene: ", style , GUILayout.Width(50));
        
        //make a box where i can drop in scene files that have to be loaded or unloaded
        trigger.ScenesToUnload[index] = EditorGUILayout.ObjectField(trigger.ScenesToUnload[index], typeof(Object), true);

        if (GUILayout.Button("X", GUILayout.Height(20), GUILayout.Width(25)))
        {
            Undo.RecordObject(trigger, "Undo delete unload list");
            trigger.ScenesToUnload.RemoveAt(index);
            EditorUtility.SetDirty(trigger);
        }
        GUILayout.EndHorizontal();
    }

    private void CustomOnSceneGUI(SceneView sceneView)
    {
        if (trigger.ScenesToLoad.Count == 0)
            Handles.color = new Color(1, 0, 0, .2f);
        else if(trigger.ScenesToLoad.Count != 0)
            Handles.color = new Color(0, 1, 0, .2f);

        Handles.CubeHandleCap(0, trigger.transform.position + new Vector3(0f, trigger.GetComponent<BoxCollider2D>().size.y + 5, 0f), Quaternion.identity, 10, EventType.Repaint);

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        Handles.Label(trigger.transform.position + new Vector3(-5, trigger.GetComponent<BoxCollider2D>().size.y + 6, 0f), trigger.gameObject.scene.name , style);
        Handles.color = Color.yellow;
        Handles.RectangleHandleCap(0, trigger.transform.position + new Vector3(0f, trigger.GetComponent<BoxCollider2D>().size.y + 5, 0f), Quaternion.identity, 5, EventType.Repaint);
    }
}
