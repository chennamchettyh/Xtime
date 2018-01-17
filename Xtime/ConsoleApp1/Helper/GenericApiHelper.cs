using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Domain;
using Newtonsoft.Json;

namespace ConsoleApp1.Helper
{
    public static class GenericApiHelper<T>
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

                    HttpResponseMessage response = await client.GetAsync(apiEndPoint);
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
                //    client.DefaultRequestHeaders.Accept.Clear();
                //  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(apiEndPoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    T answerResponse = JsonConvert.DeserializeObject<T>(responseString);
                }
                throw new Exception("Error occured during api call execution");
            }

        }

       

    }
}
