using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CoreConsoleTemplate
{
    public class Main 
    {
        private readonly ILogger<Main> _log;
        private readonly IConfiguration _config;
        private readonly IDataAccess _dataaccess;

        public Main(ILogger<Main> log, IConfiguration config, IDataAccess dataAccess)
        {
            _log = log;
            _config = config;
            _dataaccess = dataAccess;
        }
        public void Run()
        {
            //code
        }
    }
}
