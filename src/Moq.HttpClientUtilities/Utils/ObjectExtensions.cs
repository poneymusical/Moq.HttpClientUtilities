using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Utils
{
    internal static class ObjectExtensions
    {
        internal static string ToJsonString(this object obj) => 
            JsonConvert.SerializeObject(obj, Formatting.None);
    }
}