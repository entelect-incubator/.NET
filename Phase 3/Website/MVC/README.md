<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Back-End - MVC**

<br/><br/>

## **Starting Point**

Start with your finished solution from Phase 2 or in Phase 3 > src > 01. StartSolution

### **What are we building?**

A basic Back-End for Pezza to manage all their stock and restaurant data.

### **Create a new MVC project**

- [ ] Create a new Solution Folder - 00 UI
- [ ] Create a new Web Application Project
![ASP.NET Core Web Application](2020-12-10-21-28-37.png)
![Configure your new project - Pezza.Portal](2020-12-11-05-36-48.png)
![Model-View-Controller](2020-12-10-21-31-09.png)
- [ ] Copy the Pezza Branding Guide & Design System files found in [Download]() 
![Design Library Assets](2020-12-10-21-36-45.png)
- [ ] Delete wwwroot > lib
- [ ] Delete existing css from wwwroot > css > style.css

### **Configure**

Add .perfect-scrollbar-on to the html tag

Open Views > Shared > _Layout.cshtml. Add the following in the *`<head>`*

```html
<head>
    <title>Pezza • Dashboard</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="apple-touch-icon" sizes="76x76" href="../~/lib/img/pezza-logo-white.png" />
    <link rel="icon" type="image/png" href="../~/lib/img/pezza-logo-white.png" />
    <!--     Fonts and icons     -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:200,300,400,600,700,800" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.6/css/all.css" />
    <!-- Nucleo Icons -->
    <link rel="stylesheet" href="../~/lib/css/nucleo-icons.css" />
    <!-- CSS Files -->
    <link rel="stylesheet" href="~/lib/css/design-system.css?v=1.0.0" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link rel="stylesheet" href="~/lib/demo/demo.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
```

Add bd-docs class to the body tag

After the *`<body>`*
```html
<header class="navbar navbar-expand bg-primary flex-column flex-md-row bd-navbar">
    <a class="navbar-brand mr-0 mr-md-2 navbar-absolute-logo" href="https://github.com/entelect-incubator" target="_blank"> <span>Pezza•</span> Dashboard </a>
    <ul class="navbar-nav flex-row d-none d-md-flex">
        <!--li class="nav-item">
            <a class="nav-link p-2" href="index.html#version"> Home </a>
        </li-->
    </ul>
    <div class="navbar-nav-scroll ml-md-auto">
        <ul class="navbar-nav bd-navbar-nav flex-row">
            <li class="nav-item">
                <a class="nav-link" href="/" target="_blank" rel="noopener"></a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/" target="_blank" rel="noopener"></a>
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
        <main class="col-12 col-md-9 col-xl-8 py-md-3 pl-md-5 bd-content" role="main">
            @RenderBody()
        </main>
    </div>
</div>
```

After the *`<footer>`*

```html
    <!--   Core JS Files   -->
    <script src="~/lib/js/core/jquery.min.js" type="text/javascript"></script>
    <script src="~/lib/js/core/popper.min.js" type="text/javascript"></script>
    <script src="~/lib/js/core/bootstrap.min.js" type="text/javascript"></script>
    <script src="~/lib/js/plugins/perfect-scrollbar.jquery.min.js"></script>
    <!--  Plugin for Switches, full documentation here: http://www.jque.re/plugins/version3/bootstrap.switch/ -->
    <script src="~/lib/js/plugins/bootstrap-switch.js"></script>
    <!--  Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
    <script src="~/lib/js/plugins/nouislider.min.js" type="text/javascript"></script>
    <!-- Chart JS -->
    <script src="~/lib/js/plugins/chartjs.min.js"></script>
    <!--  Plugin for the DatePicker, full documentation here: https://github.com/uxsolutions/bootstrap-datepicker -->
    <script src="~/lib/js/plugins/moment.min.js"></script>
    <script src="~/lib/js/plugins/bootstrap-datetimepicker.js" type="text/javascript"></script>
    <!-- Black Dashboard DEMO methods, don't include it in your project! -->
    <script src="~/lib/demo/demo.js"></script>
    <!-- Control Center for Black UI Kit: parallax effects, scripts for the example pages etc -->
    <script src="~/lib/js/blk-design-system.min.js?v=1.0.0" type="text/javascript"></script>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @RenderSection("Scripts", required: false)
```

## **Startup Projects**

Right Click on the Solution and choose Set Startup Projects...

![Set Startup Projects](2020-12-10-23-43-55.png)

Click Apply and Press F5

Should have something looking like this -

![First-time look](2020-12-10-23-50-32.png)

Add a new BaseController to Controllers Folder

![Base Controller](2020-12-10-23-54-06.png)

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
                //TODO move to settings
                BaseAddress = new Uri("https://localhost:44315/")
            };
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
```

### **Add common app settings to the solution**

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

Modify Pezza.Portal Starup.cs

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

        public IConfiguration Configuration { get; }

        public IHostEnvironment CurrentEnvironment { get; }
```

In appsettings.json add the following Json

```json

  "AppSettings": {
    "ApiUrl": "Integrated Security=True;Persist Security Info=False;Initial Catalog=VPS.Binlist;Data Source=."
  }
```

### **Let's create the CRUD for the Back-End System**

Create a new BaseController all other Controllers will inherit from

![BaseController.cs](2020-12-11-05-42-56.png)

In the Controllers folder right click and add a new BaseController.cs

```cs
namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Common;

    public abstract class BaseController : Controller
    {
        public readonly HttpClient client;

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

Create a new StockController

![Stock Controller](2020-12-10-23-56-56.png)