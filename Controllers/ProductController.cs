using System;
using System.Diagnostics;
using FashionForward.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace FashionForward.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlConnection _connection;

        public ProductController(MySqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index(string category)
        {
            ProductController productCtrl = new ProductController(_connection);
            List<Product> products = productCtrl.GetActiveProducts(category);
            return View(products);
        }

        public IActionResult Detail(int productId)
        {
            Product product = GetProductById(productId);

            if (product == null)
            {        
                return RedirectToAction("Error", "Home");
            }

            return View(product);
        }

        public List<Product> GetActiveProducts(string category = "all")
        {
            List<Product> activeProducts = new List<Product>();
            try
            {
                string sql = "SELECT * FROM product WHERE active = 1";

                if (category != "all" && category != null)
                {
                    sql += " AND category = @category";
                }

                MySqlCommand command = new MySqlCommand(sql, _connection);

                if (category != "all" && category != null)
                {
                    command.Parameters.AddWithValue("@category", category);
                }

                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product(reader.GetInt32("id"), reader.GetString("code"), reader.GetString("name"), reader.GetDecimal("price"), reader.GetString("category"), reader.GetString("image"), reader.GetString("gallery"), reader.GetInt32("active"), reader.GetInt32("stock"));
                    activeProducts.Add(product);
                }

                _connection.Close();

                return activeProducts;
            }
            catch (Exception)
            {
                return activeProducts;
            }
        }

        public Product GetProductById(int productId)
        {
            try
            {
                string sql = "SELECT * FROM product WHERE id = @ProductId";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Product product = new Product(reader.GetInt32("id"), reader.GetString("code"), reader.GetString("name"), reader.GetDecimal("price"), reader.GetString("category"), reader.GetString("image"), reader.GetString("gallery"), reader.GetInt32("active"), reader.GetInt32("stock"));
                    _connection.Close();
                    return product;
                }

                _connection.Close();
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Product GetProductByCode(string productCode)
        {
            try
            {
                string sql = "SELECT * FROM product WHERE code = @productCode";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@productCode", productCode);
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Product product = new Product(reader.GetInt32("id"), reader.GetString("code"), reader.GetString("name"), reader.GetDecimal("price"), reader.GetString("category"), reader.GetString("image"), reader.GetString("gallery"), reader.GetInt32("active"), reader.GetInt32("stock"));
                    _connection.Close();
                    return product;
                }

                _connection.Close();
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Product> GetActiveProductsByCategory(string category)
        {
            List<Product> products = new List<Product>();
            try
            {
                string sql = "SELECT * FROM product WHERE category = @Category AND active = 1";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Category", category);
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product(reader.GetInt32("id"), reader.GetString("code"), reader.GetString("name"), reader.GetDecimal("price"), reader.GetString("category"), reader.GetString("image"), reader.GetString("gallery"), reader.GetInt32("active"), reader.GetInt32("stock"));
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

        public List<Product> GetActiveProductsByName(string name)
        {
            List<Product> products = new List<Product>();
            try
            {
                string sql = "SELECT * FROM product WHERE name LIKE @Name AND active = 1";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product(reader.GetInt32("id"), reader.GetString("code"), reader.GetString("name"), reader.GetDecimal("price"), reader.GetString("category"), reader.GetString("image"), reader.GetString("gallery"), reader.GetInt32("active"), reader.GetInt32("stock"));
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

        public Boolean ActivateProduct(int productId)
        {
            try
            {
                string sql = "UPDATE product SET active = 1 WHERE ID = @ProductId";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean DeactivateProduct(int productId)
        {
            try
            {
                string sql = "UPDATE product SET active = 0 WHERE ID = @ProductId";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                // Redireccionar a la página de productos o mostrar un mensaje de éxito
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean ReduceStock(string productCode, int quantity)
        {
            try
            {
                string sql = "UPDATE product SET stock = stock - @Quantity WHERE code = @productCode";
                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@productCode", productCode);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetCombinableProductCodesFromDatabase(string productCode)
        {
            List<string> combinableProductCodes = new List<string>();
            try
            {
                string sql = "SELECT product_b_code FROM product_combination WHERE product_a_code = @productCode";

                MySqlCommand command = new MySqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@productCode", productCode);

                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string combinableCode = reader.GetString("product_b_code");
                    combinableProductCodes.Add(combinableCode);
                }

                _connection.Close();

                return combinableProductCodes;
            }
            catch (Exception)
            {
                return combinableProductCodes;
            }
        }

        [HttpGet]
        public JsonResult GetCombinableProductCodes(string productCode)
        {
            List<string> combinableProductCodes = GetCombinableProductCodesFromDatabase(productCode);
            return Json(combinableProductCodes);
        }

        [HttpGet]
        public JsonResult GetProductByCodeEndpoint(string productCode)
        {
            Product combinableProductCodes = GetProductByCode(productCode);
            return Json(combinableProductCodes);
        }

        [HttpGet]
        public JsonResult ReduceStockEndpoint(string productCode, int quantity)
        {
            Boolean res = ReduceStock(productCode, quantity);
            return Json(res);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
