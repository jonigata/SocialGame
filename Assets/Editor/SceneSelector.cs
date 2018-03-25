using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class SceneSelector : EditorWindow {
    [MenuItem("Window/Scene Selector", false, 1000)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SceneSelector>("Scene Selector");
    }

    void OnGUI()
    {
        m_scroll_pos = EditorGUILayout.BeginScrollView(m_scroll_pos);

        foreach (var scene in EditorBuildSettings.scenes) {
            if (GUILayout.Button(scene.path)) {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(scene.path);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    Vector2 m_scroll_pos = Vector2.zero;
}
