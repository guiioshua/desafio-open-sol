using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DesafioGerenciadorTarefas.Core.Services;
using DesafioGerenciadorTarefas.Core.Interfaces;
using DesafioGerenciadorTarefas.Core.Entities;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Exceptions;

namespace DesafioGerenciadorTarefas.Tests.Core.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetPagedAsync_WithHighVolume_ShouldReturnPagedData_AndCalculateTotalPages() // Cenário: Consulta de tarefas com grande volume
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            int totalVolume = 151; // 151 itens requerem matematicamente 16 páginas (15 páginas de 10, e 1 página de 1).

            var pagedTasks = Enumerable.Range(1, pageSize)
                .Select(i => new TaskItem($"Title {i}", $"Desc {i}"))
                .ToList();

            // Configura o Mock para devolver a página preenchida e informar o volume total do banco
            _repositoryMock.Setup(repo => repo.GetPagedAsync(pageNumber, pageSize, null))
                           .ReturnsAsync((pagedTasks, totalVolume));

            // Act
            var result = await _taskService.GetPagedAsync(pageNumber, pageSize, null);

            // Assert
            Assert.Equal(pageSize, result.Items.Count()); // sistema retorna dados paginados
            Assert.Equal(totalVolume, result.TotalCount);

            // Valida se o DTO realiza o cálculo estrutural de páginas corretamente
            Assert.Equal(16, result.TotalPages);
        }

        [Fact]
        public async Task GetPagedAsync_WithStatusFilter_ShouldReturnOnlyFilteredTasks() // Cenário: Consulta por status
        {
            // Arrange
            var statusFilter = StatusTask.Pending;
            var tasks = new List<TaskItem> { new TaskItem("T1", "D1") };

            _repositoryMock.Setup(repo => repo.GetPagedAsync(1, 10, statusFilter))
                           .ReturnsAsync((tasks, 1));

            // Act
            var result = await _taskService.GetPagedAsync(1, 10, statusFilter);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(StatusTask.Pending.ToString(), result.Items.First().Status); // sistema retorna apenas tarefas do status informado
        }

        [Fact]
        public async Task DeleteAsync_ExistingTask_ShouldRemoveSuccessfully() // Cenário: Remoção de tarefa existente
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem("Title", "Desc");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);
            _repositoryMock.Setup(repo => repo.DeleteAsync(task)).Returns(Task.CompletedTask);

            // Act
            await _taskService.DeleteAsync(taskId);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(task), Times.Once); // sistema remove com sucesso
        }

        [Fact]
        public async Task DeleteAsync_NonExistentTask_ShouldThrowDomainException() // Cenário: Remoção de tarefa inexistente
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskItem?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _taskService.DeleteAsync(taskId));

            // Verifica se o sistema retorna mensagem adequada informando que não foi encontrada
            Assert.Equal(ErrorCode.TaskNotFound, exception.Code);
        }
    }
}