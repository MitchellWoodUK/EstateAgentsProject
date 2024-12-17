using EstateAgents.Data;
using EstateAgents.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUserModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();


//Seed roles and admin user during application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; //Gets the collection of services
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); //Access the services and select role manager.
    var userManager = services.GetRequiredService<UserManager<CustomUserModel>>();
    await SeedRolesAndAdminUser(roleManager, userManager);//Calls method we are going to create.
}


app.Run();


//Helper method to seed roles and admin user
async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager)
{
    string[] roles = { "Admin", "Staff", "Customer" };

    //Add roles to the role manager if they do not exist already.
    foreach (var role in roles) {
        if (!await roleManager.RoleExistsAsync(role)) {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    //Seed the admin user into the database
    var adminEmail = "admin@email.com";
    var adminPassword = "Admin123!";
    if (await userManager.FindByEmailAsync(adminEmail) == null) {
        //Collect all necessary data for the user.
        var adminUser = new CustomUserModel
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            Name = "Admin User",
            Telephone = "012345678901",
            Income = 0.0,
            Address = "Default"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

}
