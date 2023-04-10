using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(EnterLoading))]
public class GameEtcUIController : MonoBehaviour
{
    [SerializeField] GameObject etcUiObj;
    [SerializeField] Button playBtn,closeBtn;
    [SerializeField] Image gameImg;
    [SerializeField] TextMeshProUGUI gameName,gameDesc;
    [SerializeField] EnterLoading enterLoader;
    EScene eScene;

    void Awake()
    {
        playBtn.onClick.AddListener(enterLoader.Enter);
        closeBtn.onClick.AddListener(()=>SetActive(false));
    }

    public void Setting(int sceneNum, Sprite sprite,string name, string desc)
    {
        eScene = (EScene)sceneNum;
        gameImg.sprite = sprite;
        gameName.text = name;
        gameDesc.text = desc;

        enterLoader.nextSene = eScene;
    }

    public void SetActive(bool isActive)
    {
        etcUiObj.SetActive(isActive);
    }

    void OnDestroy()
    {
        closeBtn.onClick.RemoveAllListeners();
        playBtn.onClick.RemoveAllListeners();
    }
}
