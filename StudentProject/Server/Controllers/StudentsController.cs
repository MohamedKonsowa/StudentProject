namespace ThirdApp.Server;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    string allocationPath = "App_Data\\Students";
    List<Student> students = new();
    List<Student> encryptedStudntsList = new();

    public StudentsController() => ReadStudentsFromFile();
    private void ReadStudentsFromFile()
    {
        if (System.IO.File.Exists($"{allocationPath}\\students.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{allocationPath}\\students.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            students = JsonSerializer.Deserialize<List<Student>>(fileContent);
        }
    }
    ~StudentsController()
    {
        FlushStudentsToFile();
    }

    [HttpPost]
    public void CreateStudent([FromBody] Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        Student tempStudent = Encryption.DecryptStudent(student);
        students.Add(tempStudent);
        FlushStudentsToFile();
    }
    private void FlushStudentsToFile()
    {
        if (!Directory.Exists(allocationPath)) Directory.CreateDirectory(allocationPath);

        System.IO.File.WriteAllText($"{allocationPath}\\students.json", JsonSerializer.Serialize(students));
    }
    [HttpGet]
    public IEnumerable<Student> ReadAllStudents()
    {
        // Thread.Sleep(5000);
        Student tempStudent = new();
        foreach (Student student in students) encryptedStudntsList.Add(Encryption.EncryptStudent(student));

        return encryptedStudntsList;
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
            Student studentToDelete = students.FirstOrDefault(student => student.Id == id);
            if (studentToDelete == null) return NotFound();

            students.Remove(studentToDelete);
            FlushStudentsToFile();
            return NoContent();
        }
        catch (Exception ex)
        {
            await Log.Error(ex.Message);
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
