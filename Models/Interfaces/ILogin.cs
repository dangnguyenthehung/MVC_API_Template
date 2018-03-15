using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interfaces
{
    public interface ILogin
    {
        string UserName { get; set; }

        string Password { get; set; }
    }
}
