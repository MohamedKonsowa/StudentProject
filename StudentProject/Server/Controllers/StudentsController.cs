using System.Text;

namespace ThirdApp.Server;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    string allocationPath = "App_Data\\Students";
    List<Student> students = new();
    int filesCounter = 1;
    int maxFileSize = 1024;

    public StudentsController() => ReadFilesCounters();

    private async Task SaveFilesCounter()
    {
        string filePath = $"{allocationPath}\\FilesCounter.txt";
        object fileLock = new object();
        lock (fileLock)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteAsync(filesCounter.ToString());
            }
        }
    }
    private async Task ReadFilesCounters()
    {
        string filePath = $"{allocationPath}\\FilesCounter.txt";
        if (System.IO.File.Exists(filePath))
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string number = await streamReader.ReadToEndAsync();
                filesCounter = int.Parse(number);
            }
        }
    }

    [HttpPost]
    public async Task AppendStudentToFile(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (!Directory.Exists(allocationPath)) Directory.CreateDirectory(allocationPath);

        string filePath = $"{allocationPath}\\Students#{filesCounter}.json";
        long fileSize = 0;
        if (System.IO.File.Exists(filePath))
            fileSize = new FileInfo(filePath).Length;
        if (fileSize >= maxFileSize)
        {
            filesCounter++;
            await SaveFilesCounter();
            filePath = $"{allocationPath}\\Students#{filesCounter}.json";
            fileSize = 0;
        }
        StringBuilder studentAsJson = new StringBuilder(JsonSerializer.Serialize(student));
        if (fileSize.Equals(0)) studentAsJson.Insert(0, '[');
        studentAsJson.Append(',');

        object fileLock = new object();
        lock (fileLock)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteAsync(studentAsJson);
            };
        }
    }

    private async Task<string> ReadStudentsFromSpecificFile(int fileId)
    {
        string filePath = $"{allocationPath}\\Students#{fileId}.json";
        if (!System.IO.File.Exists(filePath)) return string.Empty;

        StringBuilder fileData = new StringBuilder();
        using (StreamReader streamReader = new StreamReader(filePath))
        {
            fileData = new(await streamReader.ReadToEndAsync());
        }
        //streamReader.Close();
        if (fileData.Length > 0) fileData[fileData.Length - 1] = ']';

        return fileData.ToString();
    }
    [HttpGet]
    public async Task<List<Student>> ReadAllStudents()
    {
        StringBuilder allStudentsData = new StringBuilder();
        for (int fileCount = 0; fileCount <= filesCounter; fileCount++)
        {
            if (!fileCount.Equals(0))
            {
                StringBuilder fileData = new StringBuilder(await ReadStudentsFromSpecificFile(fileCount));
                if (!fileData.Length.Equals(0) && !allStudentsData.Length.Equals(0)) fileData.Remove(0, 1);
                allStudentsData[allStudentsData.Length - 1] = ',';
                allStudentsData.Append(fileData);
            }
            else
                allStudentsData = new(await ReadStudentsFromSpecificFile(fileCount));
        }
        if (!allStudentsData.Length.Equals(0))
        {
            List<Student> students = await JsonSerializer.DeserializeAsync<List<Student>>(new MemoryStream(Encoding.UTF8.GetBytes(allStudentsData.ToString())));
            return students;
        }
        return new List<Student>();
    }

    // GET api/<StudentsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Student value)
    {
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            for (int i = 0; i <= filesCounter; i++)
            {
                StringBuilder fileData = new(await ReadStudentsFromSpecificFile(i));
                if (fileData.Length.Equals(0)) continue;

                List<Student> studentsOfSpecificFile = await JsonSerializer.DeserializeAsync<List<Student>>(new MemoryStream(Encoding.UTF8.GetBytes(fileData.ToString())));
                Student studentToDelete = studentsOfSpecificFile.FirstOrDefault(student => student.Id == id);
                if (studentToDelete is null) continue;
                else
                {
                    studentsOfSpecificFile.Remove(studentToDelete);
                    string newFileData = string.Empty;
                    if (!studentsOfSpecificFile.Count.Equals(0))
                    {
                        newFileData = JsonSerializer.Serialize(studentsOfSpecificFile);
                        fileData = new(newFileData);
                        fileData[fileData.Length - 1] = ',';
                    }
                    string filePath = $"{allocationPath}\\Students#{i}.json";
                    object fileLock = new object();
                    lock (fileLock)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(filePath))
                        {
                            streamWriter.WriteAsync(newFileData);
                        };
                    }
                    return NoContent();
                }
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            await Log.Error(ex.Message);
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
