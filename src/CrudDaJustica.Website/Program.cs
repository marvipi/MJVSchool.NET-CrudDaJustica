using CrudDaJustica.Data.Lib.Repository;
using CrudDaJustica.Data.Lib.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var heroDataFilePath = Path.Combine(appDataDirPath, "CRUD da Justica", "herodata.json");

builder.Services.AddScoped<IHeroRepository, JsonRepository>(serviceProvider => new JsonRepository(heroDataFilePath));
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
