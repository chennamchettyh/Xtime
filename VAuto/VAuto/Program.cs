using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Models;
using VAuto.Services;


namespace VAuto
{
    /// <summary>
    /// VAuto Interview Coding Question.
    /// </summary>
    class Program
    {

        
        /// <summary>
        /// Main program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Here we go. Wish me luck!");
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                VAutoService vAutoService = new VAutoService();
                vAutoService.ConstructResponse();
                Console.WriteLine(JsonConvert.SerializeObject(vAutoService.DealerVehicles));
                //Stop the timer.
                stopWatch.Stop();
                Console.WriteLine("Total time Taken: {0} seconds.", stopWatch.Elapsed);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something Went wrong : {0}", e.Message);
                throw;
            }
        }

       
    }


}
