using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var serviceOptions = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>();

builder.Services.AddSingleton(serviceProvider => {
    var mongoDbOptions = builder.Configuration.GetSection(nameof(MongoDbOptions)).Get<MongoDbOptions>();
    var mongoClient = new MongoClient(mongoDbOptions.ConnectionString);
    return mongoClient.GetDatabase(serviceOptions.ServiceName);
});

builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

builder.Services.AddControllers(options => 
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
