using PlateWebAPI.Controllers;
using PlateWebAPI.DBContext;
using PlateWebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<PlateDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BiteDbContextConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(option => option.SignIn.RequireConfirmedEmail = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<BiteDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSession();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    var roles = new[] { "SuperAdmin", "Admin" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//            await roleManager.CreateAsync(new IdentityRole(role));
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    var superAdminEmail = "yasir.s.albardawil@gmail.com";
//    var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
//    if (superAdminUser != null)
//    {
//        await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

//        if (!await userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
//        {
//            await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
//        }
//    }

//    var adminEmail = "admin@gmail.com";
//    var adminUser = await userManager.FindByEmailAsync(adminEmail);
//    if (adminUser != null)
//    {
//        await userManager.AddToRoleAsync(adminUser, "Admin");

//        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
//        {
//            await userManager.AddToRoleAsync(adminUser, "Admin");
//        }
//    }
//}

app.UseSerilogRequestLogging();


app.Run();
