using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DesafioGerenciadorTarefas.Core.DTOs;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Interfaces;

namespace DesafioGerenciadorTarefas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var response = await _taskService.CreateAsync(request);
            // Retorna HTTP 201 Created com o cabeçalho Location apontando para a rota de consulta
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] StatusTask? status = null)
        {
            if (pageNumber < 1)
                return BadRequest(new { error = "O número da página deve ser maior ou igual a 1." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new { error = "O tamanho da página deve ser entre 1 e 50." });

            var response = await _taskService.GetPagedAsync(pageNumber, pageSize, status);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _taskService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut("{id:guid}/details")]
        public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] UpdateTaskDetailsRequest request)
        {
            await _taskService.UpdateDetailsAsync(id, request);
            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTaskStatusRequest request)
        {
            await _taskService.UpdateStatusAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }
}