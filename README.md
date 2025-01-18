# Task Manager API

## Como Rodar

1. Clone o repositório:
```git clone <url>```

2. Navegue até o diretório do projeto:
```cd TaskManagerAPI```

3. Construa e rode os containers Docker:
```docker-compose up --build```

4. Acesse a API em `http://localhost:5000`.

## Endpoints

- **POST /api/project**: Criação de um novo projeto.
- **GET /api/project**: Listagem de todos os projetos.
- **POST /api/task**: Criação de uma nova tarefa.
- **GET /api/task?projectId=1**: Listagem das tarefas de um projeto.
- **PUT /api/task/{id}**: Atualização de uma tarefa.

