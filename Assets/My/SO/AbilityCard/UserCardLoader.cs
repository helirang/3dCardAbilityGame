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
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
    }

    //��ü SOCard�� ���⿡ ĳ���ϱ�
    //�ش� �����͸� ����Ͽ� Load ��� �����ϱ�
}
