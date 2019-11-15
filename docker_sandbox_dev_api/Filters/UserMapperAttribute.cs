using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Filters
{
    public class UserMapperAttribute : TypeFilterAttribute
    {
        public UserMapperAttribute() : base(typeof(UserMapperFilter))
        {
        }
    }
}
