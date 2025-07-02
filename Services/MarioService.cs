using Newtonsoft.Json;
using Polly;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using Web_Programming_Assignment_5.Entities;

namespace Web_Programming_Assignment_5.Services
{
    public class MarioService : IMarioService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<MoveEntity?> MakeActionAsync(string action)
        {
            string? responseString = null;

            var policy = Policy.HandleInner<HttpRequestException>((ex) =>
            {
                return ex?.StatusCode == HttpStatusCode.ServiceUnavailable;
            }).WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return await policy.ExecuteAsync(async () =>
            {
                var response = httpClient.GetAsync($"https://webprogrammingmario.azurewebsites.net/api/mario/{action}").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MoveEntity?>(responseString);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new HttpRequestException("Service Unavailable");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException("Mario died");
                }

                return null;


            });
        }
    }
}
