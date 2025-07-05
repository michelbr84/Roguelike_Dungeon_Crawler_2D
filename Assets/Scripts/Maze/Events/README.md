# Maze Event System - Estrutura Modular

Este diretório contém o sistema de eventos modular dividido em componentes especializados para facilitar manutenção e evolução.

## Estrutura

```
Scripts/Maze/Events/
├── MazeEventSystem.cs           # Orquestrador principal - controla eventos ativos, update, save/load
├── MazeEventTypes.cs            # Enum EventType e helpers de nome/cor
├── MazeGameEvent.cs             # Classe MazeGameEvent (dados, configurações padrão)
├── MazeEventEffects.cs          # Aplica/Remove efeitos de evento no game
├── MazeEventHelpers.cs          # Funções auxiliares (reward, unlock, random, compatibilidade)
└── README.md                    # Esta documentação
```

## Como Usar

### Para Usar o Sistema de Eventos

```csharp
// Inicializar
Events.MazeEventSystem.Initialize();

// Atualizar (chamar no Update)
Events.MazeEventSystem.Update(Time.deltaTime);

// Verificar evento ativo
var activeEvent = Events.MazeEventSystem.GetActiveEvent();
if (Events.MazeEventSystem.IsEventActive())
{
    float timeRemaining = Events.MazeEventSystem.GetEventTimeRemaining();
}

// Obter modificadores
float enemySpawnMod = Events.MazeEventSystem.GetEnemySpawnModifier();
float powerUpMod = Events.MazeEventSystem.GetPowerUpModifier();
```

### Para Usar Tipos e Helpers

```csharp
// Obter nome e cor de um tipo de evento
string eventName = Events.MazeEventTypes.GetEventTypeName(Events.MazeEventTypes.EventType.TreasureHunt);
Color eventColor = Events.MazeEventTypes.GetEventTypeColor(Events.MazeEventTypes.EventType.EnemyRush);

// Gerar evento aleatório
var randomEvent = Events.MazeEventHelpers.GenerateRandomEventReward();

// Forçar início de evento específico
Events.MazeEventSystem.ForceEvent(Events.MazeEventTypes.EventType.PowerSurge);
```

### Para Compatibilidade (Código Existente)

```csharp
// O arquivo original MazeEventSystem.cs ainda funciona como wrapper
MazeEventSystem.Initialize();
MazeEventSystem.Update(Time.deltaTime);
MazeEventSystem.GetActiveEvent();
// etc...
```

## Benefícios da Divisão

1. **Modularidade**: Cada componente tem responsabilidade única
2. **Manutenibilidade**: Fácil encontrar e corrigir problemas
3. **Escalabilidade**: Adicionar novos tipos de eventos é trivial
4. **Reutilização**: Componentes podem ser usados independentemente
5. **Organização**: Código mais limpo e organizado

## Adicionando Novos Tipos de Eventos

1. Adicione o novo tipo no enum `Events.MazeEventTypes.EventType`
2. Implemente o case correspondente em `Events.MazeGameEvent.SetDefaultsForType()`
3. Adicione nome e cor em `Events.MazeEventTypes.GetEventTypeName()` e `GetEventTypeColor()`

## Namespace

Todos os componentes estão no namespace `Events` para evitar conflitos:

```csharp
using Events; // Para usar diretamente
// ou
Events.MazeEventSystem.Initialize(); // Para usar com namespace completo
```

## Compatibilidade

O sistema mantém total compatibilidade com o código existente:
- `MazeEventSystem.cs` original funciona como wrapper
- Todas as interfaces públicas permanecem inalteradas
- Nenhuma quebra de funcionalidade

## Responsabilidades por Componente

- **MazeEventSystem**: Orquestração, eventos ativos, update, save/load
- **MazeEventTypes**: Enum, nomes, cores, helpers de tipo
- **MazeGameEvent**: Dados do evento, configurações padrão
- **MazeEventEffects**: Aplicação e remoção de efeitos
- **MazeEventHelpers**: Funções auxiliares e utilitários 