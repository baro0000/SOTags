using Microsoft.EntityFrameworkCore;
using SOTags;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.Components;
using SOTags.ApplicationServices.Components.Connectors.StackOverflow;
using SOTags.ApplicationServices.Mappings;
using SOTags.DataAccess;
using SOTags.DataAccess.Components;
using SOTags.DataAccess.CQRS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining(typeof(ResponseBase<>)));
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
