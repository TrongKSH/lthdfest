namespace FestivalApi.Services;

/// <summary>
/// Mirrors frontend <c>getTicketCodeForPurchaseType</c> in tickets-content.ts.
/// </summary>
public static class TicketPurchaseTypeMapper
{
    private static readonly Dictionary<string, string> PackIds =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["brotherhood"] = "LTHD-CMB",
            ["longtranh"] = "LTHD-GA1",
            ["hodau"] = "LTHD-GA2",
            ["metalhead"] = "LTHD-TMH",
            ["vip"] = "LTHD-VIP",
            ["atdoor"] = "LTHD-ATDOOR",
        };

    public static string? TryGetTicketCode(string purchaseType)
    {
        if (string.IsNullOrWhiteSpace(purchaseType))
            return null;
        var key = purchaseType.Trim();
        if (key.Equals("presale", StringComparison.OrdinalIgnoreCase))
            return "LTHD-GA2D";
        return PackIds.TryGetValue(key, out var code) ? code : null;
    }
}
