using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Store.Common.AutoMapper;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.DAL.Services.WebServices;
using Store.Domain.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new DomainToDTOMappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();

var swaggerSettings = builder.Configuration.GetSection("Swagger");

builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["Swagger:Title"],
        Version = builder.Configuration["Swagger:Version"],
        Description = builder.Configuration["Swagger:Description"]
    });
});
builder.Services.AddDbContext<ManagementStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ManagementStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewService, NewService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewRepository, NewRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();



if (!app.Environment.IsProduction() || builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint(swaggerSettings["Endpoint"], $"{swaggerSettings["Name"]} {swaggerSettings["Version"]}");
//    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
