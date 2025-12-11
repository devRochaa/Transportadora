using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Transportadora.Application.Adress.Services;
using Transportadora.Application.Clients.Services;
using Transportadora.Application.Supplier.Services;
using Transportadora.Domain.Abstractions;
using Transportadora.Infrasctructure.Context;
using Transportadora.Infrasctructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<CreateClientService>();
builder.Services.AddScoped<GetClientService>();
builder.Services.AddScoped<GetClientByIdService>();
builder.Services.AddScoped<UpdateClientService>();
builder.Services.AddScoped<GetSupplierAddressByIdAsync>();
builder.Services.AddScoped<GetSuppliersService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transportadora API",
        Version = "v1",
        Description = "API para gerenciamento de clientes"
    });

    // Se você gerar o XML de documentação, descomente e ajuste o caminho:
    // var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Rodar migrations uma única vez no startup (opcional, mas recomendado)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transportadora v1");
        c.RoutePrefix = string.Empty; // abre UI em / (opcional). Remova ou ajuste se preferir /swagger
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
