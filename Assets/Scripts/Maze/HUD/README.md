# Maze HUD System - Estrutura Modular

Este diretório contém o sistema HUD modular dividido em componentes especializados para facilitar manutenção e evolução.

## Estrutura

```
Scripts/Maze/HUD/
├── MazeHUD.cs              # Orquestrador principal - chama todos os outros HUDs
├── MazeLifeHUD.cs          # Gerencia vidas, corações e vida fracionária
├── MazeAmmoHUD.cs          # Gerencia munição (ícones ou texto)
├── MazePowerUpHUD.cs       # Gerencia power-ups ativos e shield
├── MazeStatsHUD.cs         # Gerencia estatísticas básicas (score, level, recorde)
├── MazeMetaHUD.cs          # Gerencia classe, pet, clima, evento, dificuldade
├── MazeStatusHUD.cs        # Gerencia mensagens de status centralizadas
├── MazeMissionHUD.cs       # Gerencia missões ativas
└── README.md               # Esta documentação
```

## Como Usar

### Para Desenhar o HUD Completo
```csharp
// No OnGUI ou onde for necessário
MazeHUD.DrawHUD(mazeObj);
```

### Para Desenhar Componentes Específicos
```csharp
// Apenas vidas
MazeLifeHUD.Draw(mazeObj);

// Apenas power-ups
MazePowerUpHUD.Draw(mazeObj);

// Apenas missões
MazeMissionHUD.Draw();
```

### Para Atualizar Dados
```csharp
// Adicionar score
MazeHUD.AddScore(100);

// Avançar level
MazeHUD.NextLevel();

// Mostrar mensagem de status
MazeHUD.ShowStatusMessage("Power-up coletado!");

// Resetar tudo
MazeHUD.ResetAll();
```

## Benefícios da Divisão

1. **Manutenibilidade**: Cada componente é responsável por uma área específica
2. **Escalabilidade**: Fácil adicionar novos elementos HUD
3. **Debugging**: Problemas isolados em componentes específicos
4. **Reutilização**: Componentes podem ser usados independentemente
5. **Organização**: Código mais limpo e organizado

## Adicionando Novos Elementos HUD

1. Crie um novo arquivo `MazeNovoElementoHUD.cs`
2. Implemente o método `public static void Draw(ProceduralMaze mazeObj)`
3. Adicione a chamada em `MazeHUD.DrawHUD()`

```csharp
// Em MazeHUD.cs
public static void DrawHUD(ProceduralMaze mazeObj)
{
    MazeLifeHUD.Draw(mazeObj);
    MazeAmmoHUD.Draw(mazeObj);
    // ... outros HUDs
    MazeNovoElementoHUD.Draw(mazeObj); // Nova linha
}
```

## Compatibilidade

O sistema mantém total compatibilidade com o código existente:
- `MazeHUD.RenderHUD(maze)` continua funcionando
- Todas as interfaces públicas permanecem inalteradas
- Nenhuma quebra de funcionalidade

## Responsabilidades por Componente

- **MazeHUD**: Orquestração, score, level, status geral
- **MazeLifeHUD**: Vidas, corações, vida fracionária
- **MazeAmmoHUD**: Munição (ícones ou texto)
- **MazePowerUpHUD**: Shield, power-ups temporários, proteção ao spawnar
- **MazeStatsHUD**: Score, level, recorde, contagem de inimigos
- **MazeMetaHUD**: Classe, pet, clima, evento, dificuldade
- **MazeStatusHUD**: Mensagens de status centralizadas
- **MazeMissionHUD**: Missões ativas 