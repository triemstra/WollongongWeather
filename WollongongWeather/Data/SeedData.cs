using TempApp.Models;
using System.Linq;

namespace TempApp.Data
{
    public static class SeedData
    {
        public static void Initialize(SampleContext context)
        {
            if (!context.Temperatures.Any())
            {
                Temperature newTemp = context.GetTemperature().Result;

                context.Temperatures.AddRange(newTemp);

                context.SaveChanges();
            }
        }
    }
}