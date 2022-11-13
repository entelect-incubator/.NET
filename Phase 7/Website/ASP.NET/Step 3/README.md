<img align="left" width="116" height="116" src="../../../pezza-logo.png" />

# &nbsp;**Pezza - Phase 7- Step 3** [![.NET 7 - Phase 7 - Dashboard - MVC](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-dashboard-mvc.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase7-dashboard-mvc.yml)

<br/><br/>

## **Filtering**

Add SearchModel to all your List functions in your Controllers.

```cs
[HttpPost]
public async Task<JsonResult> List([FromBody] SearchModel<CustomerDTO> searchmodel)
{
    var entity = searchmodel.SearchData;
    entity.OrderBy = searchmodel.OrderBy;
    entity.PagingArgs = new Common.Models.PagingArgs
    {
        Limit = searchmodel.Limit,
        Offset = (searchmodel.Page - 1) * searchmodel.Limit,
        UsePaging = true
    };
    var json = JsonConvert.SerializeObject(entity);
    var result = await this.apiCallHelper.GetListAsync(json);
    return this.Json(result);
}
```

Let's go and change the index.cshtml

Change the Add Button to include Filter Button

```html
<div class="row">
    <div class="col-4">
        <a class="btn btn-primary" href="/Customer/Create">
            <i class="fa fa-plus" aria-hidden="true"></i>
            Add
        </a>
    </div>
    <div class="col-4">
        <div class="input-group">
            <input type="text" class="form-control" placeholder="Name" id="Name" />
            <div class="input-group-append">
                <button id="FilterButton" class="btn filter-button btn-outline-secondary" type="button">Filter</button>
            </div>
        </div>
    </div>
    <div class="col-4 text-right">
        <span id="CountData">0</span> Customers
    </div>
</div>
<br />
```

Add SearchModel to your $.ajax call

New model

```js
var dto = new Object;
dto.Name = $("#Name").val();

var searchModel = new Object();
searchModel.SearchData = dto;
searchModel.Limit = parseInt($("#Limit").val());
searchModel.Page = parseInt($('#Page').val());
searchModel.OrderBy = orderBy;
```

Ajax Data Call

```js
data: JSON.stringify(searchModel),
```

Change CustomerFilter to be able to search using any case of text. 

```cs
public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        return query;
    }

    return query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
}
```

You can add filtering to any of the other index views if you want.

## **Phase 8**

Move to Phase 8
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%208)