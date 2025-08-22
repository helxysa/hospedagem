# Sistema de Hospedagem - Desafio .NET

## Visão Geral

Sistema de gerenciamento de reservas hoteleiras desenvolvido em C# (.NET 9.0) como parte do desafio de projeto da trilha .NET da DIO. O sistema implementa um modelo de domínio robusto para gestão de hóspedes, suítes e reservas, com validações de negócio e cálculos automáticos de tarifas.

## Arquitetura

### Modelo de Domínio

O sistema é construído seguindo princípios de Domain-Driven Design (DDD) com três entidades principais:

- **Pessoa**: Representa o hóspede com informações de identificação
- **Suite**: Define as características da acomodação (tipo, capacidade, tarifa)
- **Reserva**: Estabelece o relacionamento entre hóspedes e suítes com regras de negócio

### Diagrama de Classes

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Pessoa    │    │    Suite    │    │   Reserva   │
├─────────────┤    ├─────────────┤    ├─────────────┤
│ Nome        │    │ TipoSuite   │    │ Hospedes    │
│ Sobrenome   │    │ Capacidade  │    │ Suite       │
│ NomeCompleto│    │ ValorDiaria │    │ DiasReservados│
└─────────────┘    └─────────────┘    └─────────────┘
```

## Funcionalidades

### Gestão de Hóspedes
- Cadastro de pessoas com nome e sobrenome
- Validação de dados obrigatórios
- Geração automática de nome completo

### Gestão de Suítes
- Definição de tipos de acomodação
- Configuração de capacidade máxima
- Estabelecimento de tarifas diárias

### Sistema de Reservas
- Criação de reservas com validações
- Verificação automática de capacidade
- Cálculo inteligente de tarifas
- Aplicação de descontos por volume

## Regras de Negócio

### Validações de Capacidade
O sistema impede a criação de reservas que excedam a capacidade da suíte, lançando `InvalidOperationException` quando necessário.

```csharp
if (hospedes.Count > suite.Capacidade)
{
    throw new InvalidOperationException("Capacidade excedida");
}
```

### Cálculo de Tarifas
A tarifa total é calculada multiplicando o número de dias pela tarifa diária da suíte.

```csharp
decimal valor = DiasReservados * Suite.ValorDiaria;
```

### Política de Descontos
Reservas com duração igual ou superior a 10 dias recebem desconto de 10%.

```csharp
if (DiasReservados >= 10)
{
    valor *= 0.9m; // 10% de desconto
}
```

## Interface de Usuário

O sistema apresenta uma interface de linha de comando (CLI) intuitiva com menu hierárquico:

1. **Cadastrar Hóspede** - Inclusão de novos hóspedes
2. **Cadastrar Suíte** - Configuração de acomodações
3. **Criar Reserva** - Processo de reserva com validações
4. **Listar Hóspedes** - Visualização de cadastros
5. **Listar Suítes** - Consulta de acomodações
6. **Listar Reservas** - Histórico de reservas

## Tecnologias e Dependências

- **Runtime**: .NET 9.0
- **Linguagem**: C# 12.0
- **Paradigma**: Programação Orientada a Objetos
- **Padrões**: Domain-Driven Design, Command Pattern
- **Validações**: Exception handling, input validation

## Estrutura do Projeto

```
trilha-net-explorando-desafio/
├── Models/
│   ├── Pessoa.cs          # Entidade hóspede
│   ├── Suite.cs           # Entidade suíte
│   └── Reserva.cs         # Entidade reserva
├── Program.cs              # Ponto de entrada e interface CLI
├── DesafioProjetoHospedagem.csproj
└── README.md
```

## Execução

### Pré-requisitos
- .NET 9.0 SDK ou superior
- Sistema operacional: Windows, macOS ou Linux

### Comandos de Execução
```bash


# Compilar e executar
dotnet run

# Apenas compilar
dotnet build

# Executar arquivo compilado
dotnet bin/Debug/net9.0/DesafioProjetoHospedagem.dll
```

## Casos de Uso

### Cenário 1: Reserva Padrão
1. Cadastrar hóspede "João Silva"
2. Cadastrar suíte "Standard" (capacidade: 2, tarifa: R$ 100)
3. Criar reserva para 5 dias
4. Resultado: R$ 500,00 (sem desconto)

### Cenário 2: Reserva com Desconto
1. Utilizar hóspedes e suíte existentes
2. Criar reserva para 12 dias
3. Resultado: R$ 1.080,00 (R$ 1.200,00 - 10%)

### Cenário 3: Validação de Capacidade
1. Tentar reservar suíte para 2 pessoas com 3 hóspedes
2. Sistema lança exceção com mensagem explicativa


