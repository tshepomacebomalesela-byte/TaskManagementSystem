using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApplication.Common.Interfaces;

namespace TaskApplication.Tasks.Commands
{
    //public class CreateTaskCommand
    //{
    //    // The Command
    //    public record CreateTaskCom(string Name, string Description, int StatusId) : IRequest<int>;

    //}
    public record CreateTaskCommand(string Name, string Description, int StatusId) : IRequest<int>;

}
