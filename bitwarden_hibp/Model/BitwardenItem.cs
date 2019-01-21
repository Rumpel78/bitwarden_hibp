using System;
using System.Collections.Generic;

namespace bitwarden_hibp.Model
{
    public class BitwardenExport
    {
        public List<BitwardenItem> Items { get; set; }
    }
    public class BitwardenItem
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public BitwardenItemLogin Login { get; set; }
    }

    public class BitwardenItemLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
