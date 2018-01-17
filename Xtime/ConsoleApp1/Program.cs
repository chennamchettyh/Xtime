using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Domain;
using ConsoleApp1.Helper;
using Newtonsoft.Json;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var dealerVehicleResponse = new List<Dealer>();

            Console.WriteLine("Program started");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var dataSetId = ApiHelper.GetDataSetId().Result;
            // get the Vehicles for this Dataset
            var vehicleIdList = ApiHelper.GetVehicleIdList(dataSetId);
            if (vehicleIdList != null)
            {
                foreach (var v in vehicleIdList.Result)
                {
                    var vid = (int)v;
                    // get the vehicle information based on id 
                    var vehicleInfo = ApiHelper.GetVehicleInfo(dataSetId, vid).Result;
                    var dealerId = vehicleInfo.dealerId;
                    var dealerResponse = ApiHelper.GetDealerInfo(dataSetId, dealerId);

                    var item = dealerVehicleResponse.FirstOrDefault(x => x.dealerId == dealerId);
                    if (dealerVehicleResponse.Contains(item)) //dealer node exists
                    {
                        item.vehicles.Add(new VehicleViewModel { vehicleId = vid, make = vehicleInfo.make, model = vehicleInfo.model, year = vehicleInfo.year });
                    }
                    else // add a new dealer and a vehicle
                    {
                        dealerVehicleResponse.Add(new Dealer { dealerId = dealerId, name = dealerResponse.Result.name, vehicles = new List<VehicleViewModel> { new VehicleViewModel { vehicleId = vid, make = vehicleInfo.make, model = vehicleInfo.model, year = vehicleInfo.year } } });
                    }
                }
                //post the answer to endpoint
                try
                {
                    var answerResult = ApiHelper.PostAnswer(dataSetId, dealerVehicleResponse).Result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                stopWatch.Stop();
                Console.WriteLine(JsonConvert.SerializeObject(dealerVehicleResponse));
            }
            Console.WriteLine("Total time required: {0} seconds", stopWatch.Elapsed);
            Console.ReadLine();
        }

       
    }


}
