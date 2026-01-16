using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDomain;

namespace TaskApplication.Tasks.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? StatusName { get; set; }
    }
}
