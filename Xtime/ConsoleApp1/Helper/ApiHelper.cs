using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Domain;
using Newtonsoft.Json;

namespace ConsoleApp1.Helper
{
    public static class ApiHelper
    {

        public async static Task<string> GetDataSetId()
        {
            var apiPath = "api/datasetid";
            var response = await GenericApiHelper<DataSet>.GetResult(apiPath);
            return response.DatasetId;
        }

        public async static Task<IList> GetVehicleIdList(string datasetid)
        {
            var apiPath = String.Format("api/{0}/vehicles", datasetid);
            var response = await GenericApiHelper<Vehicles>.GetResult(apiPath);
            return response.vehicleIds.ToList();
        }
        public async static Task<Vehicle> GetVehicleInfo(string datasetid, int vehicleid)
        {
            var apiPath = String.Format("api/{0}/vehicles/{1}", datasetid, vehicleid);
            var response = await GenericApiHelper<Vehicle>.GetResult(apiPath);
            return response;
        }
        public async static Task<Dealer> GetDealerInfo(string datasetid, int dealerid)
        {
            var apiPath = String.Format("api/{0}/dealers/{1}", datasetid, dealerid);
            var response = await GenericApiHelper<Dealer>.GetResult(apiPath);
            return response;
        }
        public async static Task<string> PostAnswer(string datasetId, List<Dealer> dealerVehicleResponse)
        {
            var apiPath = String.Format("api/{0}/answer", datasetId);
            var content = new StringContent(JsonConvert.SerializeObject(dealerVehicleResponse), Encoding.UTF8, "application/json");
            var response = await GenericApiHelper<AnswerResponse>.PostAnswerResponse(apiPath, content);
            if (response != null)
            {
                return String.Format("Answer result Message: {0}/ TotalMillisecons: {1}/ IsSuccess?: {2}", response.message, response.totalMilliseconds.ToString(), response.success.ToString());
            }
            throw new Exception("Answer Response was not received");
        }
    }
}
