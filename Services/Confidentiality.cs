using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class Confidentiality
    {
        static public string Public
        {
            get
            {
                return "public";
            }
        }
        static public string FriendsOnly
        {
            get
            {
                return "friends";
            }
        }

        static public string Private
        {
            get
            {
                return "private";
            }
        }
    }
}
