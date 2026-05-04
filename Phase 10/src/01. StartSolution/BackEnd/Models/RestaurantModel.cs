namespace Portal.Models;

using Microsoft.AspNetCore.Http;
using Common.DTO;

public class RestaurantModel : RestaurantDTO
{
    public IFormFile Image { set; get; }

    private bool _isActive;

    public bool _IsActive
    {
        get { return this.IsActive ?? false; }
        set
        {
            this._isActive = value;
            this.IsActive = value;
        }
    }
}
