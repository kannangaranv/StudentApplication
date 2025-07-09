using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.DAL;
using StudentSubjectApplication.Infrastructure.Repositories;
using StudentSubjectApplication.Presentation.Controller;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IGenericRepository<Student, Subject>, GenericRepository<Student, Subject>>();
builder.Services.AddSingleton<IGenericRepository<Subject, Student>, GenericRepository<Subject, Student>>();
builder.Services.AddSingleton<StudentContext>();

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});




var hostUrl = Environment.GetEnvironmentVariable("HOST_URL");
builder.WebHost.UseUrls(hostUrl);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapEndpoints();

app.Run();