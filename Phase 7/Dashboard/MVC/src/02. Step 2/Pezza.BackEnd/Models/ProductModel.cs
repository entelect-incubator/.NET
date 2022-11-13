namespace Pezza.Portal.Models;

using Microsoft.AspNetCore.Http;
using Pezza.Common.DTO;

public class ProductModel : ProductDTO
{
    public IFormFile Image { set; get; }

    public bool HasOffer { set; get; } = false;

    private decimal _price;

    public decimal _Price
    {
        get { return this.Price ?? 0; }
        set
        {
            this._price = value;
            this.Price = value;
        }
    }

    private bool _special;

    public bool _Special
    {
        get { return this.Special ?? false; }
        set
        {
            this._special = value;
            this.Special = value;
        }
    }

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
