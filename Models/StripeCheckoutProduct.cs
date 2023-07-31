using System;
namespace FashionForward.Models;

public class StripeCheckoutProduct
{
	public string Id;
	public string Value;

	public StripeCheckoutProduct(string id, string value)
	{
		Id = id;
		Value = value;
	}
}

