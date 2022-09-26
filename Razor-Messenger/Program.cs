using Microsoft.EntityFrameworkCore;
using Razor_Messenger.Data;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection(SecurityOptions.Security));
builder.Services.AddDbContext<MessengerContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value);
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication("PizzaSlice")
    .AddCookie("PizzaSlice", config =>
    {
        config.Cookie.Name = "PizzaSlice";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MessengerContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();