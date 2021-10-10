using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("데이터 매니저 싱글톤 에러! - 중복존재");
            DestroyImmediate(this);
        }
        instance = this;

        folderPath = Application.dataPath + "/Save/";
        saveFileName = "SaveData.txt";
        keySettingSaveFileName = "keySettingSaveData.txt";
    }

    string folderPath;
    string saveFileName;
    string keySettingSaveFileName;

    private void Start()
    {
    }

    public void Save(SaveData data)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string jsonedData = JsonUtility.ToJson(data);

        File.WriteAllText(folderPath + saveFileName, jsonedData);
    }

    public SaveData Load()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        if(!File.Exists(folderPath + saveFileName))
        {
            Debug.LogWarning("세이브 파일이 없습니다! 기본 세이브파일을 생성합니다.");
            SaveData basicGameData = new SaveData("Player_Normal", "ShortSword", new Vector2(11.0f, 0.0f), 1.0f, 1.0f);
            Save(basicGameData);
            return basicGameData;
        }

        string jsonedData = File.ReadAllText(folderPath + saveFileName);
        SaveData data = JsonUtility.FromJson<SaveData>(jsonedData);

        return data;
    }

    public void SaveKeySetting(KeySettingData data)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        KeySettingDataForSave saveData = new KeySettingDataForSave();

        // dictionary는 직렬화가 안되므로, key와 value를 각각 list로 나눠서 저장.
        saveData.keyList = new List<Key>(data.keys.Keys);
        saveData.keyCodeList = new List<KeyCode>(data.keys.Values);

        string jsonedData = JsonUtility.ToJson(saveData);

        File.WriteAllText(folderPath + keySettingSaveFileName, jsonedData);
    }

    public KeySettingData LoadKeySetting()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // 키세팅 세이브 파일이 없으면, 기본값으로 저장.
        if (!File.Exists(folderPath + keySettingSaveFileName))
        {
            Debug.LogWarning("키세팅 세이브 파일이 없습니다! 기본 세이브파일을 생성합니다.");
            KeySettingData basicKeySettingData = new KeySettingData();
            basicKeySettingData.ResetKeySetting();
            SaveKeySetting(basicKeySettingData);
            return basicKeySettingData;
        }

        string jsonedData = File.ReadAllText(folderPath + keySettingSaveFileName);
        KeySettingDataForSave saveData = JsonUtility.FromJson<KeySettingDataForSave>(jsonedData);

        KeySettingData resultData = new KeySettingData();

        // 리스트 형태로 저장됐던 데이터를 dictionary로 변환
        for (int i = 0; i < saveData.keyList.Count; i++)
            resultData.keys.Add(saveData.keyList[i], saveData.keyCodeList[i]);

        return resultData;
    }
}
