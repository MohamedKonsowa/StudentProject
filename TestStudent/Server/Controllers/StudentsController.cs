using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using TestStudent.Shared;

namespace TestStudent.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		string directoryAllocationPath = "App_Data\\Students";
		List<Student> students = new();

		public StudentsController() => ReadDataFromFile();

		[HttpPost]
		public void Post([FromBody] Student value)
		{
			students.Add(value);
			FlushDataToFile();
		}
		private void FlushDataToFile()
		{
			if (!Directory.Exists(directoryAllocationPath))
			{
				Directory.CreateDirectory(directoryAllocationPath);
			}

			System.IO.File.WriteAllText($"{directoryAllocationPath}\\students.json", JsonSerializer.Serialize(students));

		}


		[HttpGet]
		public IEnumerable<Student> Get()
		{
			ReadDataFromFile();
			return students;
		}
		private void ReadDataFromFile()
		{
			if (System.IO.File.Exists($"{directoryAllocationPath}\\students.Json"))
			{
				string? fileContent = System.IO.File.ReadAllText($"{directoryAllocationPath}\\students.json");
				if (string.IsNullOrEmpty(fileContent))
					return;
				students = JsonSerializer.Deserialize<List<Student?>>(fileContent);
			}
			else
				students.Add(new Student());
			
		}

		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		






		// PUT api/<StudentsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<StudentsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
		~StudentsController()
		{
			FlushDataToFile();

		}

	}
}
