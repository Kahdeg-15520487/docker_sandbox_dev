using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Dal.Model
{
    public class User
    {
        public Guid UserId { get; set; }
        public virtual ICollection<Sandbox> Sandboxes { get; set; }
        public int Port { get; set; }
    }
}
