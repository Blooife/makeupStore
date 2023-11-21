namespace makeupStore.Web.Utility;

public class SD
{
    public static string AuthAPIBase { get; set; }
    public static string ProductAPIBase { get; set; }
    public static string StorageAPIBase { get; set; }
    public static string CartAPIBase { get; set; }
    
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string TokenCookie = "JWTToken";
    
    public const string Status_Pending = "Pending";
    public const string Status_Approved = "Approved";
    public const string Status_ReadyForPickup = "ReadyForPickup";
    public const string Status_Completed = "Completed";
    public const string Status_Refunded = "Refunded";
    public const string Status_Cancelled = "Cancelled";
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}