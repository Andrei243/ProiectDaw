using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Text;

namespace DataAccess.Configurations
{
    class SocializRConfiguration:DbMigrationsConfiguration<SocializRContext>
    {
        public SocializRConfiguration()
        {
            AutomaticMigrationsEnabled = false;

        }
    }
}
