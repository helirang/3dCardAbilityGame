using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyEnterLoading : EnterLoading
{
    [SerializeField] Button enterBtn;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject guidGameObject;

    private void Awake()
    {
        enterBtn.onClick.AddListener(CheckUserNickName);
    }

    void CheckUserNickName()
    {
        if(inputField.text.Length >= 3 && inputField.text.Length <= 16)
        {
            PlayerPrefs.SetString("NickName", inputField.text);
            Enter();
        }
        else
        {
            guidGameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        enterBtn.onClick.RemoveListener(CheckUserNickName);
    }
}
