using Microsoft.Extensions.DependencyInjection;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.Repositories;
using StudentSubjectApplication.Presentation;


var serviceProvider = new ServiceCollection()
    .AddSingleton<IStudentRepository, StudentRepository>()
    .AddSingleton<Application>()
    .BuildServiceProvider();

var application = serviceProvider.GetRequiredService<Application>();
application.Run();