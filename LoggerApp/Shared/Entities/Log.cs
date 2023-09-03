using LoggerApp.Shared;
using System.Text.Json;

namespace LoggerApp.Shared;
public  static class Log
{
    public static DateTime LogDateTime { get; set; }
    public static LogType Type { get; set; }
    public static string? Data { get; set; }

    static string  lineSeperator = "===========================================================================";
    static string allocationPath = "App_Data\\Loggers";

    public static async Task Information(string data) => await WriteLog(LogType.Information, data);
    public static async Task Debug(string data) => await WriteLog(LogType.Debug, data);
    public static async Task Error(string data) => await WriteLog(LogType.Error, data);
    public static async Task Critical(string data) => await WriteLog(LogType.Critical, data);
    private static async Task WriteLog(LogType logType, string data)
    {
        string logPath = $"{allocationPath}\\{logType.ToString()}log.txt";
        string content = $"Log date: {DateTime.Now.ToString()}{System.Environment.NewLine}Log:{data}{System.Environment.NewLine}{lineSeperator}{System.Environment.NewLine}";
        await System.IO.File.AppendAllTextAsync(logPath, content);
    }
//     if (logType == LogType.Information)
//            {
//                if (System.IO.File.Exists(informationLogsFilePath))
//                {
//                    string fileContent = System.IO.File.ReadAllText(informationLogsFilePath);
//                    if (string.IsNullOrEmpty(fileContent)) return new List<Log>();
//                    informationLogs = JsonSerializer.Deserialize<List<Log>>(fileContent);
//                    return informationLogs;
//                }
//                return new List<Log>();
//            }
//            else if (logType == LogType.Debug)
//{
//    if (System.IO.File.Exists(debugLogsFilePath))
//    {
//        string fileContent = System.IO.File.ReadAllText(debugLogsFilePath);
//        if (string.IsNullOrEmpty(fileContent)) return new List<Log>();
//        debugLogs = JsonSerializer.Deserialize<List<Log>>(fileContent);
//        return debugLogs;
//    }
//    return new List<Log>();
//}
//else if (logType == LogType.Error)
//{
//    if (System.IO.File.Exists(errorLogsFilePath))
//    {
//        string fileContent = System.IO.File.ReadAllText(errorLogsFilePath);
//        if (string.IsNullOrEmpty(fileContent)) return new List<Log>();
//        errorLogs = JsonSerializer.Deserialize<List<Log>>(fileContent);
//        return errorLogs;
//    }
//    return new List<Log>();
//}
//else if (logType == LogType.Critical)
//{
//    if (System.IO.File.Exists(criticalLogsFilePath))
//    {
//        string fileContent = System.IO.File.ReadAllText(criticalLogsFilePath);
//        if (string.IsNullOrEmpty(fileContent)) return new List<Log>();
//        criticalLogs = JsonSerializer.Deserialize<List<Log>>(fileContent);
//        return criticalLogs;
//    }
//    return new List<Log>();
//}
//return new List<Log>();
}
