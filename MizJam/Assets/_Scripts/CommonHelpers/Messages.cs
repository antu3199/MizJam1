public class Messages {
    public static string OnJoinChannel = "OnJoinChannel"; // void
    public static string OnGoldUpdate = "OnGoldUpdate";
    public static string OnGPSUpdate = "OnGPSUpdate";

    public static string OnItemBuy = "OnItemBuy";

    public static string OnStatsUpdate = "OnStatsUpdate";
}

public enum Resource {
    GOLD,
    GPS,
    STAT_ATTACK,
    ITEM
};

public class ResourceUpdate {
    public Resource resource;
    public double valueBefore;
    public double valueAfter;

    public ResourceUpdate(Resource resource, double valueBefore, double valueAfter) {
        this.resource = resource;
        this.valueBefore = valueBefore;
        this.valueAfter = valueAfter;
    }
};


public class ItemUpdate : ResourceUpdate {
    public int itemIndex;

    public ItemUpdate(int itemIndex, double valueBefore, double valueAfter) 
    : base(Resource.ITEM, valueBefore, valueAfter)
    {
        this.itemIndex = itemIndex;
    }
}
