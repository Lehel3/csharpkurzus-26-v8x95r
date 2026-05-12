using System;
using System.Collections.Generic;
using System.Text;

namespace BlobSimulation.ConsoleRunner.ConsoleCommander {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class CommandNameAttribute: Attribute {
        public string Name { get;}

        public CommandNameAttribute(string name)
            => Name = name;
    }

}
