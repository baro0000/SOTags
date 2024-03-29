using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Sieve.Models;
using Sieve.Services;
using SOTags;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.Domain.Models;
using SOTags.ApplicationServices.Components;
using SOTags.ApplicationServices.Components.Connectors.StackOverflow;
using SOTags.ApplicationServices.Mappings;
using SOTags.DataAccess;
using SOTags.DataAccess.Components;
using SOTags.DataAccess.CQRS;
using SOTags.DataAccess.Entities;
using SOTags.Sieve;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//app.MapPost("sieve", async ([FromBody] SieveModel query, ISieveProcessor sieveProcessor, DatabaseDbContext db) =>
//{
//    var tags = db.Tags.AsQueryable();
//    var sumOfTagUses = tags.Sum(t => t.Count);
//    var domainTags = await sieveProcessor
//    .Apply(query, tags)
//    .Select(t => new SOTags.ApplicationServices.API.Domain.Models.Tag
//    {
//        Name = t.Name,
//        Count = t.Count,
//        Percentage = Math.Round(((t.Count * 100.0) / sumOfTagUses), 2)
//    }).ToListAsync();

//    var totalCount = await sieveProcessor
//    .Apply(query, tags, applyFiltering: false, applySorting: false)
//    .CountAsync();

//    var result = new PagedResult<SOTags.ApplicationServices.API.Domain.Models.Tag>(domainTags, totalCount, query.PageSize.Value, query.Page.Value);

//    return result;
//});

app.Run();
