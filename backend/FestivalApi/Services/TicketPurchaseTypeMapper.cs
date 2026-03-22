namespace FestivalApi.Services;

/// <summary>
/// Mirrors frontend <c>getTicketCodeForPurchaseType</c> in tickets-content.ts.
/// </summary>
public static class TicketPurchaseTypeMapper
{
    private static readonly Dictionary<string, string> PackIds =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["brotherhood"] = "LTHD_BROTHERHOOD",
            ["longtranh"] = "LTHD_LONGTRANH",
            ["hodau"] = "LTHD_HODAU",
            ["metalhead"] = "LTHD_METALHEAD",
            ["vip"] = "LTHD_VIP",
            ["atdoor"] = "LTHD_ATDOOR",
        };

    public static string? TryGetTicketCode(string purchaseType)
    {
        if (string.IsNullOrWhiteSpace(purchaseType))
            return null;
        var key = purchaseType.Trim();
        if (key.Equals("presale", StringComparison.OrdinalIgnoreCase))
            return "LTHD_PRE";
        return PackIds.TryGetValue(key, out var code) ? code : null;
    }
}
