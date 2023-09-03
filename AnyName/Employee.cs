namespace AnyName
{
	public class Employee
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Salary { get; set; }


		public Employee(int employeeId, string name, int salary)
		{
			Id = employeeId;
			Name = name;
			Salary = salary;
		}
	}
}