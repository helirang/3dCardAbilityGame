using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// <para>UI Text�� Slider�� ���� ��ũ��Ʈ</para>
/// maxHp�� �������ָ�, Slider�� �� ��ȭ�� ���� �ڵ����� Text�� �������ش�.
/// </summary>
public class HPUiController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpTmp;
    [SerializeField] Slider hpSlider;
    int maxHp;

    #region ���� �� �ʱ�ȭ �Լ�
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
