using LoggerApp.Shared;
using System.Net.Http.Json;

namespace LoggerApp.Client;

//public partial class LoggerComponent
//{
//    public Log logger = new();
//    List<Log>? informationLogs = new();
//    List<Log>? debugLogs = new();
//    List<Log>? errorLogs = new();
//    List<Log>? criticalLogs = new();
//    LogType? logType;

//    protected async override Task OnInitializedAsync()
//    {

//        informationLogs = await _httpClient.GetFromJsonAsync<List<Log>>("api/logger/Information");
//        debugLogs = await _httpClient.GetFromJsonAsync<List<Log>>("api/logger/Debug");
//        errorLogs = await _httpClient.GetFromJsonAsync<List<Log>>("api/logger/Error");
//        criticalLogs = await _httpClient.GetFromJsonAsync<List<Log>>("api/logger/Critical");

//        await base.OnInitializedAsync();
//    }
//    protected async void OnValid()
//    {
//        logger.LogDateTime = DateTime.Now;
//        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync<Log>($"api/logger", logger);
//        if (response.IsSuccessStatusCode)
//        {
//            if(logger.Type == LogType.Information)
//            {
//                informationLogs?.Add(logger);
//                logger = new Log();
//            }
//            else if(logger.Type == LogType.Debug)
//            {
//                debugLogs?.Add(logger);
//                logger = new Log();
//            }
//            else if (logger.Type == LogType.Error)
//            {
//                errorLogs?.Add(logger);
//                logger = new Log();
//            }
//            else if (logger.Type == LogType.Critical)
//            {
//                criticalLogs?.Add(logger);
//                logger = new Log();
//            }

//        }


//        await InvokeAsync(StateHasChanged);
//    }
//}
