using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Hubs;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection(SecurityOptions.Security));
builder.Services.AddHttpClient();
builder.Services.AddDbContext<MessengerContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value);
});
builder.Services.AddIdentityCore<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<MessengerContext>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IEmotionService, EmotionService>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddAuthentication("PizzaSlice")
    .AddCookie("PizzaSlice", config =>
    {
        config.Cookie.Name = "PizzaSlice";
        config.ExpireTimeSpan = TimeSpan.FromHours(2);
    });
builder.Services.AddSignalR();
builder.Services.AddAntiforgery(option => option.HeaderName = "X-XSRF-TOKEN");
builder.WebHost.UseUrls("http://*:5030;https://*:5031");

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
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");
app.MapHub<UserListHub>("/userListHub");

app.Run();
