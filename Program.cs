using APIEcomerce.Implements;
using APITodoList.Implements;
using APITodoList.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware;
using Supabase;
using System.Text;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUtilities, Utilities>();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
// Add services to the container.
var key = builder.Configuration.GetValue<string>("JWT:Secret_Key");
var keyBytes = Encoding.ASCII.GetBytes(key);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
	//Esto es para generar la ui predeterminada de la documentación swagger
	swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Version = "v1",
		Title = "JWT Token Authentication Ecomerce",
		Description = ".net 8 Web API"
	});
	//Para habilitar la autorización mediante swagger (JWT)
	swagger.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Encabezado de autorización JWT utilizando el esquema Bearer. Ingrese 'Bearer' [espacio] y su token en la siguiente entrada. Ejemplo: 'Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA'"
	});
});

builder.Services.AddAuthentication(cfg =>
{
	cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8
			.GetBytes(key)
		),
		ValidateIssuer = false,
		ValidateAudience = false,
		ClockSkew = TimeSpan.Zero
	};
});

builder.Services.AddScoped<Supabase.Client>(SUPA =>
new Supabase.Client(
		builder.Configuration.GetValue<string>("SupaBase:Url"),
		builder.Configuration.GetValue<string>("SupaBase:Key"),
		new SupabaseOptions
		{
			AutoConnectRealtime = true
		}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
