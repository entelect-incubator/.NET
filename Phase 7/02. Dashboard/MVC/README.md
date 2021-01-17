<img align="left" width="116" height="116" src="../../../pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Dashboard - MVC**

<br/><br/>

## **Starting Point**

Start with your finished solution from Phase 2 or in Phase 3 > 01. StartSolution

### **What are we building?**

A basic Portal for Pezza to manage all their stock, products, orders, customers and restaurant data using MVC.

### **Create a new MVC project**

- [ ] Create a new Solution Folder - 00 UI
- [ ] Create a new Web Application Project
![ASP.NET Core Web Application](Assets/2020-12-10-21-28-37.png)
![Configure your new project - Pezza.Portal](Assets/2020-12-11-05-36-48.png)
![Model-View-Controller](Assets/2020-12-10-21-31-09.png)
- [ ] Copy the Pezza Branding Guide & Design System files found in [Download]() 

![Design Library Assets](Assets/2020-12-10-21-36-45.png)
- [ ] Delete wwwroot > lib
- [ ] Delete existing css from wwwroot > css > style.css

### **Configure**

Download [FontAweomse](https://fontawesome.com/) 

Copy the folder into wwwroot\fonts

![font awesome](Assets/2020-12-15-10-22-26.png)

Add .perfect-scrollbar-on to the html tag

Open Views > Shared > _Layout.cshtml. Add the following in the *`<head>`*

```html
<head>
    <title>Pezza • @((String.IsNullOrEmpty(ViewBag.Title)) ? "Dashboard" : @ViewBag.Title)</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="apple-touch-icon" sizes="76x76" href="../~/lib/img/pezza-logo-white.png" />
    <link rel="icon" type="image/png" href="../~/lib/img/pezza-logo-white.png" />
    <!--     Fonts and icons     -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:200,300,400,600,700,800&display=swap" />
    <link rel="stylesheet" href="~/fonts/fontawesome/css/all.css" />
    <!-- CSS Files -->
    <link rel="stylesheet" href="~/lib/css/design-system.css?v=1.0.0" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
```

Add bd-docs class to the body tag

After the *`<body>`*
```html
<header class="navbar navbar-expand bg-primary flex-column flex-md-row bd-navbar">
    <a class="navbar-brand mr-0 mr-md-2 navbar-absolute-logo" href="https://github.com/entelect-incubator" target="_blank"> <span>Pezza•</span> Portal </a>
    <ul class="navbar-nav flex-row d-none d-md-flex">
        <!--li class="nav-item">
            <a class="nav-link p-2" href="index.html#version"> Home </a>
        </li-->
    </ul>
    <div class="navbar-nav-scroll ml-md-auto">
        <ul class="navbar-nav bd-navbar-nav flex-row">
            <li class="@((ViewBag.ActiveMenu == "Home") ? "active bd-sidenav-active" : "")">
                <a href="/"> Home </a>
            </li>
            <li class="@((ViewBag.ActiveMenu == "Stock") ? "active bd-sidenav-active" : "")">
                <a href="/stock"> Stock </a>
            </li>
            <li class="@((ViewBag.ActiveMenu == "Restaurants") ? "active bd-sidenav-active" : "")">
                <a href="/restaurant"> Restaurants </a>
            </li>
            <li class="@((ViewBag.ActiveMenu == "Products") ? "active bd-sidenav-active" : "")">
                <a href="/product"> Products </a>
            </li>
            <li class="@((ViewBag.ActiveMenu == "Customer") ? "active bd-sidenav-active" : "")">
                <a href="/customer"> Customers </a>
            </li>
            <li class="@((ViewBag.ActiveMenu == "Orders") ? "active bd-sidenav-active" : "")">
                <a href="/orders"> Orders </a>
            </li>
        </ul>
    </div>
    <!-- <a class="btn btn-white d-none d-lg-inline-block" href="">Download</a> -->
</header>
<div class="container-fluid">
    <div class="row flex-xl-nowrap">
        <div class="col-12 col-md-3 col-xl-2 bd-sidebar">
            <nav class="collapse bd-links" id="bd-docs-nav">
                <div class="bd-toc-item active">
                    <ul class="nav bd-sidenav">
                        <li class="active bd-sidenav-active">
                            <a href="/stock"> Stock </a>
                        </li>
                        <li class="">
                            <a href="/restaurant"> Restaurant </a>
                        </li>
                        <li class="">
                            <a href="/product"> Product </a>
                        </li>
                        <li class="">
                            <a href="/customer"> Customer </a>
                        </li>
                        <li class="">
                            <a href="/orders"> Orders </a>
                        </li>
                    </ul>
                </div>
            </nav>
        </div>
        <div class="bd-content nopadding">
            <div id="cover"><div id="content"></div></div>
            <main role="main" class="padding">
                @RenderBody()
            </main>
        </div>
    </div>
</div>
```

After the *`<footer>`*

```html
<!--   Core JS Files   -->
<script src="~/fonts/fontawesome/js/all.js" type="text/javascript"></script>
<script src="~/js/core/jquery.min.js" type="text/javascript"></script>
<script src="~/js/core/popper.min.js" type="text/javascript"></script>
<script src="~/js/core/bootstrap.min.js" type="text/javascript"></script>
<script src="~/js/plugins/perfect-scrollbar.jquery.min.js"></script>
<!--  Plugin for Switches, full documentation here: http://www.jque.re/plugins/version3/bootstrap.switch/ -->
<script src="~/js/plugins/bootstrap-switch.js"></script>
<!--  Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
<script src="~/js/plugins/nouislider.min.js" type="text/javascript"></script>
<!-- Chart JS -->
<script src="~/js/plugins/chartjs.min.js"></script>
<!--  Plugin for the DatePicker, full documentation here: https://github.com/uxsolutions/bootstrap-datepicker -->
<script src="~/js/plugins/moment.min.js"></script>
<script src="~/js/plugins/bootstrap-datetimepicker.js" type="text/javascript"></script>
<!-- Black Dashboard DEMO methods, don't include it in your project! -->
<script src="~/demo/demo.js"></script>
<!-- Control Center for Black UI Kit: parallax effects, scripts for the example pages etc -->
<script src="~/js/design-system.min.js?v=1.0.0" type="text/javascript"></script>
<script src="~/js/site.js"></script>
@RenderSection("Scripts", required: false)
```

## **Startup Projects**

Right Click on the Solution and choose Set Startup Projects...

![Set Startup Projects](Assets/2020-12-10-23-43-55.png)

Click Apply and Press F5

Should have something looking like this -

![First-time look](Assets/2020-12-10-23-50-32.png)

Let's fix up the look and feel a bit

Add the following to style.css

```css
html {
    height: 100vh;
}

.navbar {
    box-shadow: 0px 0px 15px rgba(0,0,0,0.2);
    box-shadow: 0 1px 1px rgba(0,0,0,0.12), 0 2px 2px rgba(0,0,0,0.12), 0 4px 4px rgba(0,0,0,0.12), 0 8px 8px rgba(0,0,0,0.12), 0 16px 16px rgba(0,0,0,0.12);
}

.bd-sidebar {
    background: #bf2437;
    background: linear-gradient(0deg, rgba(135,18,32,1) 0%, rgba(191,36,55,1) 100%);
}

.bd-sidenav li:hover {
    cursor: pointer;
    background-color: #969393;
}

.bd-sidenav-active {
    background-color: #353535;
}

.bd-sidebar .nav > li > a {
    padding: 12px 20px;
}

.table thead th {
    border: none !important;
    background: #757575 !important;
}

    .table thead th:first-child {
        border-radius: 5px 0px 0px 0px;
    }

    .table thead th:last-child {
        border-radius: 0px 5px 0px 0px;
    }

.table tbody td {
    border: none !important;
    background: #969393;
    border-left: 1px solid rgba(255, 255, 255, 0.2) !important;
}

    .table tbody td:first-child {
        border-left: none;
    }

.table tbody tr:nth-child(even) td {
    background: #868686;
}

.table tbody tr:last-child td:first-child {
    border-radius: 0px 0px 0px 5px;
}

.table tbody tr:last-child td:last-child {
    border-radius: 0px 0px 5px 0px;
}

.table a.btn-round {
    border-width: 1px !important;
    border-radius: 30px !important;
    vertical-align: middle !important;
    line-height: 100% !important;
    padding-top: 7px !important;
}

select {
    background-color: #3f3f3f !important;
    padding: 6px 18px 10px 12px !important;
}

.inputfile {
    width: 0.1px;
    height: 0.1px;
    opacity: 0;
    overflow: hidden;
    position: absolute;
    z-index: -1;
}

    .inputfile + label {
        font-size: 1.25em;
        font-weight: 700;
        color: white;
        background-color: black;
        display: inline-block;
    }

        .inputfile:focus + label,
        .inputfile + label:hover {
            background-color: red;
        }
.bd-content {
    max-width: 100% !important;
    -webkit-box-flex: 0;
    -ms-flex: 0 0 calc(100% - 160px);
    flex: 0 0 calc(100% - 160px);
    max-width: calc(100% - 160px);
}
#cover{
    display:none;
}

    #cover #content {
        display: grid;
        place-items: center;
    }

        #cover #content #title {
            margin-top: 9%;
            background: rgba(0,0,0,0.2);
            padding: 20px;
            width: 100%;
            text-align: center;
        }

.nopadding {
    padding: 0 !important;
    margin: 0 !important;
}

.padding {
    padding-left: 45px !important;
    padding-right: 45px !important;
    padding-top: 40px !important;
}

input[type=checkbox] {
    margin-right: 20px;
    transform: scale(2);
}

@media (min-width: 768px) {
    .bd-sidebar {
        height: calc(100vh - 4rem);
        border-right: none;
    }
}

@media (min-width: 1200px) {
    .bd-sidebar {
        max-width: 160px;
    }
}

```

Should look like this now when you run it.

![New branding for the portal](Assets/2020-12-15-08-03-21.png)

In Pezza.Common create a new class AppSettings.cs

```cs
namespace Pezza.Common
{
    public class AppSettings
    {
        public static string ApiUrl { get; set; }
    }
}
```

![AppSettings](Assets/2020-12-15-08-05-32.png)

In Pezza.Portal open appsettings.json and add the AppSettings into it

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "AppSettings": {
    "ApiUrl": "https://localhost:44315/",
  }
}

```

In Pezza.Portal Startup.cs modify the Startup Constructor to look like this.

```cs
public Startup(IHostEnvironment env, IConfiguration configuration)
{
    this.Configuration = configuration;

    this.Configuration = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    this.Configuration.Bind("AppSettings", new AppSettings());
}
```

Add a new property for the HostEnviroment.

```cs
    public IHostEnvironment CurrentEnvironment { get; }
```

This transforms the AppSettings JSON section into a static class to be used anywhere.

## Alerts / Notifications

To be able to show the end-user either an error or just a notification we will need to add a few things into our MVC project.

In your *site.css* add the end and before the media queries add the following:

```css
.toast-container {
    position: absolute;
    right: 5%;
    width: 85%;
    z-index:5000;
    bottom: 0;
    margin: 0 auto;
}

    .toast-container .alert {
        box-shadow: 0px 0px 15px rgba(0,0,0,0.2);
        box-shadow: 0 1px 1px rgba(0,0,0,0.12), 0 2px 2px rgba(0,0,0,0.12), 0 4px 4px rgba(0,0,0,0.12), 0 8px 8px rgba(0,0,0,0.12), 0 16px 16px rgba(0,0,0,0.12);
        display:none;
    }
```

After the media queries add the following

```css
@media (max-width: 768px) {

    .toast-container {
        right: 2%;
        width: 96%;
    }
}
```

Create a new JavaScript file called alert.js in wwwroot\js. This creates a basic alert jquery plugin.

![alert.js](Assets/2020-12-20-14-22-59.png)

```js
(function ($) {
    $(document).ready(function () {
        $('body').append(`
            <div class="toast-container" data-example-id="">
                <div class="alert alert-primary alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-info alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-warning alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-danger alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>
            </div>
        `);
    });

    $.alert = function (message, callback) {
        alert(message, 'primary');

        if (callback) {
            callback();
        }
    };

    $.alertInfo = function (message, callback) {
        alert(message, 'info');

        if (callback) {
            callback();
        }
    };

    $.alertWarning = function (message, callback) {
        alert(message, 'warning');

        if (callback) {
            callback();
        }
    };

    $.alertDanger = function (message, callback) {
        alert(message, 'danger');

        if (callback) {
            callback();
        }
    };

    function alert(message, type) {
        var alert = $('.alert-' + type);
        alert.show();
        alert.find('.alert-message').html(message);
    }

} (jQuery))
```

Now you can just call $.alertWarning('Error removing stock'); for example

In your Views\Shared\_Layout.cshtml bfore you import the site.js file add the following:

```html
       <script src="~/js/site.js"></script>
```

You can now control the alerts\notifications using JQuery.

## Shared Modals

Create a partial view in Views\Shared _Modals.cshtml. This will be used for confirmation popups.

```cshtml
<!-- Remove Confirmation Modal -->
<div class="modal fade" id="confirmationModal" tabindex="-1" role="dialog" aria-labelledby="removeModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <div class="row" id="confirmation-error" style="display:none">
                    <div class="col-12">
                        <div class="alert alert-warning alert-with-icon">
                            <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                                <i class="tim-icons icon-simple-remove"></i>
                            </button>
                            <span data-notify="icon" class="tim-icons icon-bulb-63"></span>
                            <span>
                                <b> Error! - </b> Error occured
                            </span>
                        </div>
                    </div>
                </div>

                <div class="row" id="confirmation-success" style="display:none">
                    <div class="col-12">
                        <div class="alert alert-info alert-with-icon">
                            <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                                <i class="tim-icons icon-simple-remove"></i>
                            </button>
                            <span data-notify="icon" class="tim-icons icon-trophy"></span>
                            <span>
                                <b> Success - </b>
                            </span>
                        </div>
                    </div>
                </div>

                <h5 class="modal-title" id="removeModalLabel">Remove <span class="remove-entity"></span></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                Are you sure you want to remove <span class="remove-name"></span>?
                <input type="hidden" id="removeId" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-link btn-danger" data-dismiss="modal">No</button>
                <button type="button" onclick="confirmRemove()" class="btn btn-primary">Yes</button>
            </div>
        </div>
    </div>
</div>
```

Add a new BaseController to Controllers Folder

![Base Controller](Assets/2020-12-10-23-54-06.png)

```cs
namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : Controller
    {
        private readonly HttpClient client;

        public BaseController()
        {
            this.client = new HttpClient
            {
                BaseAddress = new Uri(AppSettings.ApiUrl)
            };
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
```

## Shared address information

In Phase 3\Data there is a JSON file za.json copy that into your MVC solution under wwwroot\data. We want to create a reusable Address Component that can be reused.

![za.json](2020-12-23-23-52-46.png)

In wwwroot\js create address.js. This loads the JSON data into an object. This will help us create 2 Selects for Cities and Provinces using JQuery. 

```js
(function () {
    var zaData = {};
    var city = [];
    var province = [];
    var base_url = window.location.origin;
    $.getJSON(base_url + '/data/za.json', function (data) {
        zaData = data;
        $.each(zaData, function (i, v) {
            city.push(v.city);
            if ($.inArray(v.admin_name, province) === -1) province.push(v.admin_name);
        });

        city = city.sort();
        province = province.sort();

        for (var i = 0; i < city.length; i++) {
            $("#City").append("<option>" + city[i] + "</option>");
        }

        for (var j = 0; j < province.length; j++) {
            $("#Province").append("<option>" + province[j] + "</option>");
        }
    });
})();
```

Create a new Partial View in Views\Shared _Address.cshtml

```cshtml
@model Pezza.Common.Entities.AddressBase

<fieldset>
    <legend>Address</legend>
    <div class="row">
        <div class="col">
            <label class="label-control">Street Address</label>
            @Html.TextBoxFor(model => model.Address, new { @class = "form-control" })
            @Html.ValidationMessageFor(model => model.Address)
        </div>
    </div>

    <div class="row">
        <div class="col">
            <label class="label-control">City</label>
            <select name="Address.City" id="City" class="form-control"></select>
            @Html.ValidationMessageFor(model => model.City)
        </div>
    </div>

    <div class="row">
        <div class="col">
            <label class="label-control">Province</label>
            <select name="Address.Province" id="Province" class="form-control"></select>
            @Html.ValidationMessageFor(model => model.Province)
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12 col-lg-4">
            <label class="label-control">ZipCode</label>
            @Html.TextBoxFor(model => model.ZipCode, new { @class = "form-control", type = "number" })
            @Html.ValidationMessageFor(model => model.ZipCode)
        </div>
    </div>
    <br />
</fieldset>

<script src="~/js/address.js" defer></script>
```

# CRUD

In the BaseController you would have seen we declared a new HttpCllient. We will use this to make calls to the WebAPI.

- GetAsync - Do an HTTP GET call
- PostAsync - Do an HTTP POST call
- PutAsync - Do an HTTP PUT call

Create a new StockController

![Stock Controller](Assets/2020-12-10-23-56-56.png)

We will only create a CRUD Controller. The Delete will be called using Jquery AJAX.

Method Signatures

```cs
public async Task<ActionResult> Details(int id)

public ActionResult Create()

public async Task<ActionResult> Create(StockDataDTO stock)

public async Task<ActionResult> Edit(int id)

public async Task<ActionResult> Edit(int id, StockDTO stock)

public async Task<JsonResult> Delete(int id)
```

Full Controller

```cs
namespace Pezza.BackEnd.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;

    public class StockController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Stock\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Stock>>(responseData);

            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new Stock();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Stock>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create()
        {
            return this.View(new StockDataDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StockDataDTO stock)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(stock);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Stock", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<StockDataDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(stock);
            }
        }

        [Route("Stock/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new Stock();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Stock>(responseData);
            }
            return this.View(entity.Map());
        }

        [HttpPost]
        [Route("Stock/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, StockDTO stock)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(new StockDataDTO
                {
                    Name = stock.Name,
                    Quantity = stock.Quantity,
                    UnitOfMeasure = stock.UnitOfMeasure,
                    ValueOfMeasure = stock.ValueOfMeasure,
                    ExpiryDate = stock.ExpiryDate,
                    Comment = stock.Comment
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Stock\{id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Stock>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(stock);
            }
        }

        [HttpPost]
        [Route("Stock/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(responseData);
                    
                    return this.Json(response);
                }

                return this.Json(false);

            }
            else
            {
                return this.Json(false);
            }
        }
    }
}

```
Restaurant and Product needs a bit extra information.

Create a RestaurantModel in Models folder.

```cs
namespace Pezza.Portal.Models
{
    using Microsoft.AspNetCore.Http;
    using Pezza.Common.DTO;

    public class RestaurantModel : RestaurantDataDTO
    {
        public int Id { set; get; }

        public IFormFile Image { set; get; }
    }
}
```

![RestaurantModel](2020-12-24-00-25-23.png)

For pictures you need to reference the Picture Controller in Pezza.Api

Index() Method add the following after var entities

```cs
for (var i = 0; i < entities.Count; i++)
{
    entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=restaurant";
}
```

Create() Method you need convert the IFormFile into a Data URI string

```cs
if (restaurant.Image?.Length > 0)
{
    using var ms = new MemoryStream();
    restaurant.Image.CopyTo(ms);
    var fileBytes = ms.ToArray();
    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
}
```

Edit() Method you need to return Picture Url

```cs
return this.View(new RestaurantModel
{
    Id = id,
    Name = entity.Name,
    PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entity.PictureUrl}&folder=restaurant",
    Description = entity.Description,
    Address = new AddressBase
    {
        Address = entity.Address,
        City = entity.City,
        Province = entity.Province,
        ZipCode = entity.PostalCode
    },
    IsActive = entity.IsActive
});
```

Full Controllers

```cs
namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Portal.Models;

    public class RestaurantController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Restaurant\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Restaurant>>(responseData).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=restaurant";
            }
            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new Restaurant();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Restaurant>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create() => this.View(new RestaurantModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RestaurantModel restaurant)
        {
            if (this.ModelState.IsValid)
            {
                if (restaurant.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    restaurant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(restaurant);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Restaurant", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Restaurant>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(restaurant);
            }
        }

        [Route("Restaurant/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new Restaurant();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Restaurant>(responseData);

                return this.View(new RestaurantModel
                {
                    Id = id,
                    Name = entity.Name,
                    PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entity.PictureUrl}&folder=restaurant",
                    Description = entity.Description,
                    Address = new AddressBase
                    {
                        Address = entity.Address,
                        City = entity.City,
                        Province = entity.Province,
                        ZipCode = entity.PostalCode
                    },
                    IsActive = entity.IsActive
                });
            }
            return this.View(new RestaurantModel());
        }

        [HttpPost]
        [Route("Restaurant/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RestaurantModel restaurant)
        {
            if (this.ModelState.IsValid)
            {
                if (restaurant.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    restaurant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(new RestaurantDataDTO
                {
                    Name = restaurant.Name,
                    Description = restaurant.Description,
                    Address = restaurant.Address,
                    IsActive = restaurant.IsActive,
                    ImageData = restaurant.ImageData
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Restaurant/{restaurant.Id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Restaurant>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(restaurant);
            }
        }

        [HttpPost]
        [Route("Restaurant/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(responseData);

                    return this.Json(response);
                }

                return this.Json(false);
            }
            else
            {
                return this.Json(false);
            }
        }
    }
}
```

The .cshtml pages are Razor Pages.

- [ ] [Introduction to ASP.NET Web Programming Using the Razor Syntax (C#)](https://docs.microsoft.com/en-us/aspnet/web-pages/overview/getting-started/introducing-razor-syntax-c)

Remember to add i.e. ViewBag.ActiveMenu = "Stock"; to every view. This makes Menu Item Active.

In the Views folder, create a new folder called Stock. Inside the Stock folder create a View for every Action.

- Index.cshtml
- Create.cshtml
- Edit.cshtml

![](Assets/2020-12-16-06-00-12.png)

## Index.cshtml -

This is the main table that contains all the Stock Items.

```cshtml
@model IEnumerable<Pezza.Common.Entities.Stock>

@{
    ViewBag.Title = "Stock";
    ViewBag.ActiveMenu = "Stock";
}

<div>
    <a class="btn btn-primary" href="/Stock/Create">
        <i class="fa fa-plus" aria-hidden="true"></i>
        Add
    </a>
</div>
<br />
@if (Model.Any())
{
    <table class="table">
        <thead>
            <tr>
                <th>Id</th>
                <th width="150px" class="text-right">Quantity</th>
                <th>Name</th>
                <th>Metric</th>
                <th>Expiry Date</th>
                <th>Date Created</th>
                <th>Comment</th>
                <th width="100px" class="text-right">Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var stock in Model)
            {
            <tr>
                <td>@stock.Id</td>
                <td align="right">@stock.Quantity</td>
                <td>@stock.Name</td>
                <td>@stock.ValueOfMeasure @stock.UnitOfMeasure</td>
                <td>@stock.ExpiryDate</td>
                <td>@stock.DateCreated.ToShortDateString()</td>
                <td>@stock.Comment</td>
                <td align="right">
                    <a rel="tooltip"  class="btn btn-info btn-sm btn-round btn-icon edit" href="/Stock/Edit/@stock.Id">
                        <i class="fa fa-wrench" aria-hidden="true"></i>
                    </a>
                    <button onclick="remove(@stock.Id, '@stock.Name')" type="button" rel="tooltip" class="btn btn-danger btn-sm btn-round btn-icon remove">
                        <i class="fa fa-times" aria-hidden="true"></i>
                    </button>
                </td>
            </tr>
            }
        </tbody>
    </table>

    @await Html.PartialAsync("_Modals")
}
else
{
    <div class="alert alert-info">No Stock Data</div>
}

@section Scripts
{
    <script>
        "use strict";
        function remove(id, name) {
            $('.remove-name').html(name);
            $('#removeId').val(id);
            $('#confirmationModal').modal('show');
        }

        function confirmRemove() {
            $(".alert").hide();

            $('#confirmationModal').modal('hide');
            var data = { Id: parseInt($('#removeId').val()) };
            $.ajax({
                type: "POST",
                url: '@Url.Action("Delete", "Stock")/' + $('#removeId').val(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == true) {
                        $.alertInfo('Stock has been removed', function () {
                            setTimeout(function () { window.location.reload() }, 1500);
                        });
                    }
                    else {
                        $.alertWarning('Error removing stock');
                    }
                },
                error: function (error) {
                    console.log(error);
                    $.alertWarning('Error removing stock');
                }
            });
        }
    </script>
}
```

## Create.cshtml

```cshtml
@model Pezza.Common.DTO.StockDataDTO

@{
    ViewBag.Title = "Add Stock";
}

@using (Html.BeginForm())
{
    @Html.ValidationSummary(true)
    <div class="form-group">
        <fieldset>
            <legend>Add Stock</legend>

            <div class="form-group">
                <div class="row">
                    <div class="col">
                        <label class="label-control">Name</label>
                        @Html.TextBoxFor(model => model.Name, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.Name)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Quantity</label>
                        @Html.TextBoxFor(model => model.Quantity, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.Quantity)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Value Of Measure i.e 50</label>
                        @Html.TextBoxFor(model => model.ValueOfMeasure, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.ValueOfMeasure)
                    </div>
                    <div class="col">
                        <label class="label-control">Unit Of Measure i.e Kg</label>
                        @Html.TextBoxFor(model => model.UnitOfMeasure, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.UnitOfMeasure)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Expiry Date</label>
                        @Html.TextBoxFor(model => model.ExpiryDate, new { @class = "form-control datepicker" })
                        @Html.ValidationMessageFor(model => model.ExpiryDate)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Comment</label>
                        @Html.TextAreaFor(model => model.Comment, new { @class = "form-control " })
                        @Html.ValidationMessageFor(model => model.Comment)
                    </div>
                </div>

                <br />
                <button class="btn btn-primary pull-right" type="submit">Create</button>
            </div>
        </fieldset>
    </div>
}
@section Scripts
{
    <script>
        "use strict";
        $(document).ready(function () {
            $('.datepicker').datetimepicker({
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-screenshot',
                    clear: 'fa fa-trash',
                    close: 'fa fa-remove'
                }
            });
        });
    </script>
}
```

## Edit.cshtml

```cshtml
@model Pezza.Common.DTO.StockDTO

@{
    ViewBag.Title = "Edit Stock";
}

@using (Html.BeginForm())
{
    @Html.ValidationSummary(true)
    <div class="form-group">
        <fieldset>
            <legend>Edit Stock</legend>

            <div class="form-group">
                <div class="row">
                    <div class="col">
                        <label class="label-control">Name</label>
                        @Html.TextBoxFor(model => model.Name, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.Name)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Quantity</label>
                        @Html.TextBoxFor(model => model.Quantity, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.Quantity)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Value Of Measure i.e 50</label>
                        @Html.TextBoxFor(model => model.ValueOfMeasure, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.ValueOfMeasure)
                    </div>
                    <div class="col">
                        <label class="label-control">Unit Of Measure i.e Kg</label>
                        @Html.TextBoxFor(model => model.UnitOfMeasure, new { @class = "form-control" })
                        @Html.ValidationMessageFor(model => model.UnitOfMeasure)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Expiry Date</label>
                        @Html.TextBoxFor(model => model.ExpiryDate, new { @class = "form-control datepicker" })
                        @Html.ValidationMessageFor(model => model.ExpiryDate)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Comment</label>
                        @Html.TextAreaFor(model => model.Comment, new { @class = "form-control " })
                        @Html.ValidationMessageFor(model => model.Comment)
                    </div>
                </div>

                <br />
                <button class="btn btn-primary pull-right" type="submit">Edit</button>
            </div>
        </fieldset>
    </div>
}
@section Scripts
{
    <script>
        "use strict";
        $(document).ready(function () {
            $('.datepicker').datetimepicker({
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-screenshot',
                    clear: 'fa fa-trash',
                    close: 'fa fa-remove'
                }
            });
        });
    </script>
}
```

For better UI/UX Restaurant we will be using Cards on the main page and cover image for Edit/Create.

### Index.cshtml - Uses Cards

```cshtml
@model IEnumerable<Pezza.Common.Entities.Restaurant>

@{
    ViewBag.Title = "Restaurant";
    ViewBag.ActiveMenu = "Restaurants";
}

<div>
    <a class="btn btn-primary" href="/Restaurant/Create">
        <i class="fa fa-plus" aria-hidden="true"></i>
        Add
    </a>
</div>
<br />
@if (Model.Any())
{
    @foreach (var restaurant in Model)
    {
        <div class="card" style="width: 18rem;">
            <img src="@restaurant.PictureUrl" class="card-img-top" alt="@restaurant.Name Store Front">
            <div class="card-body">
                <h3 class="card-title">@restaurant.Name - <small>@(restaurant.IsActive ? "Open" : "Close")</small></h3>
                <p class="card-text">
                    @restaurant.Address,
                    @restaurant.City,
                    @restaurant.Province,
                    @restaurant.PostalCode
                </p>
                <p class="card-text">@restaurant.Description</p>
                <p>Since | @restaurant.DateCreated.ToShortDateString()</p>
                <div class="btn-group" role="group">
                    <a rel="tooltip" class="btn btn-info btn-sm edit" href="/Restaurant/Edit/@restaurant.Id">
                        <i class="fa fa-wrench" aria-hidden="true"></i> Edit
                    </a>
                    <button onclick="remove(@restaurant.Id, '@restaurant.Name')" type="button" rel="tooltip" class="btn btn-danger btn-sm remove">
                        <i class="fa fa-times" aria-hidden="true"></i> Remove
                    </button>
                </div>
            </div>
        </div>
    }

    @await Html.PartialAsync("_Modals")
}
else
{
    <div class="alert alert-info">No Restaurant Data</div>
}

@section Scripts
{
    <script>
        "use strict";
        function remove(id, name) {
            $('.remove-entity').html('Restaurant');
            $('.remove-name').html(name);
            $('#removeId').val(id);
            $('#confirmationModal').modal('show');
        }

        function confirmRemove() {
            $(".alert").hide();

            $('#confirmationModal').modal('hide');
            var data = { Id: parseInt($('#removeId').val()) };
            $.ajax({
                type: "POST",
                url: '@Url.Action("Delete", "Stock")/' + $('#removeId').val(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == true) {
                        $.alertInfo('Stock has been removed', function () {
                            setTimeout(function () { window.location.reload() }, 1500);
                        });
                    }
                    else {
                        $.alertWarning('Error removing stock');
                    }
                },
                error: function (error) {
                    console.log(error);
                    $.alertWarning('Error removing stock');
                }
            });
        }
    </script>
}
```

### Create.cshtml - Uses a Cover Image for better UI/UX

```cshtml
@model Pezza.Portal.Models.RestaurantModel

@{
    ViewBag.Title = "Add Restaurant";
    ViewBag.ActiveMenu = "Restaurants";
}

@using (Html.BeginForm("Create", "Restaurant", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)
    <div class="form-group">
        <fieldset>
            <legend>Add Restaurant</legend>

            <div class="form-group">
                <div class="row">
                    <div class="col">
                        <label class="label-control">Name</label>
                        @Html.TextBoxFor(model => model.Name, new { @class = "form-control", id = "name" })
                        @Html.ValidationMessageFor(model => model.Name)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Upload Image</label>

                        <div class="custom-file overflow-hidden rounded-pill">
                            <input type="file" name="Image" id="Image" class="form-control custom-file-input rounded-pill" />
                            <label for="customFile" class="custom-file-label rounded-pill">Choose file</label>
                        </div>
                    </div>
                </div>

                <br />
                @{ Html.ViewData.TemplateInfo.HtmlFieldPrefix = "Address"; }
                @await Html.PartialAsync("_Address", Model.Address)
                @{ Html.ViewData.TemplateInfo.HtmlFieldPrefix = ""; }

                <div class="row">
                    <div class="col">
                        <label class="label-control">Description</label>
                        @Html.TextAreaFor(model => model.Description, new { @class = "form-control " })
                        @Html.ValidationMessageFor(model => model.Description)
                    </div>
                </div>

                <br />
                <button class="btn btn-primary pull-right" type="submit">Create</button>
            </div>
        </fieldset>
    </div>
}
@section Scripts
{
    <script>
        "use strict";
        $(document).ready(function () {
            $("#Image").change(function (e) {
                var file = e.originalEvent.srcElement.files[0];
                var reader = new FileReader();
                reader.onloadend = function () {
                    var cover = $('#cover');
                    cover.fadeIn();
                    var style = cover.attr('style');
                    style += ';height:300px;background:url("' + reader.result + '") 100% 100%;background-size:cover;';
                    cover.attr('style', style);
                }
                reader.readAsDataURL(file);
            });

            $("#name").on("keydown", function () {
                titleChange($(this).val());
            });

            $("#name").on("change", function () {
                titleChange($(this).val());
            });

            function titleChange(val){
                $('#cover').find("#content").fadeIn().html("<h3 id='title'>" + val + "</h3>");
            }

            $('.datepicker').datetimepicker({
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-screenshot',
                    clear: 'fa fa-trash',
                    close: 'fa fa-remove'
                }
            });
        });
    </script>
}
```

Edit.cshtml

```cshtml
@model Pezza.Portal.Models.RestaurantModel

@{
    ViewBag.Title = "Edit Restaurant";
    ViewBag.ActiveMenu = "Restaurants";
}

@using (Html.BeginForm("Edit", "Restaurant", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)
    <div class="form-group">
        <fieldset>
            <legend>Edit Restaurant</legend>

            <div class="form-group">
                <div class="row">
                    <div class="col">
                        <label class="label-control">Name</label>
                        @Html.TextBoxFor(model => model.Name, new { @class = "form-control", id = "name" })
                        @Html.ValidationMessageFor(model => model.Name)
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col">
                        <label class="label-control">Upload Image</label>

                        <div class="custom-file overflow-hidden rounded-pill">
                            <input type="file" name="Image" id="Image" class="form-control custom-file-input rounded-pill" />
                            <label for="customFile" class="custom-file-label rounded-pill">Choose file</label>
                        </div>
                    </div>
                </div>

                <br />
                <div class="row">
                    <div class="col">
                        <label class="label-control">Description</label>
                        @Html.TextAreaFor(model => model.Description, new { @class = "form-control " })
                        @Html.ValidationMessageFor(model => model.Description)
                    </div>
                </div>

                <br />
                <div class="row">
                    <div class="col">
                        @Html.CheckBoxFor(x => Model._IsActive, new { id = "active_" })
                        <label class="label-control">Active</label>


                    </div>
                </div>

                @Html.HiddenFor(x => x.PictureUrl, new { id = "picture" })
                @Html.HiddenFor(x => x.Id)

                <br />
                @{ Html.ViewData.TemplateInfo.HtmlFieldPrefix = "Address"; }
                @await Html.PartialAsync("_Address", Model.Address)

                <br />
                <button class="btn btn-primary pull-right" type="submit">Edit</button>
            </div>
        </fieldset>
    </div>
}
@section Scripts
{
    <script>
        "use strict";
        $(document).ready(function () {
            var cover = $('#cover');
            cover.fadeIn();
            var style = cover.attr('style');
            var pictureUrl = $("#picture").val();
            style += ';height:300px;background:url("' + pictureUrl + '") 100% 100%;background-size:cover;';
            cover.attr('style', style);
            titleChange($("#name").val());

            $("#Image").change(function (e) {
                var file = e.originalEvent.srcElement.files[0];
                var reader = new FileReader();
                reader.onloadend = function () {
                    var cover = $('#cover');
                    cover.fadeIn();
                    var style = cover.attr('style');
                    style += ';height:300px;background:url("' + reader.result + '") 100% 100%;background-size:cover;';
                    cover.attr('style', style);
                }
                reader.readAsDataURL(file);
            });

            $("#name").on("keydown", function () {
                titleChange($(this).val());
            });

            $("#name").on("change", function () {
                titleChange($(this).val());
            });

            function titleChange(val) {
                $('#cover').find("#content").fadeIn().html("<h3 id='title'>" + val + "</h3>");
            }

            $('.datepicker').datetimepicker({
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-screenshot',
                    clear: 'fa fa-trash',
                    close: 'fa fa-remove'
                }
            });
        });
    </script>
}
```

Now complete the CRUD for the other entities as well

