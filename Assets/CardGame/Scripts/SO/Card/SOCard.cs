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

    // abilityAsset �ε� �Ϸ�Ǹ� �˷��ִ� �Լ� �����ϱ�
    // �ε� �Ϸ�Ǹ� ability�� ������ �ֱ�
    // Battle �� ������ ���� ��, Ability���� ���� ��ε��ϱ�
    public void AbilityComplete(Ability_Origin ability , 
        AsyncOperationHandle<Ability_Origin> handle)
    {
        this.ability = ability;
        abilityOperation = handle;
    }

    public void UnLoadAbility()
    {
        if (abilityOperation.IsValid())
        {
            Addressables.Release(abilityOperation);
        }
        else
        {
            CustomDebugger.Debug(LogType.LogError, "ability ���۷��̼��� ��ȿ���� �ʽ��ϴ�.");
        }
    }

    private void OnDestroy()
    {
        if (abilityOperation.IsValid())
        {
            Addressables.Release(abilityOperation);
        }
    }
}

[System.Serializable]
public class AssetReferenceAbility : AssetReferenceT<Ability_Origin>
{
    public AssetReferenceAbility(string guid) : base(guid) { }
}
