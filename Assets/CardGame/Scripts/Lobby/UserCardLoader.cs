using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCardLoader : MonoBehaviour
{
    [SerializeField] List<SOCard> basicCardList;
    [SerializeField] List<SOCard> allCardLIst;
    Dictionary<int, SOCard> cardDicBySerialNumber = new Dictionary<int, SOCard>();
    private void Awake()
    {
        foreach (var data in allCardLIst)
        {
            cardDicBySerialNumber.Add(data.serialNumber, data);
        }

        LoadUserCardData();

        //기본 카드 넣기
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
    }

    // SOCard는 캐싱해서 사용.
    // SOCard가 보유한 Ability를 Addressables로 관리함.
    // 이유 : Ability에 차후 이펙트 및 애니메이션 교체 등의 기능을 추가할 예정.
    void LoadUserCardData()
    {
        string serialNumbers = CardStorage.Load();

        if(serialNumbers != "")
        {
            Parser(serialNumbers);
        }
    }

    void Parser(string data)
    {
        string trim = CardStorage.trim;

        string[] words = data.Split(trim);

        foreach(var word in words)
        {
            int serialNumber = 0;
            
            if(int.TryParse(word, out serialNumber))
            {
                AddCardStorage(serialNumber);
            }
        }
    }

    bool AddCardStorage(int serialNumber)
    {
        bool result = false;

        if (cardDicBySerialNumber.ContainsKey(serialNumber))
        {
            CardStorage.AddCard(cardDicBySerialNumber[serialNumber]);
            result = true;
        }
        else
        {
            CustomDebugger.Debug(LogType.LogError, $"카드리스트에 없는 시리얼번호 : {serialNumber} 입니다.");
        }

        return result;
    }
}
