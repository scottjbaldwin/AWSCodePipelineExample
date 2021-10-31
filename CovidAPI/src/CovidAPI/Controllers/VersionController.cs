﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CovidAPI.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            var env = Environment.GetEnvironmentVariable("env");
            if (env == "Prod")
            {
                throw new Exception("Contrived production issue");
            }
            return VersionHelper.InformationalVersion;
        }
    }
}
