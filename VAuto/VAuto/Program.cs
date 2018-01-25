using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Domain;
using VAuto.Helper;


namespace VAuto
{
    /// <summary>
    /// VAuto Interview Coding Question.
    /// </summary>
    class Program
    {
        static readonly Object _lockMe = new Object();
        private static List<Dealer>  _dealerVehicles = new List<Dealer>();

        /// <summary>
        /// Main program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Here we go. Wish me luck!");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            //Get DataSetId
            var dataSetId = ApiHelper.GetDataSetId().Result;
           
            // Get the Vehicles for this Dataset
            var vehicleIdList = ApiHelper.GetVehicleIdList(dataSetId).Result;
            if (vehicleIdList != null)
            {
                //Fire the threads and run parallel.
                Parallel.ForEach(vehicleIdList, vehicle =>
                    {
                        GetDealerVehiclesInfo(vehicle, dataSetId, _dealerVehicles);
                    });

              
                //Send the Answer
                try
                {
                    var dealers = new Dealers {dealers = _dealerVehicles};
                    var answerResult = ApiHelper.PostAnswer(dataSetId, dealers).Result;
                    Console.WriteLine(answerResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //Stop the timer.
                stopWatch.Stop();
                Console.WriteLine(JsonConvert.SerializeObject(_dealerVehicles));
            }
            Console.WriteLine("Total time Taken: {0} seconds.", stopWatch.Elapsed);
            Console.ReadLine();
        }

        /// <summary>
        /// Used to get the list of Vehicles and dealers Info based on the dataSetId.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="dataSetId"></param>
        /// <param name="dealerVehicle"></param>
        private static void GetDealerVehiclesInfo(int vehicle, string dataSetId, List<Dealer> dealerVehicle)
        {
            
            // get the vehicle information based on id 
            var vehicleInfo = ApiHelper.GetVehicleInfo(dataSetId, vehicle).Result;
            var dealerId = vehicleInfo.dealerId;
            var dealerResponse = ApiHelper.GetDealerInfo(dataSetId, dealerId);

            lock(_lockMe)
            {
                var dealer = dealerVehicle.FirstOrDefault(x => x.dealerId == dealerId);
                if (dealerVehicle.Contains(dealer)) //dealer node exists
                {
                    dealer.vehicles.Add(new VehicleViewModel
                    {
                        vehicleId = vehicle,
                        make = vehicleInfo.make,
                        model = vehicleInfo.model,
                        year = vehicleInfo.year
                    });
                }
                else // add a new dealer and a vehicle
                {
                    dealerVehicle.Add(new Dealer
                    {
                        dealerId = dealerId,
                        name = dealerResponse.Result.name,
                        vehicles =
                            new List<VehicleViewModel>
                            {
                                new VehicleViewModel
                                {
                                    vehicleId = vehicle,
                                    make = vehicleInfo.make,
                                    model = vehicleInfo.model,
                                    year = vehicleInfo.year
                                }
                            }
                    });
                }
            }
        }
    }


}
