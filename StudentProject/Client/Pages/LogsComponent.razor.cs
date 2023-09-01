using ThirdApp.Shared;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace ThirdApp.Client;

public partial class LogsComponent
{
    LogType logType = LogType.Error;
    string logTypeAsString = LogType.Error.ToString();
    List<string> logs = new();
    string selectedSortOption = "Type";
    bool chooseSpecificDate = false;
    DateTime selectedDate = DateTime.Now;
    Dictionary<LogType, int> logsFilesCounter = new()
    {
        [LogType.Information] = 0,
        [LogType.Debug] = 0,
        [LogType.Error] = 0,
        [LogType.Critical] = 0
    };
    int selectedFile = -1;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            logsFilesCounter = await _httpClient.GetFromJsonAsync<Dictionary<LogType, int>>("api/logs/logsFilesCounter");
            logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/logs/{logTypeAsString}/{selectedFile}");
            await InvokeAsync(StateHasChanged);
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    private async Task GetLogs()
    {
        logs.Clear();
        if (chooseSpecificDate)
        {
            string formattedDate = selectedDate.ToString("yyyy-MM-dd");
            if (logTypeAsString.Equals("allLogs"))
                logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/logs/GetSpecificLogs?date={formattedDate}");
            else
                logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/logs/GetSpecificLogs?date={formattedDate}&logType={logTypeAsString}");
        }
        else
        {
            if (logTypeAsString.Equals("allLogs"))
            {
                if (selectedSortOption.Equals("Type"))
                    logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/Logs/allLogs");
                else
                    logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/Logs/GetSortedLogs");
            }
            else
                logs = await _httpClient.GetFromJsonAsync<List<string>>($"api/logs/{logTypeAsString}/{selectedFile}");
        }
        await InvokeAsync(StateHasChanged);
    }
}
