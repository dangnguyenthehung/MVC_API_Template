using System;
using System.Collections.Generic;
using System.Text;

namespace Constants.Interface
{
    public interface IEmployee
    {
        int IdEmployee { get; set; }

        List<int> Permissions { get; set; }
    }
}
