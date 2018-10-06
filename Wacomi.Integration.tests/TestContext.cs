using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Wacomi.API;
using Microsoft.AspNetCore;
using Wacomi.Integration.tests;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wacomi.API.Dto;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Integration
{
    public class TestContext : IDisposable
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        public TestContext(){
                this._server = new TestServer(WebHost.CreateDefaultBuilder()
                // .UseContentRoot(projectDir)
                .UseEnvironment("Development")
                // .UseConfiguration(new ConfigurationBuilder()
                //     .SetBasePath(projectDir)
                //     .AddJsonFile("appsettings.json")
                //     .AddJsonFile("appsettings.Development.json")
                //     .Build()
                // )
                .UseStartup<TestStartup>());

            _client = _server.CreateClient();
        }

        public void Dispose(){
            _client.Dispose();
            _server.Dispose();
        }

        protected StringContent BuildStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
        protected async Task LoginByUser(string userName){
            var contents = new StringContent(JsonConvert.SerializeObject(new UserLoginDto{ UserName = userName, Password = "P@ssw0rd!!"}), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/auth/login", contents);
            var result = response.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<JObject>(result.Result);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult["tokenString"].ToObject<string>());
        }
    }
}