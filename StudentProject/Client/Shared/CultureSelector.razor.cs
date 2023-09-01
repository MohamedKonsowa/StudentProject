using Microsoft.JSInterop;
using System.Globalization;
namespace ThirdApp.Client.Shared
{
    public partial class CultureSelector
    {
        CultureInfo[] Cultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG")
        };
        CultureInfo culture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                IJSInProcessRuntime JsInProcessRunTime = (IJSInProcessRuntime)_js;
                JsInProcessRunTime.InvokeVoid("SetInLocalStorage", "culture", value.Name);
                Console.WriteLine(culture.Name);
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
            }
        }
    }
    

}
