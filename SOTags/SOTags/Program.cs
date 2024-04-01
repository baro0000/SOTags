using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using SOTags;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.Validators;
using SOTags.ApplicationServices.Components;
using SOTags.ApplicationServices.Components.Connectors.StackOverflow;
using SOTags.ApplicationServices.Mappings;
using SOTags.DataAccess;
using SOTags.DataAccess.Components;
using SOTags.DataAccess.CQRS;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetPagedTagsRequestValidator>());
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining(typeof(ResponseBase<>)));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAutoMapper(typeof(TagProfile).Assembly);
builder.Services.AddTransient<IQueryExecutor, QueryExecutor>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();
builder.Services.AddTransient<IStackOverflowConnector, StackOverflowConnector>();
builder.Services.AddTransient<IStackOverflowJsonReader, StackOverflowJsonReader>();
builder.Services.AddDbContext<DatabaseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseDbContext")));
builder.Services.AddTransient<DatabaseInitializer>();
builder.Services.AddTransient<IInitialDataLoader, InitialDataLoader>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Stack Overflow Tags",
            Version = "v1",
            Description = "An API to get list of tags from Stack Overflow API",
            Contact = new OpenApiContact
            {
                Name = "Bartosz Kuli�ski",
                Email = "bartosz.kulinski.it@gmail.com",
            }
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SOTags.xml");
    c.IncludeXmlComments(xmlPath);
});

builder.Logging.ClearProviders(); 
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

var app = builder.Build();

var initializer = app.Services.GetRequiredService<DatabaseInitializer>();
await initializer.Initialize(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
