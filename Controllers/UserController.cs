using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FashionForward.Models;
using MySql.Data.MySqlClient;
using FashionForward.Helpers;

namespace FashionForward.Controllers
{
    public class UserController : Controller
    {
        private readonly MySqlConnection _connection;

        private readonly EmailHelper _emailHelper;

        public UserController(MySqlConnection connection, EmailHelper emailHelper)
        {
            _connection = connection;
            _emailHelper = emailHelper;
        }

        [HttpPost]
        public IActionResult AddUser(string email, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    TempData["ErrorMessage"] = "Passwords do not match.";
                    return RedirectToAction("Index", "SignUp");
                }
                string hashedPassword = PasswordHelper.HashPassword(password);
                string sql = "INSERT INTO user (email, password, active, verify) VALUES (@Email, @Password, @Active, @Verify); SELECT LAST_INSERT_ID()";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                //command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.Parameters.AddWithValue("@Active", 1);
                command.Parameters.AddWithValue("@Verify", 0);
                _connection.Open();
                int userId = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
                // Send verification email
                string subject = "Email Verification";
                string verificationLink = "https://localhost:7056/Verify?token=" + userId;
                string message = _emailHelper.GetVerificationEmailHtml(verificationLink);
                try
                {
                    TempData["Mensaje"] = "Verification email sended";
                    _emailHelper.SendEmailAsync(email, subject, message).Wait();
                }
                catch (Exception) {}
                return RedirectToAction("Index", "SignUp");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "SignUp");
            }
        }

        [HttpPost]
        public IActionResult VerifyUser(string email, string password)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(password);
                string sql = "SELECT id, email, verify FROM user WHERE email = @Email AND password = @Password AND active = 1";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                // User login successful
                if (reader.Read())
                {           
                    int userId = reader.GetInt32("id");
                    string userEmail = reader.GetString("email");
                    int userVerify = reader.GetInt32("verify");

                    _connection.Close();

                    if (userVerify == 1)
                    {
                        // User verifycated
                        TempData["UserId"] = userId;
                        TempData["UserEmail"] = userEmail;
                        TempData["userVerify"] = userVerify;

                        return RedirectToAction("Index", "Home");
                    } else
                    {
                        // User not verifycated
                        TempData["ErrorMessage"] = "You need to verificate the user for sign in.";
                        return RedirectToAction("Index", "SignIn");
                    }
                } else
                {
                    // User login failed
                    TempData["ErrorMessage"] = "Incorrect email or password.";
                    return RedirectToAction("Index", "SignIn");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
           try
           {
                string sql = "DELETE FROM user WHERE id = @Id";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            try
            {
                string sql = "UPDATE user SET email = @Email, password = @Password WHERE id = @Id";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Id", user.Id);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
