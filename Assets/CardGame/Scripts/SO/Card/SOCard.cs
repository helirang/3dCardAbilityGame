using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/card")]
public class SOCard : ScriptableObject
{
    public Sprite sprite;

    public AssetReferenceAbility abilityAsset;
    public AsyncOperationHandle<Ability_Origin> abilityOperation;

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
    public void OnAbilityComplete(Ability_Origin ability , 
        AsyncOperationHandle<Ability_Origin> handle)
    {
        this.ability = ability;
        abilityOperation = handle;
    }

    public void UnLoadAbility()
    {
        //@todo 중복 능력 시스템으로 발전시키려면 어빌리트의 로드 및 언로드를 스크립트 필요
        if (ability != null && abilityOperation.IsValid())
        {
            ability = null;
            Addressables.Release(abilityOperation);
            CustomDebugger.Debug(LogType.Log,
                "어빌리티 언로드 완료");
        }
    }

    private void OnDestroy()
    {
        UnLoadAbility();
    }
}

[System.Serializable]
public class AssetReferenceAbility : AssetReferenceT<Ability_Origin>
{
    public AssetReferenceAbility(string guid) : base(guid) { }
}
