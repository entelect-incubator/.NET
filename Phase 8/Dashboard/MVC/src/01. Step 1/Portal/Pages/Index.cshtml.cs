namespace Portal.Pages;

using System.Reflection;
using API.Client.Template;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
	private readonly CustomerClient customerClient;

	private readonly PizzaClient pizzaClient;

	public List<Models.PizzaViewModel> Orders = new() { };

	public int NoCustomers = 0;
	
	public int NoPizzas = 0;

	public IndexModel(IHttpClientFactory httpClientFactory)
	{
		var httpClient = httpClientFactory.CreateClient();
		this.customerClient = new CustomerClient(AppSettings.ApiUrl, httpClient);
		this.pizzaClient = new PizzaClient(AppSettings.ApiUrl, httpClient);
	}

	public async Task OnGet(CancellationToken cancellationToken = default)
	{
		var customerClientResult = await this.customerClient.SearchAsync(new SearchCustomerModel(), cancellationToken);
		var random = new Random();
		if (customerClientResult.Succeeded)
		{
			this.NoCustomers = customerClientResult.Count;
			foreach (var customer in customerClientResult.Data)
			{
				var orderResult = await this.customerClient.GetOrdersAsync(customer.Id, cancellationToken);
				foreach (var order in this.Orders)
				{
					this.Orders.Add(new Models.PizzaViewModel
					{
						Id = order.Id,
						ImageName = $"{random.Next(5, 10)}.jpeg",
						Name = order.Name,
						Price = order.Price,
					});
				}
			}
		}

	//	var pizzaClientResult = await this.pizzaClient.SearchAsync(new SearchPizzaModel(), cancellationToken);

	}
}
