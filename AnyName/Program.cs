using System.Security.Cryptography.X509Certificates;

namespace AnyName
{
	internal class Program
	{
		public delegate bool IsGreaterThanMySalary(Employee emp);
		static void GreaterThanMySalary(List<Employee> employees, IsGreaterThanMySalary greaterThan)
		{

			foreach (Employee emp in employees)
			{
				if (greaterThan(emp))
					Console.WriteLine($"Name : {emp.Name} \n " +
						$"Id : {emp.Id}" +
						$"\n Salary : {emp.Salary}" +
						$"\n -----------------------------------------------");


			}
		}
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");



			List<Employee> employees = new List<Employee>
			{
				new Employee (1,"adel",2000),
				new Employee (2,"hamo",3000),
				new Employee (3,"disha",4000),
				new Employee (4,"fisha",5000),
				new Employee (5,"sika",6000),
				new Employee (6,"nika",7000),
				new Employee (7,"dick",8000),
				new Employee (8,"boob",9000),
				new Employee (9,"screw",10000),
				new Employee (10,"you",110000)

			};

			GreaterThanMySalary(employees, e => e.Name == "disha");


		}
		
	}
	
}