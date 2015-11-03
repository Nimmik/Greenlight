using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Greenlight.Models.Context
{
    public class DefaultContext : DbContext
    {
        public DefaultContext()
            : base("DefaultConnection")
        {
        }
    }
}