using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SceneTrigger))]
public class SceneTransition : Editor
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
            Undo.RecordObject(trigger, "Add new scene");

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

        DrawDefaultInspector();
    }

    void DrawScenesToLoadList(int index)
    {
        if (index < 0 || index >= trigger.ScenesToLoad.Count)
            return;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scene: ", GUILayout.Width(50));
        //EditorGUI.ObjectField(trigger.ScenesToLoad[index]);

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
        style.normal.textColor = Color.red;
        Handles.Label(trigger.transform.position + new Vector3(-5, trigger.GetComponent<BoxCollider2D>().size.y + 6, 0f), trigger.gameObject.scene.name , style);
        Handles.color = Color.yellow;
        Handles.RectangleHandleCap(0, trigger.transform.position + new Vector3(0f, trigger.GetComponent<BoxCollider2D>().size.y + 5, 0f), Quaternion.identity, 5, EventType.Repaint);
    }
}
