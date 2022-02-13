using FlatManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Utility
{
    public class GetFlowClass
    {
        private readonly FlatDBContext _context;
        private IWebHostEnvironment webHostEnvironment;
        public IConfiguration _configuration;

        public GetFlowClass() { }
        public GetFlowClass(FlatDBContext context, IWebHostEnvironment _webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            webHostEnvironment = _webHostEnvironment;
            _configuration = configuration;
        }

        public string GetFlow(string flowName, Double amount)
        {
            
                                           //.Where(p => p.AmountLimit_MIN >= amount)
                                           //.Where(p => p.AmountLimit_MAX <= amount)
            var GetRole = _context.ApprovalLimits
                                           .Where(p => p.Id == 5)
                                           .Select(p => p.RoleName).First(); 
            return GetRole;
        }
    }
}
