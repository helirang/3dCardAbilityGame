using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameEtcUIController))]
public class GameUIController : UIController
{
    [SerializeField] List<Button> gameBtns;
    [SerializeField] List<SOGameEtcData> gameEtcDatas;
    [SerializeField] GameEtcUIController etcUi;

    protected override void Awake()
    {
        base.Awake();
        for(int i=0; i < gameBtns.Count; i++)
        {
            EGameName copy = (EGameName)i;
            gameBtns[i].onClick.AddListener(()=>SetEtcData(copy));
            gameBtns[i].image.sprite = gameEtcDatas[i].smallImg;
        }
    }

    void SetEtcData(EGameName gameName)
    {
        SOGameEtcData data = gameEtcDatas[(int)gameName];
        etcUi.Setting((int)data.sceneNum,data.bigImg,
            data.gameName.ToString(),data.desc);
        etcUi.SetActive(true);
    }

    private void OnDisable()
    {
        etcUi.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }
}
