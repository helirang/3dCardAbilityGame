using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// <para>UI Text와 Slider를 가진 스크립트</para>
/// maxHp를 설정해주면, Slider의 값 변화에 따라 자동으로 Text를 변경해준다.
/// </summary>
public class HPUiController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpTmp;
    [SerializeField] Slider hpSlider;
    int maxHp;

    #region 설정 및 초기화 함수
    private void Awake()
    {
        hpSlider.onValueChanged.AddListener(OnHpChange);
    }

    public void SetMaxHp(int value)
    {
        maxHp = value;
        hpTmp.text = maxHp.ToString();
    }
    #endregion

    public Slider GetHpSlider()
    {
        return hpSlider;
    }

    void OnHpChange(float value)
    {
        hpTmp.text = (maxHp * value).ToString();
    }

    private void OnDestroy()
    {
        hpSlider.onValueChanged.RemoveListener(OnHpChange);
    }
}
