using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace HangFireApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public void ListaDeInteiros()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Console.WriteLine(i);
            }
        }

        [HttpPost]
        [Route("CriarBackgroundJob")]
        public IActionResult CriarBackgroundJob()
        {

            BackgroundJob.Enqueue(() => ListaDeInteiros());

            return Ok();
        }

        [HttpPost]
        [Route("CriarScheduledJob")]
        public IActionResult CriarScheduledJob()
        {

            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);

            BackgroundJob.Schedule(() => Console.WriteLine("Tarefa agendada"), dateTimeOffSet);  

            return Ok();
        }

        [HttpPost]
        [Route("CriarContinuationJob")]
        public IActionResult CriarContinuationJob()
        {

            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);

            var job1 = BackgroundJob.Schedule(() => Console.WriteLine("Tarefa agendada"), dateTimeOffSet);

            var job2 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Segundo job"));

            var job3 = BackgroundJob.ContinueJobWith(job2, () => Console.WriteLine("Terceiro job"));

            var job4 = BackgroundJob.ContinueJobWith(job3, () => Console.WriteLine("Quarto job"));

            return Ok();
        }

        [HttpPost]
        [Route("CriarRecurruingJob")]
        public IActionResult CriarRecurruingJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Recurring Job"), "* * * * *");
            

            return Ok();
        }

    }
}
