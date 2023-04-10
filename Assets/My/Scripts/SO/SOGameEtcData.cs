using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameName
{
    Battle,
    Math,
    Run
}

[CreateAssetMenu(fileName = "GameDatas", menuName = "GameDatas/EtcData")]
public class SOGameEtcData : ScriptableObject
{
    public EGameName gameName;
    public EScene sceneNum;
    public Sprite bigImg;
    public Sprite smallImg;
    public List<Sprite> rewardImgs;
    public string desc,rewardDesc;
}
