using CoreConsoleTemplate.Bussines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleTemplate
{
    public class Main 
    {
        private readonly ILogger<Main> _log;
        private readonly IConfiguration _config;
        private readonly IDataAccess _dataaccess;
        private readonly ISendRequest _sendRequest;

        public Main(ILogger<Main> log, IConfiguration config, IDataAccess dataAccess, ISendRequest sendRequest)
        {
            _log = log;
            _config = config;
            _dataaccess = dataAccess;
            _sendRequest = sendRequest;
        }
        public async Task Run()
        {
            await _sendRequest.CheckAppointment();
        }
    }
}
