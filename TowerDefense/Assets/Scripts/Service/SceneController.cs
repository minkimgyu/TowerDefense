using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : ISceneControllable
{
    public void ChangeScene(ISceneControllable.SceneName sceneName)
    {
        ServiceLocater.ReturnSoundPlayer().StopBGM(); // ∫Í±› ∏ÿ√Á¡ÿ¥Ÿ.
        SceneManager.LoadScene(sceneName.ToString());
    }

    public ISceneControllable.SceneName GetCurrentSceneName()
    {
        return (ISceneControllable.SceneName)Enum.Parse(typeof(ISceneControllable.SceneName), SceneManager.GetActiveScene().name);
    }
}
