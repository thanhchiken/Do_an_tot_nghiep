using Newtonsoft.Json;
namespace ShoppingProject.Repository
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession sessoin, string key, object value)
        {
            sessoin.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetJson<T>(this ISession sessoin, string key)
        {
            var sessionData = sessoin.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
