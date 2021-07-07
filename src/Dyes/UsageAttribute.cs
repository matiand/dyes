using System;

namespace Dyes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UsageAttribute : Attribute
    {
        public string CmdWithArgs;
        public string Description;
        public string Example;

        public UsageAttribute(string cmdWithArgs, string description, string example = "")
        {
            CmdWithArgs = cmdWithArgs;
            Description = description;
            Example = example;
        }
    }
}
