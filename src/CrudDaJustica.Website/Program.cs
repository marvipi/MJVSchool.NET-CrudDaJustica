using CrudDaJustica.Data.Lib.Repository;
using CrudDaJustica.Data.Lib.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var sqlServerUsername = Environment.GetEnvironmentVariable("MJVSCHOOLDB_USERNAME");
var sqlServerPassword = Environment.GetEnvironmentVariable("MJVSCHOOLDB_PASSWORD");
var connectionString = string.Format(builder.Configuration.GetConnectionString("SqlServer") ?? "{0}{1}", sqlServerUsername, sqlServerPassword);

builder.Services.AddScoped<IHeroRepository, SqlServerRepository>(serviceProvider => new SqlServerRepository(connectionString));
builder.Services.AddScoped(serviceProvider => new PagingService(
    heroRepository: serviceProvider.GetRequiredService<IHeroRepository>(),
    rowsPerPage: 10));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Hero}/{action=Index}");

app.Run();
