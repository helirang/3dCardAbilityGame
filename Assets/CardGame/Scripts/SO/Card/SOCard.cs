using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/card")]
public class SOCard : ScriptableObject
{
    public Sprite sprite;
    public Ability_Origin ability;
    public ETeamNum targetTeam = ETeamNum.User;
    public int cost = 0;
    public int price = 0;
    public int serialNumber = 0;
    public string GetDesc()
    {
        string cardDesc = "";

        if(ability != null)
        {
            cardDesc = $"{ability.desc}";
        }

        return cardDesc;
    }
}
