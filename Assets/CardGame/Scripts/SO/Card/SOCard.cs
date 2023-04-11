using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/card")]
public class SOCard : ScriptableObject
{
    public Sprite sprite;

    /// <summary>
    /// @todo ability ������ �þ�� list�� �ٲ㼭 �����ؼ� ����ϱ�
    /// </summary>
    [SerializeField] AssetReferenceAbility abilityAsset;
    [System.NonSerialized] public Ability_Origin ability;
    public ETeamNum targetTeam = ETeamNum.User;
    public int cost = 0;
    public int price = 0;
    public int serialNumber = 0;
    public string desc = "";
    public string GetDesc()
    {
        return desc;
    }

    // abilityAsset �ε� �Ϸ�Ǹ� �˷��ִ� �Լ� �����ϱ�
    // �ε� �Ϸ�Ǹ� ability�� ������ �ֱ�
    // Battle �� ������ ���� ��, Ability���� ���� ��ε��ϱ�
}

[System.Serializable]
public class AssetReferenceAbility : AssetReferenceT<Ability_Origin>
{
    public AssetReferenceAbility(string guid) : base(guid) { }
}
