using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Contract.Dtos
{
    public class SandboxCreationConfig
    {
        public string Port { get; set; }
        public string ConfigDir { get; set; }
        public string ProjectDir { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public override string ToString()
        {
            //-p 9090:8080 -v \"C:/Users/Minh/.local/share/code-server:/home/coder/.local/share/code-server\" -v \"C:/Users/Minh:/home/coder/project\" -e \"PASSWORD=123456\" codercom/code-server:v2
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(" -p {0}:8080", Port);
            builder.AppendFormat(" -v \"{0}:/home/coder/.local/share/code-server\"", ConfigDir);
            builder.AppendFormat(" -v \"{0}:/home/coder/project\"", ProjectDir);
            builder.AppendFormat(" -e \"PASSWORD={0}\"", Password);
            builder.AppendFormat(" {0}", Image);

            return builder.ToString();
        }
    }
}
