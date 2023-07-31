using System;
namespace FashionForward.Models;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public string Gallery { get; set; }
    public int Active { get; set; }
    public int Stock { get; set; }
    public enum Categories
    {
        all,
        kids,
        woman,
        man
    }

    public Product(int id, string code, string name, decimal price, string category, string image, string gallery, int active, int stock)
    {
        Id = id;
        Code = code;
        Name = name;
        Price = price;
        Category = category;
        Image = image;
        Gallery = gallery;
        Active = active;
        Stock = stock;
    }

}