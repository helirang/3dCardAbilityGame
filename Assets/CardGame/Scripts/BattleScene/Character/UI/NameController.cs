using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTmp;

    public void SetName(string name)
    {
        nameTmp.text = name;
    }
}
