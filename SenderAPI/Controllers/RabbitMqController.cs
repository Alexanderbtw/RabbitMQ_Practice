using Microsoft.AspNetCore.Mvc;
using SenderAPI.RabbitMQ;

namespace SenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitService;

        public RabbitMqController(IRabbitMqService rabbitMq)
        {
            _rabbitService = rabbitMq;
        }

        [Route("[action]/{message}")]
        [HttpGet]
        public IActionResult SendMessage([FromRoute] string message)
        {
            _rabbitService.SendMessage(message);

            return Ok("Message sended");
        }
    }
}
