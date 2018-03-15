using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interfaces
{
    public interface IAccount
    {
        int Id { get; set; }

        List<int> Permissions { get; set; }
    }
}
