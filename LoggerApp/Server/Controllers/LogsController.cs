using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoggerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    { 
        [HttpPost("Information")]
        public async Task LogInformation([FromBody] string content)
        {
            ArgumentNullException.ThrowIfNull(content);

            await Log.Information(content);
        }
        [HttpPost("Debug")]
        public async Task LogDebug([FromBody] string content)
        {
            ArgumentNullException.ThrowIfNull(content);

            await Log.Debug(content);
        }
        [HttpPost("Error")]
        public async Task LogError([FromBody] string content)
        {
            ArgumentNullException.ThrowIfNull(content);

            await Log.Error(content);
        }
        [HttpPost("Critical")]
        public async Task LogCritical([FromBody] string content)
        {
            ArgumentNullException.ThrowIfNull(content);

            await Log.Critical(content);
        }
        
        // GET api/<LoggerController>/5
        [HttpGet("{logType}")]
        public string ReadLogs(LogType logType)
        {
            return string.Empty;
        }

        // PUT api/<LoggerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoggerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
