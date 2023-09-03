using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ThirdApp.Shared;
using static System.Net.WebRequestMethods;

namespace ThirdApp.Client;

public partial class Students
{
    public List<Student>? students;
    public Student student = new();
    bool isLoaded;
    bool isSaved = false;
    string statusClass = string.Empty;
    string message = string.Empty;
    protected async override Task OnInitializedAsync() => await base.OnInitializedAsync();
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
            isLoaded = true;
            await InvokeAsync(StateHasChanged);
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    protected async void OnFormSubmit()
    {
        student.Id = Guid.NewGuid();
        isSaved = true;

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/students", Encryption.EncryptStudent(student));
        if (response.IsSuccessStatusCode)
        {
            students?.Add(student);
            string logContent = $"Student:{student.Name} was added successfully";
            await _logHttpClient.PostAsJsonAsync("api/logs/Information", logContent);
            statusClass = "alert-success";
            message = "Student was added successfully.";
        }
        else
        {
            string logContent = $"Error occured while adding student:{student.Name}{Environment.NewLine}Error:{response.Content}";
            await _logHttpClient.PostAsJsonAsync("api/logs/Error", logContent);
            statusClass = "alert-danger";
            message = "Error occured while adding student, Please try again.";
        }
        //students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
        student = new();
        await InvokeAsync(StateHasChanged);
    }
    public async Task DeleteFromList(Student student)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/students/{student.Id}");
        if (response.IsSuccessStatusCode)
        {
            students.Remove(student);
            string logContent = $"Student:{Encryption.UnShiftCharacters(student.Name)} was deleted successfully";
            await _logHttpClient.PostAsJsonAsync("api/logs/Information", logContent);
            await InvokeAsync(StateHasChanged);
        }
        else
        {
            string logContent = $"Error occured while deleting student:{Encryption.UnShiftCharacters(student.Name)}{Environment.NewLine}Error:{response.Content}";
            await _logHttpClient.PostAsJsonAsync("api/logs/Error", logContent);
        }
    }
}
