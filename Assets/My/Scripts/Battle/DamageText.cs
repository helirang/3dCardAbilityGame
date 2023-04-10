using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class DamageText : MonoBehaviour
{
    float moveSpeed, alphaSpeed, destroyTime;
    [SerializeField] TextMeshProUGUI tmp;
    IObjectPool<DamageText> dmgTextPool;
    Color alpha;
    Color baseColor;

    private void Awake()
    {
        moveSpeed = 2f;
        alphaSpeed = 2f;
        destroyTime = 1f;

        if(tmp == null) tmp = this.GetComponent<TextMeshProUGUI>();
        baseColor = tmp.color;
    }

    public void SetPool(IObjectPool<DamageText> pool)
    {
        dmgTextPool = pool;
    }

    public void SetDamage(int dmg)
    {
        tmp.text = dmg.ToString();
        tmp.color = baseColor;
        alpha = baseColor;
        StartCoroutine(Timer(destroyTime));
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        tmp.color = alpha;
    }

    IEnumerator Timer(float time)
    {
        WaitForSeconds waitTime = new WaitForSeconds(time);
        yield return waitTime;
        dmgTextPool.Release(this);
    }
}
