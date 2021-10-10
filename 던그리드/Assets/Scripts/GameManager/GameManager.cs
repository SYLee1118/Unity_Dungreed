using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            Debug.LogWarning("게임 매니저 중복존재!");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        LoadGameData();
    }

    #region UI_Region
    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private GameObject soundOptionPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private GameObject keySettingOptionPanel;
    [SerializeField] private KeySettingUI keySettingUI;
    [SerializeField] private GameObject GameOverFailPanel;
    [SerializeField] private GameObject GameOverClearPanel;
    [SerializeField] private SFXPool sfxPool;

    #region UIMethod
    public void OpenSoundOption()
    {
        optionCanvas.SetActive(true);
        keySettingOptionPanel.SetActive(false);

        bgmSlider.value = gameData.bgmVolume;
        effectSlider.value = gameData.sfxVolume;
        soundOptionPanel.SetActive(true);
    }
    public void CloseOption()
    {
        gameData.bgmVolume = bgmSlider.value;
        gameData.sfxVolume = effectSlider.value;
        DataManager.Instance.Save(gameData);
        soundOptionPanel.SetActive(false);
        keySettingOptionPanel.SetActive(false);
        optionCanvas.SetActive(false);
    }
    public void OpenKeySettingOption()
    {
        optionCanvas.SetActive(true);
        soundOptionPanel.SetActive(false);

        keySettingOptionPanel.SetActive(true);
        keySettingUI.ShowCurrentKeySetting();
    }
    public void SetBGMVolume()
    {
        gameData.bgmVolume = bgmSlider.value;
        // 각 씬마다 서로 다른 SceneMain이 BGM을 갖고 있으므로, 이벤트로 처리.
        volumeChangeEvent(gameData.bgmVolume);
    }
    public void SetSFXVolume()
    {
        gameData.sfxVolume = effectSlider.value;
    }

    public delegate void delegateEvent(float bgmVolume);
    public event delegateEvent volumeChangeEvent;

    #endregion
    #endregion

    // 현재 무기
    public Weapon_Base weapon { get; private set; }
    // 현재 플레이어 캐릭터
    public Player_Base player { get; private set; }
    // 로딩한 게임 데이터
    public SaveData gameData { get; private set; }

    public void LoadGameData()
    {
        gameData = DataManager.Instance.Load();
    }

    public void SaveGameData()
    {
        DataManager.Instance.Save(gameData);
    }

    public void GameOver_Fail()
    {
        SceneController.Instance.currentSceneMain.StopBGM();
        sfxPool.Play("Fail");
        GameOverFailPanel.SetActive(true);
        Invoke("ReturnToVillage", 5.0f);
    }
    public void GameOver_Clear()
    {
        SceneController.Instance.currentSceneMain.StopBGM();
        player.Pause();
        sfxPool.Play("Clear");
        GameOverClearPanel.SetActive(true);
        Invoke("ReturnToVillage", 5.0f);
    }

    // 클리어 혹은 실패했을 때, 마을로 다시 돌아가기 위함.

    void ReturnToVillage()
    {
        GameOverFailPanel.SetActive(false);
        GameOverClearPanel.SetActive(false);
        SceneController.Instance.LoadScene("Village");
    }

    // 플레이어, 무기 prefab을 로드.
    public void LoadPlayer()
    {
        if (player != null)
            Destroy(player.gameObject);

        if (weapon != null)
            Destroy(weapon.gameObject);

        GameObject _player;
        try
        {
            _player = Instantiate(Resources.Load("Prefabs/Player/" + gameData.playerName)) as GameObject;
        }
        catch
        {
            _player = Instantiate(Resources.Load("Prefabs/Player/Player_Normal")) as GameObject;
            Debug.LogWarning("잘못된 플레이어 이름 : " + gameData.playerName);
            gameData.playerName = "Player_Normal";
        }
        player = _player.GetComponent<Player_Base>();

        GameObject _weapon;
        try
        {
            _weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + gameData.weaponName)) as GameObject;
        }
        catch
        {
            _weapon = Instantiate(Resources.Load("Prefabs/Weapons/ShortSword")) as GameObject;
            Debug.LogWarning("세이브 데이터에 잘못된 무기 이름. 기본으로 초기화.");
            gameData.weaponName = "ShortSword";
        }
        weapon = _weapon.GetComponent<Weapon_Base>();

        player.transform.SetParent(this.transform);
    }

    // 게임도중 플레이어 캐릭터가 바뀌지는 않지만, 무기는 바뀔수 있다. 캐릭터 정보는 유지하고 무기 정보만 변경.
    public void ChangeWeapon(string newWeaponName)
    {
        if (weapon != null)
            Destroy(weapon.gameObject);

        GameObject _weapon;
        try
        {
            _weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + newWeaponName)) as GameObject;
            gameData.weaponName = newWeaponName;
        }
        catch
        {
            _weapon = Instantiate(Resources.Load("Prefabs/Weapons/ShortSword")) as GameObject;
            Debug.LogWarning("세이브 데이터에 잘못된 무기 이름. 기본으로 초기화.");
            gameData.weaponName = "ShortSword";
        }

        weapon = _weapon.GetComponent<Weapon_Base>();
    }
}
