using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionIntegral.WebClient.Services;

internal static class AccessTokenExpiration
{
    public static bool IsExpiredOrInvalid(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return true;

        var parts = token.Split('.');
        if (parts.Length != 3)
            return true;

        try
        {
            using var payload = JsonDocument.Parse(WebEncoders.Base64UrlDecode(parts[1]));
            if (!payload.RootElement.TryGetProperty("exp", out var exp) || !exp.TryGetInt64(out var seconds))
                return true;

            return DateTimeOffset.FromUnixTimeSeconds(seconds) <= DateTimeOffset.UtcNow;
        }
        catch (Exception ex) when (ex is FormatException or JsonException or ArgumentException)
        {
            return true;
        }
    }
}
