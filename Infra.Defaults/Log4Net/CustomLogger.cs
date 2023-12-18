using Default.Application.Interfaces.Logger;
using log4net;
using Newtonsoft.Json;

namespace Infra.Logger
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(T).Name);

        public void LogDebug(object input)
        {
            _logger.Debug(JsonConvert.SerializeObject(input));

        }

        public void LogError(object input, Exception e)
        {
            _logger.Error(JsonConvert.SerializeObject(input));
            _logger.Error(JsonConvert.SerializeObject(e));
        }

        public void LogError(object input)
        {
            _logger.Error(JsonConvert.SerializeObject(input));
        }

        public void LogInfo(object input)
        {
            _logger.Info(JsonConvert.SerializeObject(input));
        }
    }
}