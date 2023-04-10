using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AmmoUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentAmmoTmp;
    [SerializeField] TextMeshProUGUI maxAmmoTmp;
    [SerializeField] TextMeshProUGUI guideTmp;
    public void SetMaxAMMO(int num)
    {
        maxAmmoTmp.text = num.ToString();
        currentAmmoTmp.text = maxAmmoTmp.text;
    }

    public void UpdateCurrentAMMO(int num)
    {
        currentAmmoTmp.text = num.ToString();
    }

    public void ReloadAMMO(bool isReload)
    {
        if (isReload) guideTmp.text = "¿Á¿Â¿¸";
        else 
        {
            guideTmp.text = "≈∫√¢";
            currentAmmoTmp.text = maxAmmoTmp.text;
        };
    }
}
