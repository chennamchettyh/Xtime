using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VAuto.Services
{
    public static class ApiHelper<T>
    {
        public static async Task<T> GetResult(string apiEndPoint)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://vautointerview.azurewebsites.net");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(apiEndPoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        T datasetIdResponse = JsonConvert.DeserializeObject<T>(responseString);
                        return datasetIdResponse;
                    }
                    throw new Exception("Error occured during api call execution");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<T> PostAnswerResponse(string apiEndPoint, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://vautointerview.azurewebsites.net");
               
                var response = await client.PostAsync(apiEndPoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    T answerResponse = JsonConvert.DeserializeObject<T>(responseString);
                    return answerResponse;
                }
                throw new Exception("Error occured during api call execution");
            }

        }

       

    }
}
