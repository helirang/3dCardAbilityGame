using System.Collections;
using System.Collections.Generic;

public enum EItemType
{
    Equip,
    Potion,
    Elixir
}

public enum ERarity
{
    Normal,
    Rare,
    Special
}

struct ItemData
{
    readonly string itemID;
    public int Count;
}

public static class PlayerInventory
{
    static List<ItemData> itemDatas;
    static int Gold;
    static int Crystal;
}
