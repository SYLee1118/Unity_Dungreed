using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingUI : MonoBehaviour
{
    [SerializeField] private Text rightKeyText;
    [SerializeField] private Text leftKeyText;
    [SerializeField] private Text downKeyText;
    [SerializeField] private Text jumpKeyText;

    [SerializeField] private Button setRightKey;
    [SerializeField] private Button setLeftKey;
    [SerializeField] private Button setDownKey;
    [SerializeField] private Button setJumpKey;

    private Image selectedButtonImage;
    private Key key;

    private void Awake()
    {
        selectedButtonImage = null;
        key = Key.NONE;

        // 키 변경 버튼에 이벤트 추가
        setLeftKey.onClick.AddListener(() => ChangeKeyButton(Key.LEFT, setLeftKey.GetComponent<Image>()));
        setRightKey.onClick.AddListener(() => ChangeKeyButton(Key.RIGHT, setRightKey.GetComponent<Image>()));
        setDownKey.onClick.AddListener(() => ChangeKeyButton(Key.DOWN, setDownKey.GetComponent<Image>()));
        setJumpKey.onClick.AddListener(() => ChangeKeyButton(Key.JUMP, setJumpKey.GetComponent<Image>()));
    }

    // 현재 키셋팅 상황을 Text에 출력
    public void ShowCurrentKeySetting()
    {
        rightKeyText.text = KeyManager.Instance.currentKey(Key.RIGHT).ToString();
        leftKeyText.text = KeyManager.Instance.currentKey(Key.LEFT).ToString();
        downKeyText.text = KeyManager.Instance.currentKey(Key.DOWN).ToString();
        jumpKeyText.text = KeyManager.Instance.currentKey(Key.JUMP).ToString();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        // Button 이벤트인 ChangeKeyButton에 의해서 key값이 변경되었을때만, 1회 진입하도록.
        if(e.isKey && key != Key.NONE)
        {
            KeyManager.Instance.ChangeKey((Key)key, e.keyCode);
            ShowCurrentKeySetting();
            // 키 변경이 끝나면 하이라이트를 해제
            selectedButtonImage.color = new Color(1.0f, 1.0f, 1.0f);
            selectedButtonImage = null;
            key = Key.NONE;
        }
    }

    public void ChangeKeyButton(Key key, Image buttonImage)
    {
        // 현재 다른 키를 변경중이라면 return
        if (selectedButtonImage != null)
            return;

        // 현재 변경하려는 키의 버튼을 빨간색으로 하이라이트.
        selectedButtonImage = buttonImage;
        selectedButtonImage.color = new Color(1.0f, 0.5f, 0.5f);

        this.key = key;
    }
}
