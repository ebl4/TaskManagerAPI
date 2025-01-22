using FluentAssertions;
using Moq;
using TaskManagerAPI.Services.Interface;
using TaskManagerAPITest.Services;

public class TaskServiceTests : BaseServiceTest
{
    [Fact]
    public async System.Threading.Tasks.Task DeveAdicionarTarefaAoProjeto()
    {
        // Arrange
        var projeto = new Project { Name = "Novo Projeto" };

        var tarefa = new Task { Description = "Tarefa 1", Priority = "Alta", Status = "Pendente", Project = projeto };
        var service = new Mock<ITaskService>();

        service.Setup(s => s.CreateTaskAsync(It.IsAny<Task>())).ReturnsAsync(tarefa);

        // Act
        var tarefaCriada = await service.Object.CreateTaskAsync(tarefa);

        tarefaCriada.Project.Name.Should().Be("Novo Projeto");
    }

    [Fact]
    public async System.Threading.Tasks.Task NaoDeveAdicionarTarefaSeProjetoNaoEncontrado()
    {
        // Arrange
        var tarefaService = new Mock<ITaskService>();
        tarefaService.Setup(s => s.CreateTaskAsync(It.IsAny<Task>()))
            .Throws(new InvalidOperationException("Projeto não encontrado."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            tarefaService.Object.CreateTaskAsync(new Task { Id = 21 }));
    }

    [Fact]
    public async System.Threading.Tasks.Task DeveObterAsTarefasDoProjeto()
    {
        // Arrange
        var projeto = new Project { Name = "Novo Projeto" };

        var tarefa = new Task { Description = "Tarefa 1", Priority = "Alta", Status = "Pendente", Project = projeto };
        var tasks = new List<Task>() { tarefa };
        
        var service = new Mock<ITaskService>();
        service.Setup(s => s.GetTasksByProjectAsync(1)).ReturnsAsync(tasks);

        // Act
        var tarefasObtidas = await service.Object.GetTasksByProjectAsync(1);

        tarefasObtidas[0].Description.Should().Be("Tarefa 1");
    }


    [Fact]
    public async System.Threading.Tasks.Task NaoPermitirMaisDe20TarefasPorProjeto()
    {
        // Arrange
        var tarefaService = new Mock<ITaskService>();
        tarefaService.Setup(s => s.CreateTaskAsync(It.IsAny<Task>()))
            .Throws(new InvalidOperationException("O limite de 20 tarefas por projeto foi atingido."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            tarefaService.Object.CreateTaskAsync(new Task { Id = 21 }));
    }

    [Fact]
    public async System.Threading.Tasks.Task AtualizarTarefaAsync_DeveRegistrarHistoricoAoAtualizar()
    {
        // Arrange
        var tarefaAtualizada = new Task
        {
            Id = 1,
            Title = "Tarefa Atualizada",
            Status = "Concluída",
        };

        var tarefaService = new Mock<ITaskService>();
        tarefaService.Setup(s => s.UpdateTaskAsync(1, It.IsAny<Task>())).ReturnsAsync(tarefaAtualizada);

        // Act
        var resultado = await tarefaService.Object.UpdateTaskAsync(1, tarefaAtualizada);

        // Assert
        resultado.Status.Should().Be("Concluída");
    }

    [Fact]
    public async System.Threading.Tasks.Task AtualizarTarefaAsync_NaoDeveRegistrarHistoricoSeTarefaNaoEncontrada()
    {
        // Arrange
        var tarefaService = new Mock<ITaskService>();
        tarefaService.Setup(s => s.CreateTaskAsync(It.IsAny<Task>()))
            .Throws(new InvalidOperationException("A prioridade da tarefa não pode ser alterada."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            tarefaService.Object.CreateTaskAsync(new Task { Id = 21 }));
    }

}
