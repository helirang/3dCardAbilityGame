using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DmgTextPool : MonoBehaviour
{
    public IObjectPool<DamageText> dmgPool;

    [SerializeField] DamageText dmgTextPrefab;
    [SerializeField] Canvas parentCanvas;

    [Tooltip("데미지 텍스트의 위치 오프셋")]
    [SerializeField] Vector3 textOffset = new Vector3(200f, 40f, 0f);

    private void Awake()
    {
        dmgPool = new ObjectPool<DamageText>(CreateDmgText, OnGetDmgText,
            OnReleaseDmgText, OnDestroyDmgText, maxSize: 5);
    }

    DamageText CreateDmgText()
    {
        DamageText dmgText = 
            Instantiate(dmgTextPrefab, parentCanvas.transform).GetComponent<DamageText>();
        dmgText.SetPool(dmgPool);
        return dmgText;
    }

    private void OnGetDmgText(DamageText dmgText)
    {
        dmgText.transform.localPosition += textOffset;
        dmgText.gameObject.SetActive(true);
    }

    private void OnReleaseDmgText(DamageText dmgText)
    {
        dmgText.transform.localPosition = Vector3.zero;
        dmgText.gameObject.SetActive(false);
    }

    private void OnDestroyDmgText(DamageText dmgText)
    {
        if (dmgText != null)
            Destroy(dmgText.gameObject);
    }

    private void OnDestroy()
    {
        dmgPool.Clear();
    }
}
