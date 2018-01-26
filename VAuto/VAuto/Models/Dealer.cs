using System.Collections.Generic;

namespace VAuto.Models
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

    
}
