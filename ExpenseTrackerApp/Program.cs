using System.Text;
using ExpenseTrackerApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;

{
    string connectionString = config["Db:ConnectionString"];
    services.AddDbContext<ExpenseTrackerDbContext>(o =>
    {
        o.UseNpgsql(connectionString);
    },
    ServiceLifetime.Scoped
    );

    string secret = config["JwtTokenConfig:Secret"];
    string issuer = config["JwtTokenConfig:Issuer"];
    string audience = config["JwtTokenConfig:Audience"];
    int accessTokenExpire = Convert.ToInt32(config["JwtTokenConfig:AccessTokenExpire"]);

    var key = Encoding.UTF8.GetBytes(secret);
    services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience
        };
    });

    services.AddCors(o =>
    {
        o.AddPolicy("expensetracker",
        b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    });

    services.AddAuthorization();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

}

{
    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseCors("expensetracker");

    app.MapControllers();

    UpgradeDb(app);

    app.Run();

}

static void UpgradeDb(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetService<ExpenseTrackerDbContext>();
    if (context != null && context.Database != null)
    {
        context.Database.Migrate();
    }
}






