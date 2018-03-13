using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ApiCall.Static;
using Newtonsoft.Json;

namespace ApiCall
{
    public abstract class BaseHelper
    {
        public abstract string _token { get; set; }

        protected BaseHelper(string token)
        {
            _token = token;
        }

        protected internal int _Insert<T>(string apiUrl, T model)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                var obj = JsonConvert.SerializeObject(model);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(obj, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync(client.BaseAddress, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);

                        return result;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return (int)Enums.PostErrors.Exception;
                }

                return (int)Enums.PostErrors.NotSuccess;
            }
        }

        protected internal bool _Update<T>(string apiUrl, T model)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                var obj = JsonConvert.SerializeObject(model);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(obj, Encoding.UTF8, "application/json");
                try
                {
                    var response = client.PutAsync(client.BaseAddress, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return false;
                }
                
                return false;
            }   
        }

        protected internal bool _Delete(string apiUrl, int id, int idDeletionAccount)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                var urlParams = $"{id}/{idDeletionAccount}";

                try
                {
                    var response = client.DeleteAsync(urlParams).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return false;
                }
                
                return false;
            }
        }

        protected internal T _Get<T>(string apiUrl)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                try
                {
                    var response = client.GetAsync(client.BaseAddress).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

                        return result;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return default(T);
                }
                
                return default(T);
            }
        }
        
        protected internal T _Get<T>(string apiUrl, string parameter)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                try
                {
                    var response = client.GetAsync(parameter).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

                        return result;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return default(T);
                }

                return default(T);
            }
        }

        protected internal T _Get<T>(string apiUrl, string parameter1, string parameter2)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                var paramStr = $"{parameter1}/{parameter2}";

                try
                {
                    var response = client.GetAsync(paramStr).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

                        return result;
                    }
                }
                catch (Exception e)
                {
                    //log
                    
                    return default(T);
                }

                return default(T);
            }
        }
        
        protected internal T _Get<T, Q>(string apiUrl, Q objectParameter)
        {
            using (HttpClient client = CreateHttpClient(apiUrl))
            {
                var obj = JsonConvert.SerializeObject(objectParameter);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(obj, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync(client.BaseAddress, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

                        return result;
                    }
                }
                catch (Exception e)
                {
                    //log

                    return default(T);
                }
                
                return default(T);
            }
        }

        #region Function

        private HttpClient CreateHttpClient(string apiUrl)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = Encoding.ASCII.GetBytes(_token);
            var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(data));
            client.DefaultRequestHeaders.Authorization = header;

            return client;
        }

        #endregion

    }
}
