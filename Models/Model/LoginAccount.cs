using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;

namespace Model.Model
{
    public class LoginAccount : ILogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
