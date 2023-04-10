using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [SerializeField] SOCard cardData;
    [SerializeField] Button button;

    private void Awake()
    {
        button.onClick.AddListener(TestT);
    }

    public void TestT()
    {
        CardStorage.AddCard(cardData);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(TestT);
    }
}
