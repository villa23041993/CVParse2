using CVParse2.Models;
using CVParse2.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVParse2.Services
{
    public class SercurityService
    {
        SercurityDAO Sercurity = new SercurityDAO();
        public bool Authenticate(UserModel userModel)
        {
            return Sercurity.FindByUser(userModel);
        }
        public void Create(RegisterModel registerModel)
        {
             Sercurity.CreateUser(registerModel);
        }
    }
}