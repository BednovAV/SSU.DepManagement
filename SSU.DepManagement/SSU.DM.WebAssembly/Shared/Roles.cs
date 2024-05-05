namespace SSU.DM.WebAssembly.Shared;

public class Roles
{
    public const string ADMIN = "admin";

    public static List<string> AllRoles => new()
    {
        ADMIN
    };
}