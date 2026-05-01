using Proyecto_fin_curso_karimsiño.Repository;

var builder = WebApplication.CreateBuilder(args);

//Agregar servicio MVC uwu
builder.Services.AddControllersWithViews();

// Registrar Repositorios
builder.Services.AddScoped<Proyecto_fin_curso_karimsiño.Repository.RegisterRepository>();
builder.Services.AddScoped<RegisterRepository>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
