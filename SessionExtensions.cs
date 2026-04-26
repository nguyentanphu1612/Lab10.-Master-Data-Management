using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ASC.Utilities
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            var jsonString = JsonSerializer.Serialize(value);
            session.SetString(key, jsonString);
        }

        public static T? GetObject<T>(this ISession session, string key) where T : class
        {
            var jsonString = session.GetString(key);
            if (string.IsNullOrEmpty(jsonString))
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}