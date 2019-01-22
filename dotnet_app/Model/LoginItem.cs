using System;
using System.Collections.Generic;

namespace bitwarden_hibp.Model
{
    public class LoginItem
    {
        public SplitHash SplitHash {get;set;}
        public BitwardenItem Item { get; set; }
    }
}
