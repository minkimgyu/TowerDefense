using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadingSceneController : MonoBehaviour
{
    [Header("Loading")]
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    //void LoginToGoogle(Action<string, string> OnCompleted)
    //{
    //    GPGSManager gPGSManager = new GPGSManager();
    //    ServiceLocater.Provide(gPGSManager);

    //    gPGSManager.Login((isLogin, id, name) =>
    //    {
    //        if (isLogin == false)
    //        {
    //            // 만약 gpgs 로그인 안 될 경우 리턴
    //            Debug.Log("GPGS 로그인 실패");
    //            return;
    //        }

    //        Debug.Log("GPGS 로그인 성공");
    //        OnCompleted?.Invoke(id, name);
    //    });
    //}

    //void UpdateAppVersion(Action OnComplete)
    //{
    //    GameObject inAppUpdateManagerObject = new GameObject("InAppUpdateManager");
    //    InAppUpdateManager inAppUpdateManager = inAppUpdateManagerObject.AddComponent<InAppUpdateManager>();

    //    // 초기화 완료 시 실행
    //    inAppUpdateManager.Initialize((value) =>
    //    {
    //        Debug.Log(value);
    //        OnComplete?.Invoke();
    //    });
    //}

    void ChangeLoadingProgress(float value)
    {
        _loadingPregressBar.fillAmount = value;
        _loadingPregressTxt.text = $"{(value * 100f).ToString("F2")} %";
    }

    private void Start()
    {
        AddressableLoader addressableLoader = CreateAddressableLoader();
        addressableLoader.Initialize();
        addressableLoader.InjectProgressEvent((value) =>
        {
            ChangeLoadingProgress(Mathf.RoundToInt(value));
        });

        addressableLoader.Load(() =>
        {
            Initialize(addressableLoader);

#if UNITY_EDITOR
            EditorPlayManager.RestoreScenes();
#endif
        });
    }

    void Initialize(AddressableLoader addressableLoader)
    {
        TimeController timeController = new TimeController();
        ServiceLocater.Provide(timeController);

        SceneController sceneController = new SceneController();
        ServiceLocater.Provide(sceneController);

        SaveManager saveManager = new SaveManager(new SaveData("host", "meal"));
        ServiceLocater.Provide(saveManager);

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        SoundPlayer soundPlayer = CreateSoundPlayer(addressableLoader.SoundAssets, saveData);
        ServiceLocater.Provide(soundPlayer);
    }

    SoundPlayer CreateSoundPlayer(Dictionary<ISoundPlayable.SoundName, AudioClip> soundAssets, SaveData saveData)
    {
        GameObject soundPlayerObject = new GameObject("SoundPlayer");
        SoundPlayer soundPlayer = soundPlayerObject.AddComponent<SoundPlayer>();
        soundPlayer.Initialize(soundAssets, saveData.BgmVolume, saveData.SfxVolume);

        return soundPlayer;
    }

    AddressableLoader CreateAddressableLoader()
    {
        GameObject addressableObject = new GameObject("AddressableLoader");
        AddressableLoader addressableHandler = addressableObject.AddComponent<AddressableLoader>();
        addressableHandler.Initialize();

        return addressableHandler;
    }
}
