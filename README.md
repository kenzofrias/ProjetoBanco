# Sistema Bancário

Projeto de estudos em C# e .NET que simula contas correntes e poupanças, operações de crédito e débito, transferências, tarifas, rendimentos e histórico de movimentações.

O objetivo é praticar orientação a objetos, regras de domínio, Entity Framework Core, persistência relacional, testes automatizados e organização de uma solução .NET.

## Tecnologias

- C# com .NET 10
- Entity Framework Core 10
- SQL Server em Docker
- xUnit

## Estrutura

```text
ProjetoBanco/
├── src/
│   ├── ProjetoBanco.Core/
│   │   ├── Enums/
│   │   ├── Exceptions/
│   │   ├── Interfaces/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── ValueObjects/
│   ├── ProjetoBanco.Infrastructure/
│   │   ├── Data/
│   │   ├── Migrations/
│   │   └── Repositories/
│   └── ProjetoBanco.ConsoleApp/
└── tests/
    └── ProjetoBanco.Tests/
```

### Core

Contém as regras do domínio. `Conta` é a entidade base, especializada por `ContaCorrente` e `ContaPoupanca`. As entidades controlam saldo, status, operações permitidas e histórico.

`ContaId`, `ExtratoId` e `UltimaMovimentacao` são value objects usados para representar identificadores e dados de movimentação. Os serviços do Core orquestram operações que envolvem mais de uma entidade, como transferências e geração de extratos.

### Infrastructure

Contém o `BancoDBContext`, as configurações Fluent API do EF Core, as migrações e o repositório de contas. As contas usam herança TPH e os extratos possuem relacionamento com a conta por `ContaId`.

O banco possui uma restrição única para a combinação de número da conta e agência. A validação existe tanto no repositório quanto no banco, porque a regra não deve depender apenas de uma verificação feita pela aplicação.

### ConsoleApp

É uma aplicação de demonstração. Ela aplica as migrações, carrega ou cria contas, executa operações e imprime os extratos.

### Tests

Contém testes das regras de depósito, saque, rendimento, tarifas, transferências, validações e exceções. Os testes não dependem do SQL Server: os fluxos de domínio usam um repositório em memória.

## Regras principais

- Contas precisam ter número e agência válidos.
- O saldo inicial não pode ser negativo.
- Depósitos e débitos exigem valor positivo.
- Contas bloqueadas ou encerradas não podem realizar operações.
- Conta corrente pode utilizar o limite do cheque especial.
- Conta poupança não pode debitar além do saldo disponível.
- A taxa de rendimento da poupança é recebida explicitamente como fração decimal. Por exemplo, `0.05m` representa 5%.
- Uma transferência só é persistida depois que débito e crédito são concluídos.

## Fluxo da aplicação

Ao executar o `ConsoleApp`, o sistema:

1. Lê a variável de ambiente `CONNECTION_STRING`.
2. Aplica as migrations pendentes no SQL Server.
3. Cria ou carrega as contas de demonstração.
4. Executa depósitos, débitos e transferências.
5. Persiste as alterações usando o repositório.
6. Gera e exibe os extratos das contas.

As transferências são orquestradas pelo `TransferenciaService`, que carrega as duas contas, valida o destino, realiza o débito e o crédito e salva as alterações no mesmo contexto do EF Core.

## Exemplos de operações

```csharp
var conta = new ContaPoupanca(
  numero: "12345",
  agencia: "0001",
  saldo: 1000m,
  taxaRendimento: 0.05m);

conta.Creditar(200m);
conta.Debitar(100m);
conta.AplicarRendimento();
```

As taxas são informadas como frações decimais: `0.05m` representa 5% e `0.03m` representa 3%.

## Operações e exceções

| Operação | Condição de erro | Exceção |
| --- | --- | --- |
| Creditar | Conta bloqueada ou encerrada | `ContaInativaException` |
| Creditar | Valor menor ou igual a zero | `ValorInsuficienteException` |
| Debitar | Conta bloqueada ou encerrada | `ContaInativaException` |
| Debitar | Valor menor ou igual a zero | `ValorInsuficienteException` |
| Debitar | Saldo ou limite insuficiente | `SaldoInsuficienteException` |
| Transferir | Conta de origem ou destino inexistente | Exceção de conta padrão do sistema |
| Aplicar rendimento | Saldo menor ou igual a zero | `SaldoInsuficienteException` |

## Testes automatizados

Os testes cobrem atualmente:

- depósitos e débitos válidos;
- uso do cheque especial;
- saldo insuficiente;
- contas bloqueadas;
- aplicação de rendimentos;
- transferências entre contas;
- transferência para conta inativa sem debitar a origem;
- validação de número, agência e saldo inicial;
- identificadores e histórico de movimentações.

Os testes de domínio usam um repositório em memória e não dependem do SQL Server ou do Docker.

## Executando localmente

### Pré-requisitos

- .NET SDK 10
- Docker Desktop

Crie um arquivo `.env` na raiz:

```env
SQL_DOCKER_PASSWORD=SuaSenhaForteAqui!123
```

Inicie o SQL Server:

```powershell
docker compose up -d
```

Defina a conexão no PowerShell:

```powershell
$env:CONNECTION_STRING="Server=localhost;Database=BancoDB;User Id=sa;Password=SuaSenhaForteAqui!123;TrustServerCertificate=True;"
```

Execute a aplicação:

```powershell
cd src/ProjetoBanco.ConsoleApp
dotnet run
```

## Build e testes

Na raiz da solução:

```powershell
dotnet build Projetos.sln
dotnet test Projetos.sln
```

## Migrations do banco

Para listar as migrations:

```powershell
dotnet ef migrations list `
  --project src/ProjetoBanco.Infrastructure `
  --startup-project src/ProjetoBanco.ConsoleApp
```

Para aplicar as migrations manualmente:

```powershell
dotnet ef database update `
  --project src/ProjetoBanco.Infrastructure `
  --startup-project src/ProjetoBanco.ConsoleApp
```

O `ConsoleApp` também aplica automaticamente as migrations pendentes ao iniciar.

### Resetar o banco local

Para remover o container e o volume persistente do SQL Server:

```powershell
docker compose down -v
docker compose up -d
```

O parâmetro `-v` apaga os dados locais do banco.

## Observações de arquitetura

O projeto mantém o domínio no Core para que as regras possam ser testadas sem banco de dados. A Infrastructure conhece o EF Core, mas o domínio não depende da implementação do banco. O ConsoleApp é apenas um consumidor dessas abstrações.

A operação de transferência é responsabilidade de `TransferenciaService` porque envolve duas contas e uma persistência conjunta. As operações simples continuam encapsuladas nas entidades, onde o estado do saldo pode ser protegido.

## Limitações atuais

- O projeto não possui uma API web.
- Não há autenticação ou autorização de usuários.
- O `ConsoleApp` é uma aplicação de demonstração.
- Os testes atuais não executam contra um SQL Server real.
- O sistema depende da variável de ambiente `CONNECTION_STRING` para acessar o banco.

## Licença

Consulte [LICENSE](LICENSE).

---

<div align="center">
  
  **Obrigado pela visita!**  
  [Kenzo Friás](https://www.github.com/kenzofrias) © 2026
  
</div>
