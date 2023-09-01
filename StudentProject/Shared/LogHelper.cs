using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThirdApp.Shared;

public class LogHelper
{
    public  DateTime Date { get; set; }
    public  LogType Type { get; set; }
    public  string? Data { get; set; }
    public override string ToString() => $"Log date: {this.Date.ToString()}{Environment.NewLine}[{this.Type.ToString()}]Log:{this.Data}{Environment.NewLine}{Environment.NewLine}";
}
