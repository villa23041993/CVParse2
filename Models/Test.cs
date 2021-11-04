using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVParse2.Models
{
    public class Test
    {
        [AllowHtml]
        public string Description { get; set; }
    }
}