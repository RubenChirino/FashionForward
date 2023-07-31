using System;
using System.Diagnostics;
using FashionForward.Helpers;
using FashionForward.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace FashionForward.Controllers;

public class VerifyController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly MySqlConnection _connection;

    public VerifyController(ILogger<HomeController> logger, MySqlConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public IActionResult Index(int token)
    {
        bool verified = UpdateVerifyStatus(token);
        return View(verified);
    }

    [HttpPost]
    public bool UpdateVerifyStatus(int userId)
    {
        try
        {
            string sql = "UPDATE user SET verify = 1 WHERE id = @UserId";
            MySqlCommand command = new MySqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@UserId", userId);
            _connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            _connection.Close();

            return rowsAffected > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

