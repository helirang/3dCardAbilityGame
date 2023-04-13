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
    static Dictionary<int, SOCard> cardDicBySerialNumber = new Dictionary<int, SOCard>();
    public static readonly string trim = ","; 
    /// <summary>
    /// 카드 데이터를 추가시킨다
    /// </summary>
    /// <param name="cardData"></param>
    public static void AddCard(SOCard cardData)
    {
        if (!cardDicBySerialNumber.ContainsKey(cardData.serialNumber))
        {
            cardDicBySerialNumber.Add(cardData.serialNumber, cardData);
        }
    }

    public static bool CheckSerialNum(int num)
    {
        return cardDicBySerialNumber.ContainsKey(num);
    }

    /// <summary>
    /// 저장된 모든 카드 데이터를 리턴
    /// </summary>
    /// <returns></returns>
    public static List<SOCard> GetAllCardDatas()
    {
        List<SOCard> cardDatas = new List<SOCard>();

        foreach(var data in cardDicBySerialNumber)
        {
            cardDatas.Add(data.Value);
        }

        return cardDatas;
    }

    public static void Save()
    {
        string saveData = "";

        foreach(var CardSerialNum in cardDicBySerialNumber.Keys)
        {
            saveData += $"{CardSerialNum}{trim}";
        }
        UnityEngine.PlayerPrefs.SetString("Cards",saveData);
    }

    public static string Load()
    {
        return UnityEngine.PlayerPrefs.GetString("Cards", null);
    }
}

public static class MoneyStorage
{
    static int zem;
    public static int ZEM { 
        get { return zem; }
        set 
        { 
            zem = value;
            Save();
        } 
    }

    public static void Load()
    {
        zem = UnityEngine.PlayerPrefs.GetInt("ZEM",0);
    }

    static void Save()
    {
        UnityEngine.PlayerPrefs.SetInt("ZEM", zem);
    }
}
