using System.Globalization;
using System.IO;
using System.Text;

namespace ThirdApp.Shared;

public static class Log
{
    public static DateTime Date { get; set; }
    public static LogType Type { get; set; }
    public static string? Data { get; set; }

    static string allocationPath = "App_Data\\Logs";
    static string lineSeperator = "===========================================================================";
    static int maxFileSize = 10 * 1024;
    static Dictionary<LogType, int> logsFilesCounter = new()
    {
        [LogType.Information] = 0,
        [LogType.Debug] = 0,
        [LogType.Error] = 0,
        [LogType.Critical] = 0
    };
    static private async Task SaveCounterToFile()
    {
        string filePath = $"{allocationPath}\\counterData.txt";
        List<string> logsFilesCounterSavedData = new();
        foreach (var counter in logsFilesCounter)
        {
            logsFilesCounterSavedData.Add($"{counter.Key}={counter.Value}");
        }
        await File.WriteAllLinesAsync(filePath, logsFilesCounterSavedData);
    }
    static private async Task ReadFromCounterFile()
    {
        string filePath = $"{allocationPath}\\counterData.txt";
        if (!File.Exists(filePath))
        {
            FileNotFoundException fileNotFoundException = new FileNotFoundException($"File not found{filePath}");
            await Log.Error(fileNotFoundException.Message);
            return;
        }
        string[] logsFilesCounterSavedDataAslines = await File.ReadAllLinesAsync(filePath);

        foreach (string line in logsFilesCounterSavedDataAslines)
        {
            string[] keyAndValue = line.Split("=");
            Enum.TryParse(keyAndValue[0], out LogType key);
            int.TryParse(keyAndValue[1], out int value); ;
            logsFilesCounter[key] = value;
        }
    }

    public static async Task Information(string data) => await WriteLog(LogType.Information, data);
    public static async Task Debug(string data) => await WriteLog(LogType.Debug, data);
    public static async Task Error(string data) => await WriteLog(LogType.Error, data);
    public static async Task Critical(string data) => await WriteLog(LogType.Critical, data);
    private static async Task WriteLog(LogType logType, string data)
    {
        if (!Directory.Exists(allocationPath)) Directory.CreateDirectory(allocationPath);

        string filePath = $"{allocationPath}\\{logType.ToString()}logs#{logsFilesCounter[logType]}.txt";
        if (File.Exists(filePath))
        {
            long fileSize = new FileInfo(filePath).Length;
            if (fileSize >= maxFileSize)
            {
                logsFilesCounter[logType]++;
                filePath = $"{allocationPath}\\{logType.ToString()}logs#{logsFilesCounter[logType]}.txt";
                await SaveCounterToFile();
            }
        }
        string content = $"Log date: {DateTime.Now.ToString()}{Environment.NewLine}[{logType.ToString()}]Log:{data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";
        await File.AppendAllTextAsync(filePath, content);
    }

    private static async Task<IEnumerable<string>> ReadLogs(LogType logType, int fileId = -1)
    {
        string filePath = $"{allocationPath}\\{logType.ToString()}logs#{logsFilesCounter[logType]}.txt";
        if (!File.Exists(filePath))
        {
            FileNotFoundException fileNotFoundException = new($"File not found: {filePath}");
            await Log.Error(fileNotFoundException.Message);
            return Enumerable.Empty<string>();
        }
        else
        {
            if (!fileId.Equals(-1)) filePath = $"{allocationPath}\\{logType.ToString()}logs#{fileId}.txt";
            else
            {
                string fielContent = string.Empty;
                string[] allFileLogs;
                List<string> allLogs = new();
                for (int i = 0; i <= logsFilesCounter[logType]; i++)
                {
                    filePath = $"{allocationPath}\\{logType.ToString()}logs#{i}.txt";
                    fielContent = await File.ReadAllTextAsync(filePath);
                    if (string.IsNullOrEmpty(fielContent))
                        continue;
                    else
                    {
                        allFileLogs = fielContent.Trim().Split(lineSeperator, StringSplitOptions.RemoveEmptyEntries);
                        allLogs.AddRange(allFileLogs);
                    }
                }
                return allLogs;
            }
            string content = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrEmpty(content)) return Enumerable.Empty<string>();

            string[] logs = content.Trim().Split(lineSeperator, StringSplitOptions.RemoveEmptyEntries);
            return logs;
        }
    }
    public static async Task<IEnumerable<string>> ReadInformaitonLogs(int fileId = -1) => (await ReadLogs(LogType.Information, fileId));
    public static async Task<IEnumerable<string>> ReadDebugLogs(int fileId = -1) => (await ReadLogs(LogType.Debug, fileId));
    public static async Task<IEnumerable<string>> ReadErrorLogs(int fileId = -1) => (await ReadLogs(LogType.Error, fileId));
    public static async Task<IEnumerable<string>> ReadCriticalLogs(int fileId = -1) => (await ReadLogs(LogType.Critical, fileId));
    public static async Task<List<string>> ReadAllLogsFiles()
    {
        List<string> allLogs = new List<string>();
        IEnumerable<string> tempData = await ReadInformaitonLogs();
        allLogs.AddRange(tempData.ToList() ?? new List<string>());
        tempData = await ReadDebugLogs();
        allLogs.AddRange(tempData.ToList() ?? new List<string>());
        tempData = await ReadErrorLogs();
        allLogs.AddRange(tempData.ToList() ?? new List<string>());
        tempData = await ReadCriticalLogs();
        allLogs.AddRange(tempData.ToList() ?? new List<string>());
        return allLogs;
    }
    private static async Task<List<LogHelper>> GetSortedLogsAsListOfLogHelper()
    {
        List<string> lines = await ReadAllLogsFiles();
        if(lines.Count.Equals(0)) return new List<LogHelper>();
        List<LogHelper> sortedLogs = new();
        int dateIndex;
        int typeStartIndex;
        int typeEndIndex;
        foreach (var line in lines)
        {
            dateIndex = line.IndexOf("Log date: ") + "Log date: ".Length;
            typeStartIndex = line.IndexOf("[");
            typeEndIndex = line.IndexOf("]", typeStartIndex);
            sortedLogs.Add(new LogHelper
            {
                Date = DateTime.ParseExact(line.Substring(dateIndex, typeStartIndex - dateIndex).Trim(), "M/d/yyyy h:mm:ss tt", null),
                Type = (LogType)Enum.Parse(typeof(LogType), line.Substring(typeStartIndex + 1, typeEndIndex - typeStartIndex - 1)),
                Data = line.Substring(line.IndexOf("Log:") + "Log:".Length).Trim()
            });
        }
        sortedLogs = sortedLogs.OrderBy(log => log.Date).ToList();
        return sortedLogs;
    }
    private static async Task<List<LogHelper>> GetSortedLogsAsListOfLogHelper(LogType logType)
    {
        List<string> lines = (List<string>)await ReadLogs(logType);
        if(lines.Count.Equals(0)) return new List<LogHelper>();
        List<LogHelper> sortedLogs = new();
        int dateIndex;
        int typeStartIndex;
        int typeEndIndex;
        foreach (var line in lines)
        {
            dateIndex = line.IndexOf("Log date: ") + "Log date: ".Length;
            typeStartIndex = line.IndexOf("[");
            typeEndIndex = line.IndexOf("]", typeStartIndex);
            sortedLogs.Add(new LogHelper
            {
                Date = DateTime.ParseExact(line.Substring(dateIndex, typeStartIndex - dateIndex).Trim(), "M/d/yyyy h:mm:ss tt", null),
                Type = (LogType)Enum.Parse(typeof(LogType), line.Substring(typeStartIndex + 1, typeEndIndex - typeStartIndex - 1)),
                Data = line.Substring(line.IndexOf("Log:") + "Log:".Length).Trim()
            });
        }
        sortedLogs = sortedLogs.OrderBy(log => log.Date).ToList();
        return sortedLogs;
    }
    public static async Task<List<string>> GetSortedList()
    {
        var sortedLogs = await GetSortedLogsAsListOfLogHelper();
        ArgumentNullException.ThrowIfNull(sortedLogs);

        List<string> sortedLogsAsString = new();
        foreach (var log in sortedLogs)
            sortedLogsAsString.Add(log.ToString());
        return sortedLogsAsString;
    }
    public static async Task<List<string>> GetLogsOfSpecificDate(string dateAsString)
    {
        if (DateTime.TryParseExact(dateAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            List<LogHelper>? sortedLogs = await GetSortedLogsAsListOfLogHelper();
            ArgumentNullException.ThrowIfNull(sortedLogs);

            if (!sortedLogs.Count.Equals(0))
            {
                List<LogHelper> logsOnTargetDate = sortedLogs.FindAll(log => log.Date.Date.Equals(date.Date));
                List<string> sortedLogsAsString = logsOnTargetDate.Select(log => log.ToString()).ToList();
                return sortedLogsAsString;
            }
            return new List<string>();
        }
        return new List<string>();
    }
    public static async Task<List<string>> GetLogsOfSpecificDate(string dateAsString, string logTypeAsString)
    {
        if (DateTime.TryParseExact(dateAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            LogType logType = (LogType)Enum.Parse(typeof(LogType), logTypeAsString);
            List<LogHelper>? sortedLogs = await GetSortedLogsAsListOfLogHelper(logType);
            ArgumentNullException.ThrowIfNull(sortedLogs);

            if (!sortedLogs.Count.Equals(0))
            {
                List<LogHelper> logsOnTargetDate = sortedLogs.FindAll(log => log.Date.Date.Equals(date.Date));
                List<string> sortedLogsAsString = logsOnTargetDate.Select(log => log.ToString()).ToList();
                return sortedLogsAsString;
            }
            return new List<string>();
        }
        return new List<string>();
    }

    public static async Task<Dictionary<LogType, int>> GetLogsFilesCounter()
    {
        await ReadFromCounterFile();
        return logsFilesCounter;
    }
}
