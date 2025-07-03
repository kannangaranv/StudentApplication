using Microsoft.Extensions.DependencyInjection;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.Repositories;
using StudentSubjectApplication.Infrastructure.DAL;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Presentation.Controller;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;


//var serviceProvider = new ServiceCollection()
//    .AddSingleton<IGenericRepository<Student, Subject>, GenericRepository<Student, Subject>>()
//    .AddSingleton<IGenericRepository<Subject, Student>, GenericRepository<Subject, Student>>()
//    .AddSingleton<Application>()
//    .AddSingleton<StudentContext>()
//    .BuildServiceProvider();


//var application = serviceProvider.GetRequiredService<Application>();
//application.Run();


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddSingleton<IGenericRepository<Student, Subject>, GenericRepository<Student, Subject>>();
builder.Services.AddSingleton<IGenericRepository<Subject, Student>, GenericRepository<Subject, Student>>();
builder.Services.AddSingleton<StudentContext>();

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();


app.MapEndpoints();

app.Run();