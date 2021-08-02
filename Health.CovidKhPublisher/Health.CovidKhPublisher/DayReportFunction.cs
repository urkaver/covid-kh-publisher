using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Health.CovidKhPublisher
{
    public class DayReportFunction
    {
        private readonly IMessageService _messageService;

        public DayReportFunction(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [FunctionName("DayReportFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("uk-UA");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("uk-UA");

            string dateString = await GetDateFromRequest(req);

            log.LogInformation("The function 'DayReportFunction' is triggered.");

            DateTime requestedDate = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(dateString) && !DateTime.TryParseExact(dateString, "yyyy-MM-dd", new DateTimeFormatInfo(), DateTimeStyles.None,
                    out requestedDate))
            {
                log.LogWarning($"Invalid date input: {dateString}");
                return new BadRequestObjectResult("The date should be in the format 'yyyy-MM-dd'.");
            }

            try
            {
                await _messageService.SendDayReportMessageAsync(Location.Kharkiv, requestedDate);
            }
            catch (Exception e)
            {
                log.LogError(e, "The error occured while function execution.");
                throw;
            }
            
            return new OkResult();
        }

        private async Task<string> GetDateFromRequest(HttpRequest request)
        {
            string date = request.Query["date"];

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return date ?? data?.date;
        }
    }
}
