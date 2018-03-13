using System;

namespace Constants
{
    public class ApiUrls
    {
        private static string _apiUrl;

        public static void SetApiUrls(string url)
        {
            _apiUrl = url;
        }

        public class Login
        {
            private static readonly string BaseUrl = $"{_apiUrl}/login";

            public string Customer = $"{BaseUrl}/customer";
        }
    }
}
