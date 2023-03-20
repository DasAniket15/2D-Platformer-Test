using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]

public class TransformEditor : Editor
{
    static TransformEditor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e != null && e.keyCode != KeyCode.None)
        {
            Debug.Log("Key pressed in editor: " + e.keyCode);
        }

        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.D)
            {
                if (Selection.activeTransform != null)
                {
                    Selection.activeTransform.position += new Vector3(1, 0, 0); // Move 1 unit to the right
                    e.Use();
                }
            }

            if (e.keyCode == KeyCode.A)
            {
                if (Selection.activeTransform != null)
                {
                    Selection.activeTransform.position -= new Vector3(1, 0, 0); // Move 1 unit to the left
                    e.Use();
                }
            }

            if (e.keyCode == KeyCode.W)
            {
                if (Selection.activeTransform != null)
                {
                    Selection.activeTransform.position += new Vector3(0, 1, 0); // Move 1 unit up
                    e.Use();
                }
            }

            if (e.keyCode == KeyCode.S)
            {
                if (Selection.activeTransform != null)
                {
                    Selection.activeTransform.position -= new Vector3(0, 1, 0); // Move 1 unit down
                    e.Use();
                }
            }
        }
    }
}
