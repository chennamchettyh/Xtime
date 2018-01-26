using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Models;

namespace VAuto.Services
{
    public static class ApiHandler
    {

        public static async Task<string> GetDataSetId()
        {
            var apiEndPoint = "api/datasetid";
            var response = await ApiHelper<DataSet>.GetResult(apiEndPoint);
            return response.DatasetId;
        }
        public static async Task<IEnumerable<int>> GetVehicleIdList(string datasetid)
        {
            var apiEndPoint = string.Format("api/{0}/vehicles", datasetid);
            var response = await ApiHelper<Vehicles>.GetResult(apiEndPoint);
            return response.vehicleIds;
        }
        public static async Task<Vehicle> GetVehicleInfo(string datasetid, int vehicleid)
        {
            var apiEndPoint = string.Format("api/{0}/vehicles/{1}", datasetid, vehicleid);
            var response = await ApiHelper<Vehicle>.GetResult(apiEndPoint);
            return response;
        }
        public static async Task<Dealer> GetDealerInfo(string datasetid, int dealerid)
        {
            var apiEndPoint = string.Format("api/{0}/dealers/{1}", datasetid, dealerid);
            var response = await ApiHelper<Dealer>.GetResult(apiEndPoint);
            return response;
        }
        public static async Task<string> PostAnswer(string datasetId, Dealers dealers)
        {
            
            var apiEndPoint = string.Format("api/{0}/answer", datasetId);
            var content = new StringContent(JsonConvert.SerializeObject(dealers), Encoding.UTF8, "application/json");
            var response = await ApiHelper<FinalResponse>.PostAnswerResponse(apiEndPoint, content);
            if (response != null)
            {
                return string.Format("Answer  Message: {0} TotalMillisecons: {1} IsSuccess: {2}", response.message, response.totalMilliseconds.ToString(), response.success.ToString());
            }
            throw new Exception("Did not recieve Response.");
        }
    }
}
