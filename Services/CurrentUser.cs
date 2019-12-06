using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class CurrentUser
    {
        public CurrentUser(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        public string SexualIdentity { get; set; }
        public string Vizibility { get; set; }
        public Domain.Locality Locality { get; set; }
        public int? ProfilePhoto { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }

    }
}
