---

## Análise Arquitetural e Decisões de Design

A arquitetura foi desenhada para priorizar a separação de responsabilidades, a consistência dos dados e a escalabilidade organizacional e de performance.

### 1. Modelos de Design de APIs: Abordagem RESTful (Orientada a Recursos)
A modelagem de rotas adotou a **Abordagem RESTful Orientada a Recursos**, rejeitando métodos de chamadas de procedimento remoto (RPC) para transições de estado. 
* **Decisão:** A URI identifica estritamente o recurso através de substantivos e transições de estado são controladas via verbos HTTP e payload (ex: `/api/task/{id}/status`).
* **Justificativa:** Mantém a superfície da API limpa e previsível. Evita-se a "explosão de rotas" à medida que o sistema escala, transferindo a complexidade de transição de estado para o payload que é validado de forma centralizada pelo backend.

### 2. Camada Core: Enums vs. Entidades
Para a representação do estado da tarefa, optou-se pela utilização de **Enums** (`StatusTask`) em vez de tabelas de domínio dinâmicas.
* **Decisão:** Os estados `Pending`, `InProgress` e `Done` são estáticos e codificados diretamente no domínio.
* **Justificativa:** O sistema possui lógica estrita e regras de negócio intrinsecamente acopladas ao estado, onde o fluxo é linear e uma tarefa concluída não permite reversão. O código compilado requer o conhecimento exato destes estados para proteger a consistência; o uso de Enums fornece segurança de tipo (Type Safety) e elimina junções (Joins) com tabelas adicionais para um domínio de baixíssima volatilidade.

### 3. Tratamento de Exceções e Regras de Negócio
A garantia da integridade das regras de negócio foi isolada na entidade, enquanto a comunicação HTTP de falhas é gerida de forma global.
* **Decisão:** Implementação de um **Middleware Global** (`GlobalExceptionMiddleware`) na camada de apresentação.
* **Justificativa:** A entidade atua como guardiã do seu próprio estado, lançando a exceção `DomainException` quando regras de transição falham. O middleware global intercepta estas interrupções no pipeline HTTP, traduzindo o erro estrutural em respostas HTTP padronizadas com os códigos apropriados e um JSON estruturado (`{ error, code }`). Isso garante consistência nas mensagens de erro retornadas aos clientes e remove duplicação de blocos `try-catch` nos controladores.

### 4. Paginação e OFFSET
A extração de dados em alto volume exigiu o fatiamento obrigatório dos registros.
* **Decisão:** Utilização de paginação baseada em OFFSET no SGBD (através do LINQ `Skip` e `Take`), acompanhada de um cálculo dinâmico de `TotalPages` no DTO (`PagedResponse<T>`).
* **Justificativa:** O processamento no banco de dados mitiga a sobrecarga da memória da API ao impedir a transferência de bases inteiras para a aplicação. O DTO centraliza as regras de navegação do estado ao devolver cálculos matemáticos estruturais diretamente da API. Isto eleva a usabilidade para consumidores (Front-end Web ou Mobile), centralizando uma lógica que pode ser consistentemente coberta por testes na camada Core sem exigir duplicação em diferentes plataformas cliente.