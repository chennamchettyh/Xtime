using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAuto.Models;

namespace VAuto.Services
{
    public  class VAutoService
    {
        private  readonly Object _lockMe = new Object();
        private  List<Dealer> _dealerVehicles = new List<Dealer>();


        public List<Dealer> DealerVehicles { get { return _dealerVehicles; } }

        /// <summary>
        /// Construct the Final object that needs to be sent to the API.
        /// </summary>
        public void ConstructResponse()
        {
            //Get DataSetId
            var dataSetId = ApiHandler.GetDataSetId().Result;

            // Get the Vehicles for this Dataset
            var vehicleIdList = ApiHandler.GetVehicleIdList(dataSetId).Result;
            if (vehicleIdList != null)
            {
                //Fire the threads and run parallel.
                Parallel.ForEach(vehicleIdList, vehicle => { GetDealerVehiclesInfo(vehicle, dataSetId, _dealerVehicles); });


                //Send the Answer
                try
                {
                    var dealers = new Dealers { dealers = _dealerVehicles };
                    var answerResult = ApiHandler.PostAnswer(dataSetId, dealers).Result;
                    Console.WriteLine(answerResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Used to get the list of Vehicles and dealers Info based on the dataSetId.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="dataSetId"></param>
        /// <param name="dealerVehicle"></param>
        private  void GetDealerVehiclesInfo(int vehicle, string dataSetId, List<Dealer> dealerVehicle)
        {

            // get the vehicle information based on id 
            var vehicleInfo = ApiHandler.GetVehicleInfo(dataSetId, vehicle).Result;
            var dealerId = vehicleInfo.dealerId;
            var dealerResponse = ApiHandler.GetDealerInfo(dataSetId, dealerId);

            lock (_lockMe)
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
