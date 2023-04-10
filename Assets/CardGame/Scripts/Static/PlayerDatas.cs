using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Stats {

    public int str, con, wit, mp;
}

public static class PlayerDatas
{
    public static Stats stats;
    public static Stats runTimeStats;

    public static void Add(Stats addStat,bool isRuntime=false)
    {
        Stats baseStat;
        if (isRuntime) baseStat = runTimeStats;
        else baseStat = stats;

        baseStat.str += addStat.str;
        baseStat.con += addStat.con;
        baseStat.wit += addStat.wit;
        baseStat.mp += addStat.mp;

        if (isRuntime) runTimeStats = baseStat;
        else stats = baseStat;
    }
}

public static class StatCalculator
{
    public static Stats Add(Stats baseStat, Stats addStat)
    {
        Stats resultStats = new Stats();
        resultStats.str = baseStat.str + addStat.str;
        resultStats.str = baseStat.con + addStat.con;
        resultStats.str = baseStat.wit + addStat.wit;
        resultStats.str = baseStat.mp + addStat.mp;

        return resultStats;
    }
}

public static class CardStorage
{
    static Dictionary<int, SOCard> cardDictionary = new Dictionary<int, SOCard>(); 

    /// <summary>
    /// ī�� �����͸� �߰���Ų��
    /// </summary>
    /// <param name="cardData"></param>
    public static void AddCard(SOCard cardData)
    {
        //Ű ������ �� ���̵� �� ���� �ʿ�
        //�ϴ� ī���� �ø��� �ѹ��� ��ü
        //if (!cardDictionary.ContainsKey(cardData.GetInstanceID())) 
        //{
        //    cardDictionary.Add(cardData.GetInstanceID(), cardData);
        //}
        if (!cardDictionary.ContainsKey(cardData.serialNumber))
        {
            cardDictionary.Add(cardData.serialNumber, cardData);
        }
    }

    public static bool CheckSerialNum(int num)
    {
        return cardDictionary.ContainsKey(num);
    }

    /// <summary>
    /// ����� ��� ī�� �����͸� ����
    /// </summary>
    /// <returns></returns>
    public static List<SOCard> GetAllCardDatas()
    {
        List<SOCard> cardDatas = new List<SOCard>();

        foreach(var data in cardDictionary)
        {
            cardDatas.Add(data.Value);
        }

        return cardDatas;
    }

    public static void SaveCard()
    {
        //���̺� �����ϱ�
    }
}

public static class MoneyStorage
{
    public static int zem;
}
