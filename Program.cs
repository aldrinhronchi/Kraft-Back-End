using Kraft_Back_CS.Extensions;
using Kraft_Back_CS.Extensions.Helpers;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
NativeInjector.RegisterBuild(builder);

IServiceCollection services = builder.Services;
NativeInjector.RegisterServices(services);

WebApplication? app = builder.Build();

NativeInjector.ConfigureApp(app, app.Environment);
app.MapControllers();

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine($"==========================================================");
Console.WriteLine($"App Started running in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
Console.WriteLine($"==========================================================");
Console.ForegroundColor = ConsoleColor.Green;

app.Run();