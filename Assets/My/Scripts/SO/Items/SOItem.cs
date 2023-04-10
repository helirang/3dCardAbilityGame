using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Items", menuName = "Items/item")]
[System.Serializable]
public class SOItem : ScriptableObject
{
    [SerializeField] EItemType itemType;
    [SerializeField] ERarity rarity;
    [SerializeField] string id;
    [SerializeField] int price;
    [SerializeField] Sprite sprite;
    [SerializeField] List<SOItemAbilty> abilties;
}
