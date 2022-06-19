using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleTemplate.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string? FirstMail { get; set; }
        public string? SecondMail { get; set; }
        public string? Password { get; set; }
        public string? BrowserExe { get; set; }
        public int Multiplier { get; set; }
    }
}
