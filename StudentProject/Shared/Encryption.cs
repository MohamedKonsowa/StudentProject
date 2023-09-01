using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ThirdApp.Shared
{
    public static class Encryption
    {
        public static string ShiftCharacters(string data)
        {
            ArgumentNullException.ThrowIfNull(data);

            char[] charArray = data.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                if (charArray[i].Equals('z')) charArray[i] = 'a';
                else if (charArray[i].Equals('Z')) charArray[i] = 'A';
                else if (charArray[i].Equals(' ')) continue;
                else
                    charArray[i]++;
            }
            return new string(charArray);
        }
        public static string UnShiftCharacters(string data)
        {
            ArgumentNullException.ThrowIfNull(data);

            char[] charArray = data.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                if (charArray[i].Equals('a')) charArray[i] = 'z';
                else if (charArray[i].Equals('A')) charArray[i] = 'Z';
                else if (charArray[i].Equals(' ')) continue;
                else
                    charArray[i]--;
            }
            return new string(charArray);
        }
        public static int EncryptInt(int number) => number + 707;
        public static int DecryptInt(int number) => number - 707;
        public static string EncryptMobileNumber(string mobileNumber)
        {
            char[] charArray = mobileNumber.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string DecryptMobileNumber(string inversedMobileNumber)
        {
            char[] charArray = inversedMobileNumber.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static Student EncryptStudent(Student student)
        {
            student.Name = ShiftCharacters(student.Name);
            student.Age = EncryptInt(student.Age);
            student.Mobile = EncryptMobileNumber(student.Mobile);
            return student;
        }
        public static Student DecryptStudent(Student student)
        {
            student.Name = UnShiftCharacters(student.Name);
            student.Age = DecryptInt(student.Age);
            student.Mobile = DecryptMobileNumber(student.Mobile);
            return student;
        }
    }
}
