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
        Student tempStudent =  Encryption.EncryptStudent(student);

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/students", tempStudent);
        if(response.IsSuccessStatusCode)
        {
            students?.Add(tempStudent);
            string logContent = $"Student:{Encryption.UnShiftCharacters(student.Name)} was added successfully";
            await _logHttpClient.PostAsJsonAsync("api/logs/Information", logContent);
            student = new();
        }
        else
        {
            string logContent = $"Error occured while adding student:{student.Name}{Environment.NewLine}Error:{response.Content}";
            await _logHttpClient.PostAsJsonAsync("api/logs/Error", logContent);
        }
        //students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
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
