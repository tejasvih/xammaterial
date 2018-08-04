/*
 * Version 1.1 
 * Updates on returning results
 * */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Calibre.Xam.Log;
using Newtonsoft.Json;
using Plugin.Connectivity;
using static Calibre.Xam.Log.Logger;

namespace Calibre.REST
{
    public class RestService //: IRestService
    {
        HttpClient client;
        public const int NO_CONNECTION = -1;
        public const int ERROR = -2;
        public const int SEND_ERROR = -3;
        public const int RECEIVE_ERROR = -4;
        //public const int SERVER_NOT_REACHABLE_ERROR = -5;
        public const int OK = 0;
        public const int NO_DATA = -5;

        public const string FORMAT_DATA = "data";
        public const string FORMAT_JSON = "json";
        public static RestService GetInstance() { return new RestService(); }
        LogDelegate log { get; set; }

        public RestService()
        {
            var authData = string.Format("{0}:{1}", Calibre.Settings.LoggedInUserName, Calibre.Settings.AuthToken);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
            
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);

            client.MaxResponseContentBufferSize = 256000*1024;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }
        public RestService(LogDelegate logger) : base()
        {
            log = logger;
        }
        async public static Task<bool> IsServerReachableAndRunning()
        {
            var connectivity = CrossConnectivity.Current;
            if (!connectivity.IsConnected)
                return false;

            //return true;
            
            var host = Calibre.Constants.Host;
            var port = Calibre.Constants.Port;

            var reachable = await connectivity.IsRemoteReachable(host, port, 3000);
            return reachable;
            
        }


        public async Task<(int code, string message, string data)> PostData(object data, string url)
        {
            /*if (!(await IsServerReachableAndRunning()))
            {
                return (NO_CONNECTION, "Not Connected", null);
            }*/
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 3000; //ms
            var itemToSend = JsonConvert.SerializeObject(data);
            try
            {
                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    streamWriter.Write(itemToSend);
                    streamWriter.Flush();
                    streamWriter.Dispose();
                }
            } catch (Exception ex)
            {
                //Debug.WriteLine(@"GetRequestStreamAsync ERROR {0}", ex.Message);
                log(this,LogType.ERROR, $"GetRequestStreamAsync ERROR {ex.Message}");
                return (SEND_ERROR, ex.Message, null); 
            }
            
            try
            {

            // Send the request to the server and wait for the response:  
            using (var response = await request.GetResponseAsync())
            {

                    // Get a stream representation of the HTTP web response:  
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        var message = reader.ReadToEnd();
                        return (OK, null, message);

                    }
            }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"GetResponseAsync {0}", ex.Message);
                log(this, LogType.ERROR, $"PostData ERROR {ex.Message}");
                return (RECEIVE_ERROR, ex.Message,null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="data"></param>
        /// <param name="format">data/json</param>
        /// <returns></returns>
        public async Task<(int code ,List<T> data)> ExecuteApiAsync<T>(string apiName, object data = null, string format = RestService.FORMAT_DATA)
        {
            List<T> Items = new List<T>();
            /*if (!(await IsServerReachableAndRunning()))
            {
                return (NO_CONNECTION,null);
            }*/
            var uri = new Uri(Calibre.Constants.RestUrl);
            try
            {
                var postItem = new
                {
                    ApiName = apiName,
                    AppData = new
                    {
                        EmpId = Settings.LoggedInUserId
                    },
                    Data = data
                };
                var json = JsonConvert.SerializeObject(postItem);
                var postContent = new StringContent(json, Encoding.UTF8, "application/json");
                /*HttpResponseMessage response = await client.PostAsync(uri, postContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully got data.");
                    var resultContent = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<T>>(resultContent);
                }*/
                
                var response = await PostData(postItem, (format == RestService.FORMAT_JSON ?  Calibre.Constants.RestJsonUrl : Calibre.Constants.RestUrl));
                if (response.code == OK)
                {
                    Debug.WriteLine(@"successfully got data.");
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    Items = JsonConvert.DeserializeObject<List<T>>(response.data, settings);
                    return (OK, Items);
                }
                return (response.code, null);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"Get Data ERROR {0}", ex.Message);
                log(this, LogType.ERROR, $"ExecuteApiAsync ERROR {ex.Message}");
                return (ERROR, null);
                
            }

            
        }
        public async Task<(int code, List<T> data)> GetDataAsync<T>(object data = null, string format = RestService.FORMAT_DATA, string apiName = null)
        {
            /*if (!(await IsServerReachableAndRunning()))
            {
                return (NO_CONNECTION, null);
                
            }*/
            if (apiName == null)
                apiName = $"{Constants.ApiListingPrefix}{typeof(T).Name}{Constants.ApiListingSuffix}";
            var result = await ExecuteApiAsync<T>(apiName, data,format);
            return (result.code, result.data);
        }

        public async Task<bool> SaveAsync(object data, Type type = null)
        {
            /*if (!(await IsServerReachableAndRunning()))
            {
                return false;
            }*/
            if (type == null)
                type = data.GetType();

            //var uri = new Uri(Calibre.Constants.RestJsonUrl);
            var uri = new Uri(Calibre.Constants.RestUrl);
            bool result = false;
            try
            {

                var postItem = new
                {
                    ApiName = $"{Constants.ApiSavePrefix}{type.Name}{Constants.ApiSaveSuffix}",
                    AppData = new
                    {
                        EmpId = Settings.LoggedInUserId
                    },
                    Data = data
                };
                var json = JsonConvert.SerializeObject(postItem);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                //var responseText = await response?.Content?.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    result = true;
                    Debug.WriteLine(@"Successfully saved.");
                }

            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"Saving ERROR {0}", ex.Message);
                log(this, LogType.ERROR, $"SaveAsync ERROR {ex.Message}");
            }
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            
            var uri = new Uri(Calibre.Constants.RestUrl);
            try
            {
                var response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"Successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"ERROR {0}", ex.Message);
                log(this, LogType.ERROR, $"DeleteAsync ERROR {ex.Message}");
            }
        }

        public async Task<bool> UploadData(string url,byte[] data, string fileName,Dictionary<string,string> values)
        {
            try
            {
                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                //HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(data);

                content.Add(baContent, "File", fileName);
                foreach(var key in values.Keys)
                {
                    content.Add(new StringContent(values[key]), key);
                }
                
                //upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, content);

                //read response result as a string async into json var
                //var responsestr = response.Content.ReadAsStringAsync().Result;

                //debug
                //Debug.WriteLine(responsestr);
                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                //debug
                //Debug.WriteLine("Exception Caught: " + e.ToString());
                log(this, LogType.ERROR, $"UploadData ERROR {ex.Message}");

                return false;
            }
            
        }
        public async Task<Stream> DownloadData(string url, Dictionary<string, string> values = null)
        {
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                if (values != null)
                {
                    //to be tested
                    NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                    foreach (var key in values.Keys)
                    {
                        queryString[key] = values[key];
                    }
                    var qs = queryString.ToString();
                    if (url.Contains("?"))
                    {
                        url += "&" + qs;
                    }
                    else
                    {
                        url += "?" + qs;
                    }
                }
                
                
                var response = await client.GetStreamAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                //debug
                //Debug.WriteLine("Exception Caught: " + e.ToString());
                log(this, LogType.ERROR, $"DownloadData ERROR {ex.Message}");

                return null;
            }

        }
    }
}
