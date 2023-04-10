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

    //전체 SOCard를 여기에 캐싱하기
    //해당 데이터를 사용하여 Load 기능 구현하기
}
