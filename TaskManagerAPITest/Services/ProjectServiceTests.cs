using Moq;
using TaskManagerAPI.Services;
using TaskManagerAPI.Services.Interface;
using TaskManagerAPITest.Services;

public class ProjectServiceTests : BaseServiceTest
{
    [Fact]
    public async System.Threading.Tasks.Task DeveRetornarTodosOsProjetos()
    {
        // Arrange
        var projetosMock = new List<Project>
        {
            new Project { Id = 1, Name = "Projeto 1" },
            new Project { Id = 2, Name = "Projeto 2" }
        };

        var projectService = new Mock<IProjectService>();
        projectService.Setup(p => p.GetProjectsAsync()).ReturnsAsync(projetosMock);

        // Act
        var projetos = await projectService.Object.GetProjectsAsync();

        // Assert
        Assert.Equal(2, projetos.Count);
        Assert.Equal("Projeto 1", projetos.First().Name);
    }

    [Fact]
    public async System.Threading.Tasks.Task CriarProjetoAsync_DeveAdicionarProjeto()
    {
        // Arrange
        var projetos = new List<Project>();
        var mockProjetosDbSet = CreateMockDbSet(projetos);

        MockContext.Setup(c => c.Projects).Returns(mockProjetosDbSet.Object);

        var service = new ProjectService(MockContext.Object);

        var novoProjeto = new Project { Name = "Novo Projeto" };

        // Act
        var projetoCriado = await service.CreateProjectAsync(novoProjeto);

        // Assert
        Assert.Equal("Novo Projeto", projetoCriado.Name);
        MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); // Verifica que SaveChanges foi chamado
    }


    [Fact]
    public async System.Threading.Tasks.Task NaoDeveRetornarProjetoSeNaoExistir()
    {
        var projectService = new Mock<IProjectService>();
        projectService.Setup(p => p.GetProjectByIdAsync(1))
            .Throws(new InvalidOperationException("Projeto não encontrado.")); ;

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => projectService.Object.GetProjectByIdAsync(1)
        );

        Assert.Equal("Projeto não encontrado.", ex.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoverProjetoAsync_DeveLancarExcecao_SeExistiremTarefasPendentes()
    {
        // Arrange
        var projetoMock = new Project
        {
            Id = 1,
            Name = "Projeto com Tarefas",
            Tasks = new List<Task> { new Task { Id = 1, Status = "Pendente" } }
        };

        var projectService = new Mock<IProjectService>();
        projectService.Setup(p => p.DeleteProjectAsync(1))
            .Throws(new InvalidOperationException("Não é possível remover um projeto com tarefas pendentes.")); ;

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => projectService.Object.DeleteProjectAsync(1)
        );

        Assert.Equal("Não é possível remover um projeto com tarefas pendentes.", ex.Message);
    }


}
