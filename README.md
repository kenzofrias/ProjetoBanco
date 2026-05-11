# 🏦 Sistema de Banco - C# .NET

Um sistema bancário completo e robusto desenvolvido em C# utilizando os princípios da Programação Orientada a Objetos (POO). O projeto simula operações bancárias do mundo real, incluindo gestão de contas corrente e poupança, limites de cheque especial, rendimentos, tarifas, transferências e testes unitários automatizados.

## 🛠️ Tecnologias Utilizadas

* **C#** (Linguagem de programação principal)
* **.NET SDK** (Plataforma e ecossistema de desenvolvimento)
* **xUnit** (Framework utilizado para a criação dos testes unitários)

## 🗂️ Estrutura do Projeto

A solução é dividida em três projetos principais, seguindo boas práticas de modularização e separação de responsabilidades:

```
projeto-banco/
│
├── 📂 sistema-de-banco/
│   ├── 📂 Models/
|   |    ├── 📄 Conta.cs
│   |    ├── 📄 ContaCorrente.cs
│   |    └── 📄 ContaPoupanca.cs
│   └── 📂 Exceptions/
|        ├── 📄 SaldoInsuficienteException.cs
|        ├── 📄 ValorInsuficienteException.cs
|        └── 📄 ContaInativaException.cs
│
├── 📂 demo-banco/
│    └── 📄 Program.cs
│
└── 📂 teste-sistema-de-banco/
     ├── 📄 ContaCorrenteTests.cs
     └── 📄 ContaPoupancaTests.cs
```

* **`sistema-de-banco/`**: A biblioteca de classes (Class Library) principal contendo as regras e o domínio do negócio.
  * `Models/`: Contém as entidades centrais do sistema (`Conta`, `ContaCorrente`, `ContaPoupanca`).
  * `Exceptions/`: Contém as exceções personalizadas para o controle de domínio (`SaldoInsuficienteException`, `ValorInsuficienteException`,`ContaInativaException`).
* **`demo-banco/`**: Uma aplicação de console (Console App) com o `Program.cs`. Ela serve como demonstração prática do uso do sistema, instanciando objetos, injetando dados simulados e realizando operações em tempo real para exibir extratos no console.
* **`teste-sistema-de-banco/`**: Projeto de testes unitários utilizando o framework **xUnit**, que garante a integridade de todas as regras de negócio de depósitos, saques e exceções.



## 🧩 Classes e Modelos Principais

### 1. `Conta` (Classe Base/Abstrata)
Representa a estrutura fundamental de uma conta bancária. Todas as outras contas herdam desta classe, aproveitando seus métodos e atributos.
* **Atributos base:** Número da conta, Titular da conta e Saldo atual.
* **Métodos base:**
  * `Depositar(decimal valor)`: Adiciona fundos à conta. Valida ativamente se o valor inserido é estritamente positivo.
  * `Sacar(decimal valor)`: Retira fundos da conta. As classes filhas determinam se há limite disponível (via polimorfismo).
  * `Transferir(Conta destino, decimal valor)`: Debita da conta de origem e credita na conta de destino informada.
  * `ObterSaldo()`: Retorna o saldo real exato da conta.
  * `ObterSaldoDisponivel()`: Retorna o saldo total de "poder de compra" disponível para o usuário (incluindo o cheque especial, se aplicável).
  * `ExibirExtrato()`: Imprime detalhadamente no console todo o histórico e movimentações da conta.
  * `ToString()`: Retorna uma representação em texto formatada com os dados essenciais da conta.

### 2. `ContaCorrente`
Herda de `Conta`. Representa uma conta padrão para movimentação e uso diário, com suporte a limite de crédito (cheque especial) e taxas de manutenção.
* **Atributos específicos:** `LimiteChequeEspecial` e tarifa periódica.
* **Comportamento específico:**
  * Os métodos `Sacar()` e `Transferir()` sobrescrevem o comportamento padrão para permitir que o saldo fique negativo, suportando débitos até o limite estipulado pelo cheque especial do titular.
  * `CalcularTarifaMensal()`: Deduz automaticamente a tarifa de manutenção do saldo da conta (no exemplo, o teste aponta um custo mensal que pode, inclusive, deixar a conta negativada).

### 3. `ContaPoupanca`
Herda de `Conta`. Ideal para o acúmulo de patrimônio, oferecendo taxa de juros e rendimento sobre o saldo positivo guardado.
* **Atributos específicos:** `TaxaRendimento` (em valor percentual).
* **Comportamento específico:**
  * Diferente da conta corrente, **não possui cheque especial**. Qualquer saque ou transferência superior ao valor real guardado é negado imediatamente.
  * `AplicarRendimento()`: Calcula o percentual de juros determinado no construtor e o adiciona ao saldo. Apenas é aplicado se o saldo for maior que zero.



## ⚠️ Tratamento de Exceções

O sistema implementa uma camada de segurança robusta baseada em Exceções customizadas, não permitindo operações inconsistentes:
* `ValorInsuficienteException`: Disparada imediatamente caso um usuário tente depositar, sacar ou transferir valores negativos (ex: `-100m`) ou zerados (`0m`).
* `SaldoInsuficienteException`: Disparada quando a conta não possui fundos suficientes para a operação (levando em conta que a poupança usa saldo base, enquanto a corrente considera saldo + limite).
* `ContaInativaException`: Dispara caso haja problema na instanciação da propriedade `Ativa` (por padrão é `false`).



## ✅ Testes Unitários

O projeto foi construído focando em qualidade, contando com extensivos testes automatizados na pasta `teste-sistema-de-banco`. Estão cobertos cenários como:
* **Depósitos:** Acúmulo progressivo de saldo, rejeição rigorosa de valores negativos e zeros.
* **Saques:** Atualização correta do *saldo real* vs *saldo disponível*. Testes validando o bloqueio de saques fora de limites ou saques que usam parte do limite especial.
* **Transferências:** Débito atômico na origem e crédito correto e sincronizado no destino.
* **Mecânicas Específicas:** Testes aplicando vários meses (loops) de juros na poupança e observando se o valor bate; testes cobrando mensalidade de clientes de conta corrente zerada e garantindo o uso correto do limite para cobrir a taxa.



## 🌟 Boas Práticas Aplicadas

Durante o desenvolvimento deste projeto, foram aplicadas diversas práticas reconhecidas na engenharia de software:

* **Programação Orientada a Objetos (POO):** Uso massivo de Herança (`ContaPoupanca` e `ContaCorrente` derivando de `Conta`), Encapsulamento (controle rígido de estado interno) e Polimorfismo (sobrescrita de métodos de saque e rendimento).
* **Tratamento de Exceções de Domínio:** Adoção de arquitetura resiliente substituindo retornos booleanos tradicionais por exceções de domínio semanticamente ricas (`SaldoInsuficienteException`, `ValorInsuficienteException`, `ContaInativaException`).
* **Testes Unitários Bem Estruturados:** Construção de testes utilizando as convenções **AAA** (Arrange, Act, Assert) estruturadas via comentários **Given, When, Then**, cobrindo o caminho feliz e casos extremos para proteger a aplicação de regressões.
* **Clean Code:** Código limpo e autoexplicativo com nomenclatura clara em português, divisão de responsabilidades claras (camadas de _Models_ e _Exceptions_) e métodos objetivos.


## 🚀 Como Executar o Projeto

### Pré-requisitos
* **.NET SDK** (Recomendado 6.0 ou superior) instalado em sua máquina.

### Passo a Passo

1. Clone o repositório ou baixe o código fonte.
2. Abra o terminal na pasta raiz do projeto.
3. **Para ver o programa funcionando na prática (Console):**
   ```bash
   cd demo-banco
   dotnet run
   ```
   *O console exibirá criações de contas, transferências sendo feitas, o impacto do cheque especial e a listagem dos extratos finais.*

4. **Para rodar a bateria de testes automatizados e validar o código:**
   ```bash
   cd teste-sistema-de-banco
   dotnet test
   ```
   *Você verá a validação das regras de negócio atestadas como "Passed".*



## 📌 Considerações

Este projeto foi desenvolvido com foco em consolidar conhecimentos em:
- **Programação Orientada a Objetos (POO):** Abstração, herança e polimorfismo.
- **Modelagem de Domínio:** Tradução de regras de negócio reais para código (limites, tarifas, rendimentos).
- **Qualidade de Software:** Criação de testes unitários automatizados utilizando o framework xUnit.
- **Resiliência:** Tratamento robusto de erros criando exceções customizadas da aplicação.
- **Modularidade:** Separação de responsabilidades entre regras de negócio (Class Library) e interface (Console App).

## 📝 Licença
Esse projeto está sob licença. Veja o arquivo [LICENÇA](LICENSE) para mais detalhes.

---

<div align="center">
  
  **Obrigado pela visita!**  
  [Kenzo Friás](https://www.github.com/kenzofrias) © 2026
  
</div>
