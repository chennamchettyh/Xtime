using System.Collections.Generic;

namespace VAuto.Domain
{
    public class Dealer
    {
        public int dealerId { get; set; }
        public string name { get; set; }
        public List<VehicleViewModel> vehicles { get; set; }

        public Dealer()
        {
            vehicles = new List<VehicleViewModel>();
        }
    }

    public class Dealers
    {
       public List<Dealer> dealers { get; set; }
    }
}
