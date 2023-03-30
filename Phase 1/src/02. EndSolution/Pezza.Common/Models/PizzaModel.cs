namespace Pezza.Common.Models;

public class PizzaModel
{
	public PizzaModel(string name, string description, string url, decimal price)
		=> (this.Name, this.Description, this.PictureUrl, this.Price) = (name, description, url, price);

	public int Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string PictureUrl { get; set; }

	public decimal Price { get; set; }

	public DateTime DateCreated { get; set; }
}
