#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// 에디터 전용
[InitializeOnLoad]
public static partial class EditorPlayManager
{
    static EditorPlayManager()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                var allScenes = "";
                for (int i = 0; i < EditorSceneManager.sceneCount; i++)
                {
                    var scene = EditorSceneManager.GetSceneAt(i);
                    if (scene.isLoaded)
                        allScenes += scene.path + ";";
                }
                EditorPrefs.SetString("EditorPlayManager.OpenScenes", allScenes);
                break;

            case PlayModeStateChange.EnteredPlayMode:
                UnityEngine.SceneManagement.SceneManager.LoadScene("AddressableScene");
                break;
        }
    }
}
#endif