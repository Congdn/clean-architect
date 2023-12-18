using System.Reflection;
using System.Xml;

namespace BE.Template.Configurations
{
    public class Log4NetConfiguration
    {
        public static void Log4NetConfig()
        {
            //Dùng cho trường hợp thích dùng serilog , 
            //Log.Logger = new LoggerConfiguration()
            //   .MinimumLevel.Information()
            //   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //   .Enrich.FromLogContext()
            //   .WriteTo.File(@"logs\applog.txt", rollingInterval: RollingInterval.Day)
            //   .CreateLogger();


            //ở đây dùng log4net
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

        }
    }
}
