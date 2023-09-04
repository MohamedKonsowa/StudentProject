using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static async Task<byte[]> FileEncrypt(string filePath, string publicKeyXml)
        {
            string fileData = string.Empty;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                fileData = await streamReader.ReadToEndAsync();
                using (RSA rsa = RSA.Create())
                {
                    rsa.FromXmlString(publicKeyXml);

                    byte[] fileDataAsBytes = Encoding.UTF8.GetBytes(fileData);
                    byte[] encryptedData = rsa.Encrypt(fileDataAsBytes, RSAEncryptionPadding.OaepSHA256);

                    return encryptedData;
                }
            }
        }
        public static string FileDecrypt(byte[] encryptedDataAsBytes, string privateKeyXml)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKeyXml);

                byte[] decryptedDataAsBytes = rsa.Decrypt(encryptedDataAsBytes, RSAEncryptionPadding.OaepSHA256);
                string decryptedData = Encoding.UTF8.GetString(decryptedDataAsBytes);

                return decryptedData;
            }
        }
        public static void CreatePublicAndPrivateKeys(string filePath)
        {
            using (RSA rsa = RSA.Create())
            {
                string PublicKeyXml = rsa.ToXmlString(false);
                string privateKeyXml = rsa.ToXmlString(true);
                string publicKeyXmlFilePath = $"{filePath}\\publicKey.xml";
                string privateKeyXmlFilePath = $"{filePath}\\privateKey.xml";

                File.WriteAllText(publicKeyXmlFilePath, PublicKeyXml);
                File.WriteAllText(privateKeyXmlFilePath, privateKeyXml);
            }
        }
    }
}
