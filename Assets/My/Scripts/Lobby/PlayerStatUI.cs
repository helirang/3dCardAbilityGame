using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
    [Tooltip("0 ¿Ã∏ß, 1 str, 2 con, 3 wit, 4 mp")]
    [SerializeField] TextMeshProUGUI[] tmps;

    private void OnEnable()
    {
        tmps[0].text = PlayerPrefs.GetString("NickName");
        tmps[1].text = $"Str : {PlayerDatas.stats.str.ToString()}";
        tmps[2].text = $"Con : {PlayerDatas.stats.con.ToString()}";
        tmps[3].text = $"Wit : {PlayerDatas.stats.wit.ToString()}";
        tmps[4].text = $"MP : {PlayerDatas.stats.mp.ToString()}";
    }
}
