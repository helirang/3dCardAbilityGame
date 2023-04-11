using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCardLoader : MonoBehaviour
{
    [SerializeField] List<SOCard> basicCardList;
    [SerializeField] List<SOCard> allCardLIst;
    Dictionary<int, SOCard> cardDictionary;
    private void Awake()
    {
        //@todo 테스트용 동작 / 차후 변경하기
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
        LoadUserCardData();
    }

    // SOCard는 캐싱해서 사용.
    // SOCard가 보유한 Ability를 Addressables로 관리함.
    // 이유 : Ability에 차후 이펙트 및 애니메이션 교체 등의 기능을 추가할 예정.
    void LoadUserCardData()
    {
        string card = CardStorage.Load();

        if(card != "")
        {
            Debug.Log(card);
        }
        else
        {
            Debug.Log("CardDataIsNone");
        }
        CardStorage.Save();
    }

    void Parser(string data)
    {

    }
}
