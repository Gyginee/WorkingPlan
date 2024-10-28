using System.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(db => new SqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Register Repository for dependency injection
builder.Services.AddScoped<WorkingPlan.Repository.WorkingPlanRepository>();
builder.Services.AddScoped<Attendant.Repository.AttendantRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WorkingPlanView}/{action=Index}/{page?}");

app.Run();
