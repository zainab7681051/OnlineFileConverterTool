using OnlineFileConverterToolBackend;

var builder = WebApplication.CreateBuilder(args);
var AllowSpecificOrigin = "_allowSpecificOrigin";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowSpecificOrigin, builder =>
            {
                builder
                    .WithOrigins(EnvVariables.Origin)
                    .AllowAnyHeader()
                    .WithMethods("POST");
            });
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowSpecificOrigin);
app.UseAuthorization();

app.MapControllers();

app.Run();
