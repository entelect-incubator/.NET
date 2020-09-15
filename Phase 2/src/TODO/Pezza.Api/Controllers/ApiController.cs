namespace Pezza.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    namespace CleanArchitecture.WebUI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public abstract class ApiController : ControllerBase
        {
            private IMediator mediator;

            protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
        }
    }
}