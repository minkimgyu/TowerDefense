using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 런타임 전용
public static partial class EditorPlayManager
{
#if UNITY_EDITOR
    public static void RestoreScenes()
    {
        string scenes = UnityEditor.EditorPrefs.GetString("EditorPlayManager.OpenScenes", "");

        if (string.IsNullOrEmpty(scenes)) return;

        var sceneList = scenes.Split(';');
        if (sceneList.Length > 0 && !string.IsNullOrEmpty(sceneList[0]))
            SceneManager.LoadScene(sceneList[0], LoadSceneMode.Single);

        for (int i = 1; i < sceneList.Length; i++)
        {
            if (!string.IsNullOrEmpty(sceneList[i]))
                SceneManager.LoadScene(sceneList[i], LoadSceneMode.Additive);
        }
    }
#endif
}