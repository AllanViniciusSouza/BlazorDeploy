using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorDeploy.Shared
{
    // Converts ISO date strings into DateTime preserving Kind when present.
    // If incoming string has no timezone/kind, we assume UTC to avoid losing the time portion.
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrEmpty(s)) return default;

                // Try roundtrip parse which honors kind if present
                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dt))
                {
                    // If parsed value has unspecified kind (no offset in string), treat it as Local
                    if (dt.Kind == DateTimeKind.Unspecified)
                        return DateTime.SpecifyKind(dt, DateTimeKind.Local);
                    return dt;
                }

                // If roundtrip failed, attempt to parse treating value as Local then fallback to Unspecified treated as Local
                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return DateTime.SpecifyKind(dt, DateTimeKind.Local);
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var v))
                {
                    try
                    {
                        var dt = DateTimeOffset.FromUnixTimeMilliseconds(v).UtcDateTime;
                        return dt;
                    }
                    catch { }
                }
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("o", CultureInfo.InvariantCulture));
        }
    }
}
