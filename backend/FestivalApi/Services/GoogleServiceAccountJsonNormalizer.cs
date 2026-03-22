using System.Text.Json;
using System.Text.Json.Nodes;

namespace FestivalApi.Services;

/// <summary>
/// Fixes common mangling of GCP service account JSON when stored in env vars (e.g. Render, Docker).
/// The PKCS8 error usually means <c>private_key</c> lost real newlines or has literal <c>\n</c> instead.
/// </summary>
public static class GoogleServiceAccountJsonNormalizer
{
    public static string Normalize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return raw ?? "";

        var json = raw.Trim();

        // Accidental extra wrapping quotes around the whole JSON (some UIs)
        if (json.Length >= 2 && json[0] == '"' && json[^1] == '"')
        {
            try
            {
                var unwrapped = JsonSerializer.Deserialize<string>(json);
                if (!string.IsNullOrEmpty(unwrapped))
                    json = unwrapped.Trim();
            }
            catch
            {
                // ignore
            }
        }

        try
        {
            var node = JsonNode.Parse(json);
            if (node is not JsonObject obj)
                return json;

            if (obj["private_key"] is not JsonValue pkVal)
                return json;

            var pk = pkVal.GetValue<string>();
            var fixedPk = FixPrivateKeyPem(pk);
            if (fixedPk != pk)
                obj["private_key"] = fixedPk;

            return obj.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
        }
        catch (JsonException)
        {
            return json;
        }
    }

    private static string FixPrivateKeyPem(string pk)
    {
        if (string.IsNullOrEmpty(pk))
            return pk;

        var s = pk.Trim();

        // After JSON parse, we should have real newlines; env corruption often leaves literal \ + n
        s = s.Replace("\\n", "\n", StringComparison.Ordinal);
        s = s.Replace("\r\n", "\n", StringComparison.Ordinal);

        // If still no PEM markers, try one more unescape pass (double-escaped from some hosts)
        if (!s.Contains("BEGIN PRIVATE KEY", StringComparison.Ordinal))
        {
            s = pk.Trim().Replace("\\\\n", "\n", StringComparison.Ordinal).Replace("\\n", "\n", StringComparison.Ordinal);
        }

        return s;
    }
}
