using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.Entity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


// Add services to the container.

#region Data Access

#region Real Estate

// Use SqlServer
builder.Services.AddDbContext<RealEstateContext>(options => options.UseSqlServer(configuration.GetConnectionString("RealEstate")));

#endregion

#region Real Estate Identity

builder.Services.AddDbContext<RealEstateIdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("RealEstateIdentity")));

#endregion

#endregion

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RealEstateIdentityContext>()
    .AddDefaultTokenProviders();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Web API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()

        }
    });
});

var app = builder.Build();

#region Migrate and Seed on Boot

if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("Migrating and seeding database...");
    using var scope = app.Services.CreateScope();
    var realEstateIdentityContext = scope.ServiceProvider.GetRequiredService<RealEstateIdentityContext>();
    var realEstateContext = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
    Console.WriteLine("Checking for pending migrations...");
    var pendingMigrations = realEstateIdentityContext.Database.GetPendingMigrations().Any();
    if (!pendingMigrations)
    {
        Console.WriteLine("No pending migrations.");
    }
    else
    {
        Console.WriteLine("Migrating database...");
        await realEstateContext.Database.MigrateAsync();
        Console.WriteLine("Migration complete.");
    }
    var pendingAuthMigrations = realEstateIdentityContext.Database.GetPendingMigrations().Any();
    if (!pendingAuthMigrations)
    {
        Console.WriteLine("No pending authentication migrations.");
    }
    else
    {
        Console.WriteLine("Migrating authentication database...");
        await realEstateIdentityContext.Database.MigrateAsync();
        Console.WriteLine("Auth migration complete.");
    }

    // Seed database
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    Console.WriteLine("Checking for admin account...");
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser != null)
    {
        Console.WriteLine("Admin account found, skipping authentication seeding.");
    }
    else
    {
        Console.WriteLine("Seeding database...");
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminRole = await roleManager.FindByNameAsync(UserRoles.Admin);
        if (adminRole == null) await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        var userRole = await roleManager.FindByNameAsync(UserRoles.User);
        if (userRole == null) await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        var newAdminUser = new IdentityUser()
        {
            UserName = "admin",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = configuration["Credentials:AdminEmail"]
        };
        var result = await userManager.CreateAsync(newAdminUser, configuration["Credentials:AdminPassword"]);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            // Add the admin as a regular user
            var newUser = new User()
            {
                Name = newAdminUser.UserName,
                Email = newAdminUser.Email,
                Username = newAdminUser.UserName
            };
            realEstateContext.Users.Add(newUser);
            await realEstateContext.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Error creating admin user:");
            foreach (var error in result.Errors) Console.WriteLine(error.Description);
        }
        Console.WriteLine("Authentication seeding complete.");
    }

    // Add default property types, statuses, and currencies
    Console.WriteLine("Checking for default currencies, property types, and statuses...");
    var currencyExists = await realEstateContext.Currencies.AsNoTracking().AnyAsync();
    if (!currencyExists)
    {
        await realEstateContext.Currencies.AddRangeAsync(Currency.GenerateDefault());
        await realEstateContext.SaveChangesAsync();
        Console.WriteLine("Default currencies added.");
    }
    else Console.WriteLine("Default currencies already exist.");
    var typeExists = await realEstateContext.PropertyTypes.AsNoTracking().AnyAsync();
    if (!typeExists)
    {
        await realEstateContext.PropertyTypes.AddRangeAsync(PropertyType.GenerateDefault());
        await realEstateContext.SaveChangesAsync();
        Console.WriteLine("Default property types added.");
    }
    else Console.WriteLine("Default property types already exist.");
    var statusExists = await realEstateContext.PropertyStatuses.AsNoTracking().AnyAsync();
    if (!statusExists)
    {
        await realEstateContext.PropertyStatuses.AddRangeAsync(PropertyStatus.GenerateDefault());
        await realEstateContext.SaveChangesAsync();
        Console.WriteLine("Default property statuses added.");
    }
    else Console.WriteLine("Default property statuses already exist.");

    Console.WriteLine("Seeding process complete.");
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Cors
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
