using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

public class UserCardLoader : MonoBehaviour
{
    [SerializeField] List<SOCard> basicCardList;

    private void Awake()
    {
        //@todo �׽�Ʈ�� ���� / ���� �����ϱ�
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
    }

    // @todo ��� SOCard�� ȣ���ϴ� ��� �����ϱ� or ��� SOCard�� ĳ���ϱ�
    // SOCard�� ��巹���� �������� �����ϱ�
    // AssetReference.Asset�� Ȱ���� ������ ���� SOCard�� �ε��ϱ�
    void LoadUserCardData()
    {

    }
}
