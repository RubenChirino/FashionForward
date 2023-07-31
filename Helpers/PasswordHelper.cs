using System;
using System.Security.Cryptography;
using System.Text;

namespace FashionForward.Controllers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        // Utilizar una función de hashing como SHA256
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Convertir la contraseña en bytes
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Calcular el hash
            byte[] hashBytes = sha256Hash.ComputeHash(bytes);

            // Convertir el hash en una cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Convertir la contraseña ingresada en bytes
            byte[] enteredBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Calcular el hash de la contraseña ingresada
            byte[] enteredHashBytes = sha256Hash.ComputeHash(enteredBytes);

            // Convertir el hash de la contraseña ingresada en una cadena hexadecimal
            StringBuilder enteredBuilder = new StringBuilder();
            for (int i = 0; i < enteredHashBytes.Length; i++)
            {
                enteredBuilder.Append(enteredHashBytes[i].ToString("x2"));
            }

            string enteredHashedPassword = enteredBuilder.ToString();

            // Comparar el hash de la contraseña ingresada con el hash almacenado
            return string.Equals(enteredHashedPassword, storedHashedPassword);
        }
    }
}
