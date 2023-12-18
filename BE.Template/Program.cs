using BE.Template.Configurations.Inversion_ofControl;
using Default.Application.BusinessServices.StudentService.Handlers;
using Default.Application.CommonFeautures.Authorization;
using Infra.Defaults.Cors;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Register tung Hander
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateStudentHandler).GetTypeInfo().Assembly));
//Lay ra toan bo cac Class handler cua cqrs de register 
var allTypes = typeof(Default.Application.BusinessServices.StudentService.Handlers.CreateStudentHandler).Assembly.GetTypes();
var allHanderCqrs = allTypes  //typeof(Program).Assembly.GetTypes()
    .Where(s => s.Name.EndsWith("Handler") && s.IsInterface == false)
    .Select(item => item.GetTypeInfo().Assembly).ToList<Assembly>();
foreach (var appService in allHanderCqrs)
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appService));


var services = builder.Services;

//Inversion of Control (tách ra class khác để code cho sạch)
services.RegisterServices();



#region config jwt
var issuer = builder.Configuration["Jwt:Issuer"];
var Audience = builder.Configuration["Jwt:Audience"];
var key = builder.Configuration["Jwt:Key"];
//Custom authentication

var tokenValidationParameters = new TokenValidationParameters
{

	// Token signature will be verified using a private key.
	ValidateIssuerSigningKey = true,
	IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

	// Token will only be valid if contains below domain (e.g http://localhost) for "iss" claim.
	ValidateIssuer = true,
	ValidIssuer = issuer,

	// Token will only be valid if contains below domain (e.g http://localhost) for "aud" claim.
	ValidateAudience = true,
	ValidAudience = Audience,

	// Token will only be valid if not expired yet, with 5 minutes clock skew.
	ValidateLifetime = true
};

builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme = "default";
})
		.AddScheme<AuthenticationSchemeOptions, ApiAuthHandler>("Api", o => { })
		.AddPolicyScheme("default", "Authorization Bearer or Cookies", options => {
			options.ForwardDefaultSelector = context =>
			{
			
				string authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
				return (authHeader?.StartsWith("Bearer ") == true) ? JwtBearerDefaults.AuthenticationScheme : "Api";
			};
		})
			
			.AddJwtBearer(options => {
				options.TokenValidationParameters = tokenValidationParameters;
			});

builder.Services.AddAuthorization();
#endregion 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Thêm phần này (bắt buộc) khi sử dụng authentication cho api
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


//Custom middleware
app.UseMiddleware<CorsMiddleware>();

//Config log4net
BE.Template.Configurations.Log4NetConfiguration.Log4NetConfig();


//Custom Middleware for permisssion
//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
//{
//	appBuilder.UseMiddleware<PermissionCustomHeaderAttribute>();
//});


app.Run();
