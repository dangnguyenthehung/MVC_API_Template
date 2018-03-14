using System;
using System.Collections.Generic;
using System.Text;

namespace Constants.Interface
{
    public interface IAccount
    {
        int Id { get; set; }

        List<int> Permissions { get; set; }
    }
}
