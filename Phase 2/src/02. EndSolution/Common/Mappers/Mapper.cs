namespace Common.Mappers;

using Common.Entities;
using Common.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class Mapper
{
	public static partial PizzaModel Map(this Pizza pizza);

	public static IEnumerable<PizzaModel> Map(this List<Pizza> pizzas)
		=> pizzas.Select(x => x.Map());

	public static partial Pizza Map(this PizzaModel pizza);

	public static IEnumerable<Pizza> Map(this List<PizzaModel> pizzas)
		=> pizzas.Select(x => x.Map());

	public static partial CustomerModel Map(this Customer pizza);

	public static IEnumerable<CustomerModel> Map(this List<Customer> pizzas)
		=> pizzas.Select(x => x.Map());

	public static partial Customer Map(this CustomerModel pizza);

	public static IEnumerable<Customer> Map(this List<CustomerModel> pizzas)
		=> pizzas.Select(x => x.Map());
}
