using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

public class AddressableLoader : MonoBehaviour
{
    public enum Label
    {
        Dot,
        Effect,
        ModeTitle,

        ArtData,
        ArtSprite,

        ArtworkFrame,

        RectProfileIcon,
        CircleProfileIcon,

        RankBadgeIcon,
        RankDecorationIcon,
        StageRankIcon,

        SpawnableUI,

        ArtworkData,
        LocalizationData,
        ChallengeModeStageData,
        ColorPaletteData,

        Sound,
    }

    HashSet<BaseLoader> _assetLoaders;

    int _successCount;
    int _totalCount;
    Action OnCompleted;
    Action<float> OnProgress;

    public void Initialize(bool dontDestroyOnLoad = true)
    {
        if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        _successCount = 0;
        _totalCount = 0;
        _assetLoaders = new HashSet<BaseLoader>();
    }

    public void AddProgressEvent(Action<float> OnProgress)
    {
        this.OnProgress = OnProgress;
    }

    //public Dictionary<Effect.Name, Effect> EffectAssets { get; private set; }
    //public Dictionary<SpawnableUI.Name, SpawnableUI> SpawnableUIAssets { get; private set; }
    //public Dictionary<ISoundPlayable.SoundName, AudioClip> SoundAssets { get; private set; }


    public void Load(Action OnCompleted)
    {
        //_assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectAssets = value; OnSuccess(label); }));

        //_assetLoaders.Add(new SoundAssetLoader(Label.Sound, (value, label) => { SoundAssets = value; OnSuccess(label); }));

        //_assetLoaders.Add(new SpawnableUIAssetLoader(Label.SpawnableUI, (value, label) => { SpawnableUIAssets = value; OnSuccess(label); }));

        this.OnCompleted = OnCompleted;
        _totalCount = _assetLoaders.Count;
        foreach (var loader in _assetLoaders)
        {
            loader.Load();
        }
    }

    void OnSuccess(Label label)
    {
        _successCount++;
        Debug.Log(_successCount);
        Debug.Log(label.ToString() + "Success");

        OnProgress?.Invoke((float)_successCount / _totalCount);
        if (_successCount == _totalCount)
        {
            Debug.Log("Complete!");
            OnCompleted?.Invoke();
        }
    }

    public void Release()
    {
        foreach (var loader in _assetLoaders)
        {
            loader.Release();
        }
    }
}
