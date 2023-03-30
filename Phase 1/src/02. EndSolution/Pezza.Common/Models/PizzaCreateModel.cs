namespace Pezza.Common.Models;

public class PizzaCreateModel
{
	public PizzaCreateModel(string name, string description, string pictureUrl, decimal price)
		=> (this.Name, this.Description, this.PictureUrl, this.Price) = (name, description, pictureUrl, price);

	public string Name { get; set; }

	public string Description { get; set; }

	public string PictureUrl { get; set; }

	public decimal Price { get; set; }
}
