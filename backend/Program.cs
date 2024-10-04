using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Authorization;
using backend.Data;
using DotNetEnv;
using backend.Interfaces;
using backend.GraphQL;
using backend.GraphQL.Queries;
using backend.GraphQL.Mutations;
using backend.Repositories;
using backend.Services;
using System.Net.Http.Headers;
using Resend;
Env.Load();

var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");
var managementApiToken = Environment.GetEnvironmentVariable("MANAGEMENT_API_TOKEN");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)); 

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType(d => d.Name("Query"))
        .AddType<UserQuery>()
        .AddType<EmailQuery>()
         .AddType<PasswordQuery>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddType<UserMutation>()
        .AddType<EmailMutation>()
        .AddType<PasswordMutation>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true
    };
});


builder.Services.AddHttpClient<IAuth0Client, Auth0Client>(client =>
{
    client.BaseAddress = new Uri($"https://{auth0Domain}/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", managementApiToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>( o =>
{
    o.ApiToken = Environment.GetEnvironmentVariable("RESEND_APITOKEN")!;
} );
builder.Services.AddTransient<IResend, ResendClient>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTokenRepository, EmailTokenRepository>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();  

app.Run();