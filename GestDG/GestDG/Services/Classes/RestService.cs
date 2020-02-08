using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Linq;

namespace GestDG.Services.Classes
{
    public class RestService
    {
        HttpClient client;
       public Cookie cookie_connexion;
        public RestService()
        {
            client = new HttpClient();
        }

        public async Task<Cookie> Login(User user,String url_login)
        {
            Cookie resultat;
            var cookieContainer = new CookieContainer();
            var uri = new Uri(url_login);
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            })
            {
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    await httpClient.GetAsync(uri);
                    var cookie = cookieContainer.GetCookies(uri);
                    resultat = cookie.Count != 0 ? cookie.Cast<Cookie>().FirstOrDefault((item)=>item.Name=="fa_dynamixgaming_forumgaming_fr_sid"):null;
                }
            }
            return resultat;
        }

        public async Task<Stream> getresponse(String url)
        {
            if (cookie_connexion != null)
            {
                try
                {
                    CookieContainer cookies = new CookieContainer();
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.CookieContainer = cookies;
                    handler.CookieContainer.Add(cookie_connexion);

                    HttpClient client = new HttpClient(handler);
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var streamresult = await response.Content.ReadAsStreamAsync();
                        return streamresult;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
