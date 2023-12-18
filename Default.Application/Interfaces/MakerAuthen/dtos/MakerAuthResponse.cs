using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Authen.dtos
{
    public class MakerAuthResponse
    {
        public string? JWT { get; set; }
        public string? message { get; set; }    
        public string? httpCode { get; set; }
    }
}
