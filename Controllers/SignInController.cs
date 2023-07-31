using System;
using FashionForward.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace FashionForward.Controllers;

public class SignInController : Controller
{

    private readonly ILogger<HomeController> _logger;

    private readonly MySqlConnection _connection;

    public SignInController(ILogger<HomeController> logger, MySqlConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}