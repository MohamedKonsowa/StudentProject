// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.VisualBasic;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThirdApp.Server.Controllers
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
        [HttpGet("allLogs")]
        public async Task<IEnumerable<string>> ReadAllLogs() => await Log.ReadAllLogsFiles();
        [HttpGet("GetSortedLogs")]
        public async Task<IEnumerable<string>> GetSortedLogs() => await Log.GetSortedList();
        [HttpGet("GetSpecificLogs")]
        public async Task<IEnumerable<string>> GetSpecificLogs([FromQuery] string date, [FromQuery] string logType = null)
        {
            IEnumerable<string> logs;

            if (string.IsNullOrEmpty(logType)) logs = await Log.GetLogsOfSpecificDate(date);
            else
                logs = await Log.GetLogsOfSpecificDate(date, logType);

            return logs;
        }
        [HttpGet("Information/{id:int}")]
        public async Task<IEnumerable<string>> ReadInformationLogs(int id) => await Log.ReadInformaitonLogs(id);
        [HttpGet("Debug/{id:int}")]
        public async Task<IEnumerable<string>> ReadDebugLogs(int id) => await Log.ReadDebugLogs(id);
        [HttpGet("Error/{id:int}")]
        public async Task<IEnumerable<string>> ReadErrorLogs(int id) => await Log.ReadErrorLogs(id);
        [HttpGet("Critical/{id:int}")]
        public async Task<IEnumerable<string>> ReadCriticalLogs(int id) => await Log.ReadCriticalLogs(id);
        [HttpGet("logsFilesCounter")]
        public async Task<Dictionary<LogType, int>> GetLogsFilesCounter() => await Log.GetLogsFilesCounter();

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
