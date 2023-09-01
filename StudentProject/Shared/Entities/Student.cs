using System.ComponentModel.DataAnnotations;

namespace ThirdApp;
public class Student
{
    public Guid Id { get; set; }

    [MinLength(3, ErrorMessage = "Name must be 3 digits at least")]
    [RegularExpression(@"^[\p{L}\s]*$", ErrorMessage = "Name must contain only charachters.")]
    public string? Name { get; set; }

    [Range(4, 1000, ErrorMessage = "Age must be bigger than 3")]
    public int Age { get; set; }

    [StringLength(11, ErrorMessage = "Phone number  must be 11 digit only")]
    [MinLength(11, ErrorMessage = "Phone number  must be 11 digit")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must contain only numbers.")]
    public string? Mobile { get; set; }
}
