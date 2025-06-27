using Microsoft.Extensions.DependencyInjection;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.Repositories;
using StudentSubjectApplication.Presentation;
using StudentSubjectApplication.DAL;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IStudentRepository, StudentRepository>()
    .AddSingleton<ISubjectRepository, SubjectRepository>()
    .AddSingleton<Application>()
    .AddSingleton<StudentContext>()
    .BuildServiceProvider();


var application = serviceProvider.GetRequiredService<Application>();
application.Run();