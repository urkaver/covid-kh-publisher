using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Health.CovidKhPublisher.DataSeed
{
    public class Program
    {
        private const string FunctionUrl = "http://localhost:7071/api/DayReportFunction";

        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
 
            var start = new DateTime(2020, 4, 2);
            var end = DateTime.UtcNow.Date;

            Console.WriteLine($"Start for date range [{start:yyyy MM dd}; {end}];");

            foreach (var date in EachDay(start, end))
            {
                A: var response = await httpClient.GetAsync($"{FunctionUrl}?date={date:yyy-MM-dd}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error - retry after 2 sec");
                    await Task.Delay(TimeSpan.FromMilliseconds(3000));
                    goto A;
                }

                Console.WriteLine($"{date}:yyy-MM-dd");
                await Task.Delay(TimeSpan.FromMilliseconds(3000));
            }

            Console.WriteLine("Finish");
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
