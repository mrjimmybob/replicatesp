using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Net;

namespace replicatesp
{
    public class Configuration
    {
        public string Receipt { get; set; }
        public List<Data> Connections { get; set; }

    }

    public class Data
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }


}
