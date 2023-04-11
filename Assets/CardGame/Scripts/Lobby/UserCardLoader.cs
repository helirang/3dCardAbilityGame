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
        //@todo �׽�Ʈ�� ���� / ���� �����ϱ�
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
        LoadUserCardData();
    }

    // SOCard�� ĳ���ؼ� ���.
    // SOCard�� ������ Ability�� Addressables�� ������.
    // ���� : Ability�� ���� ����Ʈ �� �ִϸ��̼� ��ü ���� ����� �߰��� ����.
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
