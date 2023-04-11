using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/card")]
public class SOCard : ScriptableObject
{
    public Sprite sprite;

    /// <summary>
    /// @todo ability 개수가 늘어나면 list로 바꿔서 조합해서 사용하기
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

    // abilityAsset 로드 완료되면 알려주는 함수 구현하기
    // 로드 완료되면 ability에 데이터 넣기
    // Battle 씬 끝나고 나올 때, Ability번들 전부 언로드하기
}

[System.Serializable]
public class AssetReferenceAbility : AssetReferenceT<Ability_Origin>
{
    public AssetReferenceAbility(string guid) : base(guid) { }
}
