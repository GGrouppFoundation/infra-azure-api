namespace GGroupp.Infra;

internal static class ApiNames
{
    public const string IdFieldName = "id";

    public const string MailFieldName = "mail";

    public const string DisplayNameFieldName = "displayName";

    public const string MeRelativeUrl = "/v1.0/me?$select=" + IdFieldName + "," + MailFieldName + "," + DisplayNameFieldName;
}