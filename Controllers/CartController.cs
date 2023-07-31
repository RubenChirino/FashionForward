using System;
using FashionForward.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using FashionForward.Helpers;

namespace FashionForward.Controllers;

public class CartController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly MySqlConnection _connection;

    private readonly EmailHelper _emailHelper;

    public CartController(ILogger<HomeController> logger, MySqlConnection connection, EmailHelper emailHelper)
    {
        _logger = logger;
        _connection = connection;
        _emailHelper = emailHelper;
    }

    public IActionResult Index()
    {
        List<StripeCheckoutProduct> stripeProducts = GetAllStripeCheckoutProducts();
        return View(stripeProducts);
    }

    public List<StripeCheckoutProduct> GetAllStripeCheckoutProducts()
    {
        List<StripeCheckoutProduct> products = new List<StripeCheckoutProduct>();
        try
        {
            string sql = "SELECT * FROM stripe_checkout_product";
            MySqlCommand command = new MySqlCommand(sql, _connection);
            _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                StripeCheckoutProduct product = new StripeCheckoutProduct(reader.GetString("id"), reader.GetString("value"));
                products.Add(product);
            }
            _connection.Close();

            return products;
        }
        catch (Exception)
        {
            return products;
        }
    }

    [HttpPost]
    public Boolean SendPurchaseEmail(string products)
    {
        // Send verification email
        string subject = "Success Purchase";
        string message = "You purchase has been completed successfully";
        try
        {
            _emailHelper.SendEmailAsync("", subject, message).Wait();
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

