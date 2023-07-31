using System;
using System.Diagnostics;
using FashionForward.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace FashionForward.Controllers;

public class SuccessController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly MySqlConnection _connection;

    public SuccessController(ILogger<HomeController> logger, MySqlConnection connection)
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