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
        GUILayout.Label("This is a label!");

        DrawDefaultInspector();
    }

    private void CustomOnSceneGUI(SceneView sceneView)
    {
        if (trigger.ScenesToLoad.Length == 0)
            Handles.color = new Color(1, 0, 0, .2f);
        else if(trigger.ScenesToLoad.Length != 0)
            Handles.color = new Color(0, 1, 0, .2f);

        Handles.CubeHandleCap(0, trigger.transform.position + new Vector3(0f, trigger.GetComponent<BoxCollider2D>().size.y + 5, 0f), Quaternion.identity, 10, EventType.Repaint);

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        Handles.Label(trigger.transform.position + new Vector3(-5, trigger.GetComponent<BoxCollider2D>().size.y + 6, 0f), trigger.gameObject.scene.name , style);
        Handles.color = Color.yellow;
        Handles.RectangleHandleCap(0, trigger.transform.position + new Vector3(0f, trigger.GetComponent<BoxCollider2D>().size.y + 5, 0f), Quaternion.identity, 5, EventType.Repaint);
    }
}
