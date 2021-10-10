using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // 캐릭터 종류
    public string playerName;
    // 무기 종류
    public string weaponName;
    // 현재 위치
    public Vector2 playerPosition;
    // BGM 볼륨
    public float bgmVolume;
    // sfx 볼륨
    public float sfxVolume;

    public SaveData(string playerName, string weaponName, Vector2 playerPosition, float bgmVolume, float sfxVolume)
    {
        this.playerName = playerName;
        this.weaponName = weaponName;
        this.playerPosition = playerPosition;
        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
    }
}

// 실제 활용용 키셋팅 데이터
public class KeySettingData
{
    public Dictionary<Key, KeyCode> keys;

    public KeySettingData()
    {
        keys = new Dictionary<Key, KeyCode>();
    }

    // 기본 키셋팅
    public void ResetKeySetting()
    {
        keys = new Dictionary<Key, KeyCode>();
        keys.Add(Key.ATTACK, KeyCode.Mouse0);
        keys.Add(Key.DOWN, KeyCode.S);
        keys.Add(Key.LEFT, KeyCode.A);
        keys.Add(Key.RIGHT, KeyCode.D);
        keys.Add(Key.JUMP, KeyCode.Space);
    }
}

// dictionary는 직렬화가 안돼서 json저장이 안되므로, 저장용 데이터
[System.Serializable]
public class KeySettingDataForSave
{
    public List<Key> keyList;
    public List<KeyCode> keyCodeList;

    public KeySettingDataForSave()
    {
        keyList = new List<Key>();
        keyCodeList = new List<KeyCode>();
    }
}
