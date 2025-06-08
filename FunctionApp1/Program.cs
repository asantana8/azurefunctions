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
        // Outras configurações de serviço, se necessário
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // Outras configurações, se necessário
    })
    .ConfigureCors(corsOptions =>
    {
        corsOptions.AddDefaultPolicy(builder =>
        {
            builder
                .WithOrigins("*","http://127.0.0.1:5500/", "http://localhost:4200") // Adicione os domínios permitidos
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    })
    .Build();

host.Run(); 