using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public interface ISaveable
{
    bool VerifyJson(string json) { return default; }
    bool HaveSaveFile() { return false; }

    void Save() { }
    void ClearSave() { }

    void Load() { }

    SaveData GetSaveData() { return default; }

    virtual void ChangeBGMVolume(float volume) { }
    virtual void ChangeSFXVolume(float volume) { }
}

public class NullSaveManager : ISaveable
{
}

public struct SaveData
{
    [JsonProperty] string _userId;
    [JsonProperty] string _userName;
    [JsonProperty] bool _muteBGM;
    [JsonProperty] bool _muteSFX;

    [JsonProperty] float _bgmVolume;
    [JsonProperty] float _sfxVolume;

    public SaveData(string id, string name)
    {
        _userId = id;
        _userName = name;

        _muteBGM = false;
        _muteSFX = false;

        _bgmVolume = 0.5f;
        _sfxVolume = 0.5f;
    }

    [JsonIgnore] public bool MuteBGM { get => _muteBGM; set => _muteBGM = value; }
    [JsonIgnore] public bool MuteSFX { get => _muteSFX; set => _muteSFX = value; }
    [JsonIgnore] public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; }
    [JsonIgnore] public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }
   
    [JsonIgnore] public string UserId { get => _userId; set => _userId = value; }
    [JsonIgnore] public string UserName { get => _userName; set => _userName = value; }
}

public class SaveManager : ISaveable
{
    JsonParser _parser;
    string _filePath;

    SaveData _defaultSaveData;
    SaveData _saveData;

    public SaveManager(SaveData defaultSaveData)
    {
        _parser = new JsonParser();
        _defaultSaveData = defaultSaveData;
        _filePath = Application.persistentDataPath + "/SaveData.txt";
        Debug.Log(_filePath);
        Load();
    }

    public void ChangeBGMVolume(float volume)
    {
        _saveData.BgmVolume = volume;
        Save();
    }

    public void ChangeSFXVolume(float volume)
    {
        _saveData.SfxVolume = volume;
        Save();
    }

    public SaveData GetSaveData()
    {
        return _saveData;
    }

    public void ClearSave() 
    {
        if (HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save();
            return;
        }
    }

    public string GetSaveJsonData()
    {
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save(); // ���̺� ������ ������ְ� �����Ѵ�.
        }

        // ����� ������ �ҷ��� �����Ѵ�.
        string json = File.ReadAllText(_filePath);
        return json;
    }


    /// <summary>
    /// GPGS�� ������ ����
    /// ���� �������� ���� �����Ͱ� ���峭 ��� ���� �����͸� �������� �ʰ�
    /// �״�� ���
    /// </summary>

    public bool VerifyJson(string json)
    {
        // �ҷ����� �� ������ �ִٸ� �⺻ �����͸� �Ѱ��ش�.
        try
        {
            _saveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData;
            return false; // ��ȿ���� ����
        }

        return true; // ��ȿ��
    }

    public bool HaveSaveFile()
    {
        return File.Exists(_filePath);
    }

    public void Load()
    {
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData; // �⺻ ���̺�� ��ü���ش�.
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);
        bool nowValidate = VerifyJson(json);
        Save();
    }

    void Save()
    {
        string json = _parser.ObjectToJson(_saveData);
        File.WriteAllText(_filePath, json);
    }
}
