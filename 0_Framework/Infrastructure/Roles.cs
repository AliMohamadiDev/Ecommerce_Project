namespace _0_Framework.Infrastructure;

public static class Roles
{
    public const string Administration = "1";
    public const string SystemUser = "2";
    public const string BlogAdmin = "3";

    public static string GetRoleBy(long id)
    {
        switch (id)
        {
            case 1: return "مدیر سیستم";
            case 3: return "مدیر بلاگ";
            default: return "";
        }
    }
}