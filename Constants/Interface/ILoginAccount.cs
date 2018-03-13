using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Constants.Interface
{
    public interface ILoginAccount
    {
        string UserName { get; set; }
        
        string Password { get; set; }
    }
}
