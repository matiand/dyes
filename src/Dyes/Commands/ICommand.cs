using System;

namespace Dyes.Commands
{
    public interface ICommand
    {
        public void Run(IWriter writer);
    }
}
