public class Messages {
    public static string OnJoinChannel = "OnJoinChannel"; // void
    public static string OnGoldUpdate = "OnGoldUpdate";
    public static string OnGPSUpdate = "OnGPSUpdate";

    public static string OnItemBuy = "OnItemBuy";
}

public enum Resource {
    GOLD,
    GPS,
    STAT_ATTACK
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


