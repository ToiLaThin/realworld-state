using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Realworld.Api.Data;
using Realworld.Api.Utils;
using Realworld.Api.Services;
using Realworld.Api.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Realworld.Api.Utils.ExceptionHandling;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// https://stackoverflow.com/a/59924569/23165722 & https://github.dev/gothinkster/aspnetcore-realworld-example-app
// this is the config require to use FluentValidation before filter
builder.Services.AddControllers(opts => opts.Filters.Add(new ValidateModelStateFilter()))
                .ConfigureApiBehaviorOptions(opts => opts.SuppressModelStateInvalidFilter = true);
                //this is require for filter to get called (default behaviour if [ApiController] is used is that return problem details)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Information()
    //logging of Microsoft and System is too verbose, so we override it
    //they're only logged in the warning level, sourceContext 
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.WithProperty("ThreadId", Environment.ProcessId)
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}",
        theme: AnsiConsoleTheme.Code
    )
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day, 
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}"
    )
    .WriteTo.File(
        formatter: new JsonFormatter(), 
        path: "logs/log-.json"
    )
);

// //global logger
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information()
//     .Enrich.WithProperty("ThreadId", Environment.ProcessId)
//     .WriteTo.Console(
//         outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}",
//         theme: AnsiConsoleTheme.Code
//     )
//     .WriteTo.File(
//         path: "logs/log-.txt",
//         rollingInterval: RollingInterval.Day, 
//         outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}"
//     )
//     .WriteTo.File(
//         formatter: new JsonFormatter(), 
//         path: "logs/log-.json"
//     )
//     .CreateLogger();

// //this is the message
// Log.Information("Starting up {AppName}", builder.Environment.ApplicationName);

//builder.Services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the bearer scheme (\"Token {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    option.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<ConduitContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConduitConnString"));
    // no host name like . or PostgreSQL 15
});
//bind JwtOptions to appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]))
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
builder.Services.AddAuthorization(opt => opt.AddPolicy(
    Policy.OptionalAuthenticated, p => p.AddRequirements(new OptionalAuthRequirement())
));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ICurrentUsernameAccessor, CurrentUsernameAccessor>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();

//register the auth handler
builder.Services.AddSingleton<IAuthorizationHandler, OptionalAuthHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<GetAuthInfoMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
