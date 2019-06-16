using Microsoft.AspNetCore.Http;

namespace DatingApplication.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse respon, string message)
        {
            respon.Headers.Add("Application-Error", message);
            respon.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            respon.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}