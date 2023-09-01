using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace ThirdApp.Shared;

public static class RequestTypes
{
    public static async Task<HttpResponseMessage> NormalPostRequest(HttpClient _httpClient, string url, Student student)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync<Student>(url, student);
        return response;
    }
    public static async Task<HttpResponseMessage> TimedGetRequest(HttpClient _httpClient, string url, int secondsToWait)
    {
        CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(secondsToWait));
        HttpResponseMessage? response = await _httpClient.GetAsync(url, cancellationTokenSource.Token);
        return response;
    }
}
// Promise request => Not ok !
//string message = string.Empty;
//var requestInit = new
//{
//    method = "POST",
//    headers = new { Content_Type = "application/json" },
//    body = JsonSerializer.Serialize(tempStudent)
//};

//var response = await js.InvokeAsync<string>("fetch", "api/students", requestInit);


//HttpResponseMessage response = await RequestTypes.TimedGetRequest(_httpClient, "api/students", 10);
//            if (response.IsSuccessStatusCode)
//            {
//                string content = await response.Content.ReadAsStringAsync();
//students = JsonSerializer.Deserialize<List<Student>>(content);
//            }
