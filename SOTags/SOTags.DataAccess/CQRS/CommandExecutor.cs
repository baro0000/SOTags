using SOTags.DataAccess.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.DataAccess.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly DatabaseDbContext context;

        public CommandExecutor(DatabaseDbContext context)
        {
            this.context = context;
        }

        public Task<TResult> Execute<TParameter, TResult>(CommandBase<TParameter, TResult> command)
        {
            return command.Execute(this.context);
        }
    }
}
