﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SE344.Controllers
{
    public class StocksController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
