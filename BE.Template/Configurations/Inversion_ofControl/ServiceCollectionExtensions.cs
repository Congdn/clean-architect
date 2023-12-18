using AutoMapper;
using Default.Application.BusinessServices.StudentService.Handlers;
using Default.Application.BusinessServices.UserServices.dtos;
using Default.Application.CommonFeautures.APIConsumingFlow.Template;
using Default.Application.CommonFeautures.Authentication;

using Default.Application.Interfaces.Authen;
using Default.Application.Interfaces.Authen.dtos;
using Default.Application.Interfaces.Cache;
using Default.Application.Interfaces.Common;
using Default.Application.Interfaces.DbContext;
using Default.Application.Interfaces.Logger;
using Default.Application.Interfaces.Repositories;
using Default.Application.Interfaces.WSO2Authen;
using Default.Application.Mappers;
using Default.Domain.Configs;
using Infra.Defaults.CacheService;
using Infra.Defaults.Repositories;
using Infra.Logger;
using Infra.MakerConsumer.AuthServices;
using Infra.WSO2.Consumer.Authen;
using Infra.WSO2.Consumer.UserQueriesServices;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BE.Template.Configurations.Inversion_ofControl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {

            var mapperConfig = new MapperConfiguration(mc =>
            {
               //Sử dụng auto mapper để mapping dữ liệu từ  tầng Application tới phần APi presentation 
               //Hoặc mapping dữ liệu từ aplication service tới external interface
                mc.AddProfile(new RequestMappingProfile());
                mc.AddProfile(new ResponeMappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            //ReadConfig Object 
            #region đọc config từ appsetting và lưu vào Ioption
            var _defaulConnString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DBConnect");
            services.Configure<Default.Domain.Configs.DefaultDbConfig>(_defaulConnString);

            var _aspnetMakerConfig = new
                ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AspnetMakerConfig");
            services.Configure<Default.Domain.Configs.AspnetMakerConfig>(_aspnetMakerConfig);


            var _wso2Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("WSO2Config");
            services.Configure<Default.Domain.Configs.Wso2AuthConfig>(_wso2Config);
            #endregion


            //Common logger
            services.AddScoped(typeof(ICustomLogger<>), typeof(CustomLogger<>));

            //Inmemory Cache
            services.AddScoped<ISimpleCacheService, InMemoryCacheService>();

            //Dapper
            services.AddScoped<IDapper, Infra.Defaults.DbService.Dapper>();

            //DyamicStoreCalling
            services.AddScoped<ISqlServerDynamicStoreServices, Infra.Defaults.DbService.SqlServerDynamicStoreServices>();

            //services.AddTransient<IBaseRepository, BaseRepository>();
            //services.AddScoped<ICurrentPrincipal, CurrentPrincipal>();
            //services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
            //services.AddScoped(typeof(IDapperProvider), typeof(DapperProvider));
            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<ICurrentPrincipal, CurrentPrincipal>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IMakerAuthService, MakerAuthService>();

            

            //Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
     

            //Unit of work nhưng áp dụng theo template design pattern
            services.AddScoped<GetDataFromHttpTemplate<UserInfoRequest, UserInfoResp>, GetUserDataFromWso2>();



            //Cqrs MediatR
            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateStudentHandler).GetTypeInfo().Assembly));


            //external service 
            services.AddScoped<IWso2Authentication, Wso2Authentication>(); 


            return services;
        }
    }
}
