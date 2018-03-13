using System;
using System.Collections.Generic;
using System.Text;
using ApiCall.Static;

namespace ApiCall
{
    public class DemoHelper : BaseHelper
    {
        public override string _token { get; set; }

        public DemoHelper(string token) : base(token)
        {
        }

        public int Insert(string url, string demo)
        {
            return _Insert(url, demo);
        }
    }

}
