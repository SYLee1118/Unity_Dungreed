using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Key
{
    NONE = -1,
    LEFT = 0,
    RIGHT,
    DOWN,
    JUMP,
    ATTACK,
}

public class KeyManager : MonoBehaviour
{
    private static KeyManager instance;
    public static KeyManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("키 매니저 중복 존재!");
            Destroy(this);
        }
        instance = this;
    }

    KeySettingData keyData;

    void Start()
    {
        keyData = DataManager.Instance.LoadKeySetting();
    }

    public void ChangeKey(Key key, KeyCode keyCode)
    {
        // 이미 할당되어있는 KeyCode일경우, 해당 KeyCode와 수정하려는 KeyCode를 맞바꾼다.
        if(keyData.keys.ContainsValue(keyCode))
        {
            Key targetKey = keyData.keys.FirstOrDefault(x => x.Value == keyCode).Key;
            keyData.keys[targetKey] = keyData.keys[key];
        }
        keyData.keys[key] = keyCode;
        DataManager.Instance.SaveKeySetting(keyData);
    }

    public bool GetKey(Key key)
    {
        if (Input.GetKey(keyData.keys[key]))
            return true;
        else
            return false;
    }

    public bool GetKeyDown(Key key)
    {
        if (Input.GetKeyDown(keyData.keys[key]))
            return true;
        else
            return false;
    }

    public bool GetKeyUp(Key key)
    {
        if (Input.GetKeyUp(keyData.keys[key]))
            return true;
        else
            return false;
    }

    public KeyCode currentKey(Key key)
    {
        return keyData.keys[key];
    }
}
