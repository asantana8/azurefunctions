using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseNewtonsoftJson();
    })
    .ConfigureServices(services =>
    {
        // Outras configura��es de servi�o, se necess�rio
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // Outras configura��es, se necess�rio
    })
    .ConfigureCors(corsOptions =>
    {
        corsOptions.AddDefaultPolicy(builder =>
        {
            builder
                .WithOrigins("*","http://127.0.0.1:5500/", "http://localhost:4200") // Adicione os dom�nios permitidos
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    })
    .Build();

host.Run(); 