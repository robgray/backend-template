using System;
using System.Text;

namespace tests.Plumbing
{
    public class Constants
    {
        public static readonly string TestSecurityKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("qwertyuiop198571qwertyuiop198571"));
    }
}