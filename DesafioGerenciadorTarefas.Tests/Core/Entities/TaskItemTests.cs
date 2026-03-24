using System;
using Xunit;
using DesafioGerenciadorTarefas.Core.Entities;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Exceptions;

namespace DesafioGerenciadorTarefas.Tests.Core.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void Constructor_WithoutTitle_ShouldThrowDomainException_AndBlockCreation() // Cenário: Criação de tarefa sem título
        {
            // Arrange
            string invalidTitle = "";
            string description = "Description";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new TaskItem(invalidTitle, description));

            // Verifica se o sistema bloqueia e retorna a mensagem correta (código de erro)
            Assert.Equal(ErrorCode.TitleRequired, exception.Code);
        }

        [Fact]
        public void Constructor_ValidData_ShouldCreateTask_WithPendingStatus() // Cenário: Criação de tarefa válida
        {
            // Arrange
            string title = "Valid Title";
            string description = "Description";

            // Act
            var task = new TaskItem(title, description);

            // Assert
            Assert.Equal(title, task.Title);
            Assert.Equal(StatusTask.Pending, task.Status); // Cria tarefa com status Pending
            Assert.NotEqual(default(DateTime), task.CreatedTime);
        }

        [Fact]      
        public void UpdateStatus_FollowingAllowedFlow_ShouldAccept() // Cenário: Atualização de status seguindo o fluxo permitido
        {
            // Arrange
            var task = new TaskItem("Title", "Description"); // Inicia Pending

            // Act
            task.UpdateStatus(StatusTask.InProgress); // Sistema aceita

            // Assert
            Assert.Equal(StatusTask.InProgress, task.Status);
        }

        [Fact]      
        public void UpdateStatus_AttemptingToSkipOrRegress_ShouldThrowDomainException() // Cenário: Tentativa de regressão ou salto de status
        {
            // Arrange
            var task = new TaskItem("Title", "Description"); // Inicia Pending

            // Act & Assert
            // Salto: Pending para Done
            var skipException = Assert.Throws<DomainException>(() => task.UpdateStatus(StatusTask.Done));
            Assert.Equal(ErrorCode.InvalidStateTransitionToInProgress, skipException.Code); // sistema bloqueia a operação

            // Regressão: Preparar para InProgress e tentar voltar para Pending
            task.UpdateStatus(StatusTask.InProgress);
            var regressException = Assert.Throws<DomainException>(() => task.UpdateStatus(StatusTask.Pending));
            Assert.Equal(ErrorCode.InvalidStateTransitionToDone, regressException.Code); // sistema bloqueia a operação
        }
    }
}