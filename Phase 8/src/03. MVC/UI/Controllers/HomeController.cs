namespace UI.Controllers;

using System.Net.Http;
using API.Client.Template;
using Microsoft.AspNetCore.Mvc;

public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
	private const string SessionKey = "SessionId";

	/// TODO Add proper settings to get the URL
	private readonly TodosClient todoClient = new("https://localhost:44315/", httpClientFactory.CreateClient());

	public async Task<ActionResult<List<TodoModel>>> Index()
	{
		if (string.IsNullOrEmpty(this.HttpContext.Session.GetString(SessionKey)))
		{
			this.HttpContext.Session.SetString(SessionKey, Guid.NewGuid().ToString());
		}

		this.ViewBag.SessionId = this.HttpContext.Session.GetString(SessionKey);
		if (!string.IsNullOrEmpty(this.ViewBag.SessionId))
		{
			var result = await this.todoClient.SearchAsync(new SearchTodoModel()
			{
				SessionId = new Guid(this.ViewBag.SessionId)
			});

			return this.View(result.Data);
		}

		return this.View(new List<TodoModel>());
	}

	[HttpPost]
	public async Task<ActionResult> Add(CreateTodoModel newItem)
	{
		newItem.IsCompleted = false;
		newItem.Task = $"{newItem.Task}";
		if (this.ModelState.IsValid)
		{
			newItem.SessionId = new Guid(this.HttpContext.Session.GetString(SessionKey));
			var result = await this.todoClient.AddAsync(newItem);
			if(result.Succeeded.HasValue)
			{
				if (!result.Succeeded.Value)
				{
					return this.RedirectToAction("Error");
				}
			}
		}

		return this.RedirectToAction("Index");
	}

	public async Task<ActionResult> MarkComplete(int id)
	{
		if (this.ModelState.IsValid)
		{
			var result = await this.todoClient.CompleteAsync(id);
			if (result.Succeeded.HasValue)
			{
				if (!result.Succeeded.Value)
				{
					return this.RedirectToAction("Error");
				}
			}
		}

		return this.RedirectToAction("Index");
	}

	public async Task<ActionResult> Delete(int id)
	{
		if (this.ModelState.IsValid)
		{
			var result = await this.todoClient.DeleteAsync(id);
			if (result.Succeeded.HasValue)
			{
				if (!result.Succeeded.Value)
				{
					return this.RedirectToAction("Error");
				}
			}
		}

		return this.RedirectToAction("Index");
	}
}
