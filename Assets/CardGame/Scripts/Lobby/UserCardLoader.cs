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

        //�⺻ ī�� �ֱ�
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
    }

    // SOCard�� ĳ���ؼ� ���.
    // SOCard�� ������ Ability�� Addressables�� ������.
    // ���� : Ability�� ���� ����Ʈ �� �ִϸ��̼� ��ü ���� ����� �߰��� ����.
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
            CustomDebugger.Debug(LogType.LogError, $"ī�帮��Ʈ�� ���� �ø����ȣ : {serialNumber} �Դϴ�.");
        }

        return result;
    }
}
