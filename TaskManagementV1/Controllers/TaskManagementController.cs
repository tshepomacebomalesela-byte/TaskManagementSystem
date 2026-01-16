using Microsoft.AspNetCore.Mvc;
using TaskApplication.Tasks.Commands;
using MediatR;
using TaskApplication.Tasks.Queries;
using TaskApplication.Tasks.DTOs;


namespace TaskManagementV1.Controllers
{
    /// <summary>
    /// Task Management Controller to handle creating and modification of tasks
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        private readonly ISender _sender;

        public TaskManagementController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Method to create a task
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            var id = await _sender.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// Method to update the Task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var success = await _sender.Send(command);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Method to retrieve tasks by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTaskByIdQuery(id);
            var result = await _sender.Send(query);

            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Get all the tasks
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TaskDTO>>> GetAll(CancellationToken ct)
        {
            var query = new GetAllTasksQuery();
            var result = await _sender.Send(query, ct);
            return Ok(result);
        }
    }
}
