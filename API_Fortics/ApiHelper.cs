using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace API_Fortics
{
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; } = new HttpClient();

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://unimedprudentepoc.sz.chat/api/v4/");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            var _urlBase = config.GetSection("API_Access:UrlBase").Value;

            var _uri = _urlBase + "auth/login";
            var _email = config.GetSection("Api_Access:email").Value;
            var _password = config.GetSection("Api_Access:password").Value;

            Console.WriteLine("\nRealizando autenticação...");
            RunAuthentication(_uri, _email, _password);
        }

        private static void RunAuthentication(string _uri, string _email, string _password)
        {
            HttpResponseMessage respToken = ApiClient.PostAsync(_uri, new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        email = _email,
                        password = _password
                    }), Encoding.UTF8, "application/json")).Result;

            string conteudo = respToken.Content.ReadAsStringAsync().Result;

            //Console.WriteLine(conteudo + "\n");

            if (respToken.StatusCode == HttpStatusCode.OK)
            {
                Token token = JsonConvert.DeserializeObject<Token>(conteudo);
                Console.WriteLine("A U T E N T I C A D O \n");
                ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            }
        }
    }
}
