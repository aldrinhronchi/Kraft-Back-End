using KaibaSystem_Back_End.Extensions;
using KaibaSystem_Back_End.Extensions.Helpers;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
NativeInjector.RegisterBuild(builder);

IServiceCollection services = builder.Services;
NativeInjector.RegisterServices(services);

WebApplication? app = builder.Build();

NativeInjector.ConfigureApp(app, app.Environment);
app.MapControllers();

Console.WriteLine($"==========================================================");
Console.WriteLine($"App Started running in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
Console.WriteLine($"==========================================================");

app.Run();