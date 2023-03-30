namespace Pezza.Common.Models;

public class PizzaUpdateModel
{
	public PizzaUpdateModel(string name, string description, string url, decimal price)
		=> (this.Name, this.Description, this.PictureUrl, this.Price) = (name, description, url, price);

	public string Name { get; set; }

	public string Description { get; set; }

	public string PictureUrl { get; set; }

	public decimal? Price { get; set; }
}
