Set-Location "c:\Dev\Incubator\.NET"
$root = "c:\Dev\Incubator\.NET"

$phases = @(
  "Phase 1\src\02. EndSolution",
  "Phase 2\src\01. StartSolution",
  "Phase 2\src\02. EndSolution",
  "Phase 3\src\01. StartSolution",
  "Phase 3\src\02. Step1",
  "Phase 3\src\03. EndSolution",
  "Phase 4\src\01. StartSolution",
  "Phase 4\src\03. Step2",
  "Phase 5\src\01. StartSolution",
  "Phase 5\src\02. Step 1",
  "Phase 5\src\03. EndSolution",
  "Phase 6\src\01. StartSolution",
  "Phase 6\src\02. Step 1",
  "Phase 6\src\03. Step 2",
  "Phase 7\src\01. StartSolution",
  "Phase 7\src\02. Step 1",
  "Phase 7\src\03. Step 2",
  "Phase 7\src\04. Step 3",
  "Phase 9\src\01. StartSolution"
)

Write-Host "=== STEP 1: DELETE TODO ARTIFACTS ==="
foreach ($p in $phases) {
  $dir = "$root\$p"
  if (-not (Test-Path $dir)) { Write-Host "SKIP (missing): $p"; continue }
  $targets = @(
    "$dir\Common\Entities\Todo.cs",
    "$dir\Common\Models\Todos",
    "$dir\Common\Models\TodoModel.cs",
    "$dir\Core\Todos",
    "$dir\Core\TodoCore.cs",
    "$dir\Core.Contracts\ITodoCore.cs",
    "$dir\Api\Controllers\TodosController.cs",
    "$dir\Core\Email\Templates\TodoEmail.html",
    "$dir\DataAccess\Mapping\TodoMap.cs",
    "$dir\Test\Core\TestTodoCore.cs",
    "$dir\Test\Setup\TestData\Todos"
  )
  foreach ($t in $targets) {
    if (Test-Path $t) { Remove-Item $t -Recurse -Force; Write-Host "DELETED: $($t.Substring(28))" }
  }
}

Write-Host ""
Write-Host "=== STEP 2: RESTORE MISSING ENTITIES AND MAPS ==="

$srcCustomer = Get-Content "$root\Phase 4\src\02. Step1\Common\Entities\Customer.cs" -Raw
$srcPizza = Get-Content "$root\Phase 4\src\02. Step1\Common\Entities\Pizza.cs" -Raw
$srcPizzaMap = Get-Content "$root\Phase 4\src\02. Step1\DataAccess\Mapping\PizzaMap.cs" -Raw

# Phases that need ONLY Pizza.cs entity restored (no Customer)
$needPizzaOnly = @(
  "Phase 1\src\02. EndSolution",
  "Phase 2\src\01. StartSolution"
)

# Phases that need BOTH Customer.cs and Pizza.cs entities restored
$needCustomerAndPizza = @(
  "Phase 2\src\02. EndSolution",
  "Phase 3\src\01. StartSolution",
  "Phase 3\src\02. Step1",
  "Phase 4\src\01. StartSolution",
  "Phase 5\src\01. StartSolution",
  "Phase 5\src\02. Step 1",
  "Phase 6\src\01. StartSolution",
  "Phase 6\src\02. Step 1",
  "Phase 6\src\03. Step 2"
)

# Phase 7/01 only needs Pizza.cs (Customer.cs already exists)
$needPizzaOnlyLate = @(
  "Phase 7\src\01. StartSolution"
)

# Phases that need PizzaMap.cs added to DataAccess/Mapping
$needPizzaMap = @(
  "Phase 1\src\02. EndSolution",
  "Phase 2\src\01. StartSolution",
  "Phase 2\src\02. EndSolution",
  "Phase 3\src\01. StartSolution",
  "Phase 3\src\02. Step1",
  "Phase 4\src\01. StartSolution",
  "Phase 5\src\01. StartSolution",
  "Phase 5\src\02. Step 1",
  "Phase 6\src\01. StartSolution",
  "Phase 6\src\02. Step 1",
  "Phase 6\src\03. Step 2"
)

foreach ($p in $needPizzaOnly + $needPizzaOnlyLate) {
  $entDir = "$root\$p\Common\Entities"
  if (Test-Path $entDir) {
    if (-not (Test-Path "$entDir\Pizza.cs")) { Set-Content "$entDir\Pizza.cs" $srcPizza -Encoding UTF8; Write-Host "ADDED Pizza.cs: $p" }
  }
}

foreach ($p in $needCustomerAndPizza) {
  $entDir = "$root\$p\Common\Entities"
  if (Test-Path $entDir) {
    if (-not (Test-Path "$entDir\Customer.cs")) { Set-Content "$entDir\Customer.cs" $srcCustomer -Encoding UTF8; Write-Host "ADDED Customer.cs: $p" }
    if (-not (Test-Path "$entDir\Pizza.cs")) { Set-Content "$entDir\Pizza.cs" $srcPizza -Encoding UTF8; Write-Host "ADDED Pizza.cs: $p" }
  }
}

foreach ($p in $needPizzaMap) {
  $mapDir = "$root\$p\DataAccess\Mapping"
  if (Test-Path $mapDir) {
    if (-not (Test-Path "$mapDir\PizzaMap.cs")) { Set-Content "$mapDir\PizzaMap.cs" $srcPizzaMap -Encoding UTF8; Write-Host "ADDED PizzaMap.cs: $p" }
  }
}

Write-Host ""
Write-Host "=== STEP 3: FIX DATABASE CONTEXTS ==="

# Standard Customer+Pizza DatabaseContext content
$dbCtxCustomerPizza = @'
namespace DataAccess;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Customer> Customers { get; set; }
	public virtual DbSet<Pizza> Pizzas { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CustomerMap());
		modelBuilder.ApplyConfiguration(new PizzaMap());
	}

	protected override void OnConfiguring
	   (DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}
'@

# Phases needing Customer+Pizza DbContext fix
$fixCustomerPizza = @(
  "Phase 2\src\02. EndSolution",
  "Phase 3\src\01. StartSolution",
  "Phase 3\src\02. Step1",
  "Phase 3\src\03. EndSolution",
  "Phase 5\src\02. Step 1",
  "Phase 5\src\03. EndSolution",
  "Phase 6\src\01. StartSolution",
  "Phase 6\src\02. Step 1",
  "Phase 6\src\03. Step 2",
  "Phase 7\src\02. Step 1",
  "Phase 7\src\03. Step 2"
)

foreach ($p in $fixCustomerPizza) {
  $ctx = "$root\$p\DataAccess\DatabaseContext.cs"
  if (Test-Path $ctx) { Set-Content $ctx $dbCtxCustomerPizza -Encoding UTF8; Write-Host "FIXED DbContext (Customer+Pizza): $p" }
}

# Phase 7/04.Step3: Customer+Notify+Pizza
$dbCtxCustomerNotifyPizza = @'
namespace DataAccess;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Customer> Customers { get; set; }
	public virtual DbSet<Pizza> Pizzas { get; set; }
	public virtual DbSet<Notify> Notifies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CustomerMap());
		modelBuilder.ApplyConfiguration(new PizzaMap());
		modelBuilder.ApplyConfiguration(new NotifyMap());

		modelBuilder.Entity<Pizza>()
			 .HasData(
			  new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow });
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}
'@

$ctx74 = "$root\Phase 7\src\04. Step 3\DataAccess\DatabaseContext.cs"
if (Test-Path $ctx74) { Set-Content $ctx74 $dbCtxCustomerNotifyPizza -Encoding UTF8; Write-Host "FIXED DbContext (Customer+Notify+Pizza): Phase 7/04.Step3" }

# Phase 9/01.StartSolution: Customer+Notify+Order+Pizza
$dbCtxPhase9 = @'
namespace DataAccess;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Customer> Customers { get; set; }
	public virtual DbSet<Pizza> Pizzas { get; set; }
	public virtual DbSet<Notify> Notifies { get; set; }
	public virtual DbSet<Order> Orders { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Order>().HasNoKey();
		modelBuilder.ApplyConfiguration(new CustomerMap());
		modelBuilder.ApplyConfiguration(new PizzaMap());
		modelBuilder.ApplyConfiguration(new NotifyMap());
		modelBuilder.ApplyConfiguration(new OrderMap());

		modelBuilder.Entity<Pizza>()
			 .HasData(
			  new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow },
			  new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow });
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}
'@

$ctx91 = "$root\Phase 9\src\01. StartSolution\DataAccess\DatabaseContext.cs"
if (Test-Path $ctx91) { Set-Content $ctx91 $dbCtxPhase9 -Encoding UTF8; Write-Host "FIXED DbContext (Customer+Notify+Order+Pizza): Phase 9/01.StartSolution" }

Write-Host ""
Write-Host "=== ALL DONE ==="
