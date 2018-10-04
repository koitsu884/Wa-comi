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

namespace Integration
{
    public class TestContext
    {
        private TestServer _server;
        public HttpClient Client {get; private set;}

        public TestContext(){
            SetUpClient();
        }

        private void SetUpClient()
        {
            // var projectDir = System.IO.Directory.GetCurrentDirectory();
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                // .UseContentRoot(projectDir)
                .UseEnvironment("Development")
                // .UseConfiguration(new ConfigurationBuilder()
                //     .SetBasePath(projectDir)
                //     .AddJsonFile("appsettings.json")
                //     .AddJsonFile("appsettings.Development.json")
                //     .Build()
                // )
                .UseStartup<TestStartup>());

            Client = _server.CreateClient();
        }

        public void Dispose(){
            Client.Dispose();
            _server.Dispose();
        }
    }
}