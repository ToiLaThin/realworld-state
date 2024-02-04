using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Realworld.Api.Data;
using Realworld.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ConduitContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConduitConnString"));
    // no host name like . or PostgreSQL 15
});
//bind JwtOptions to appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
        //assign the token from the header to the context, discard the Token prefix
        options.Events = new JwtBearerEvents {
            OnMessageReceived = context => {
                string authorization = context.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authorization)) {
                    context.NoResult();
                    return Task.CompletedTask;
                }

                if (authorization.StartsWith("Token ", StringComparison.OrdinalIgnoreCase)) {
                    context.Token = authorization.Substring("Token ".Length).Trim();
                }

                if (string.IsNullOrEmpty(context.Token)) {
                    context.NoResult();
                }
                return Task.CompletedTask;
            }
        };
    }
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
