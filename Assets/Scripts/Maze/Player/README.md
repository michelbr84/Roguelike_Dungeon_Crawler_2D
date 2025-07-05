# Sistema Modular do Jogador

Este diretório contém o sistema modular do jogador, dividido em componentes especializados para facilitar manutenção e expansão.

## Estrutura

```
Scripts/Maze/
├── MazePlayerUtils.cs          # Orquestrador principal
├── MazePlayerMovement.cs       # Movimento e entrada do jogador
├── MazePlayerShooting.cs       # Lógica de disparo
├── MazePlayerBullets.cs        # Movimentação dos projéteis
├── MazePlayerCollision.cs      # Colisões do jogador
└── MazePlayerProtection.cs     # Proteção ao spawn
```

## Componentes

### MazePlayerUtils.cs
**Orquestrador principal** que delega tarefas para os componentes especializados.

**Métodos públicos:**
- `IsValid()` - Validação de posição
- `HandlePlayerInput()` - Entrada do jogador
- `MoveBullets()` - Movimentação de projéteis
- `HandleCollisions()` - Colisões do jogador
- `ActivateSpawnProtection()` - Ativar proteção
- `IsSpawnProtected()` - Verificar proteção

### MazePlayerMovement.cs
**Movimento do jogador** incluindo:
- Input de teclado e touch
- Speed boost
- Efeitos visuais (glow, trilhas)
- Colisão com power-ups
- Verificação de saída do nível

### MazePlayerShooting.cs
**Lógica de disparo** incluindo:
- Verificação de munição
- Power-ups de tiro (single/double/triple)
- Efeitos sonoros
- Estatísticas de tiro

### MazePlayerBullets.cs
**Movimentação e colisões dos projéteis** incluindo:
- Movimento dos projéteis
- Colisão com inimigos
- Colisão com o jogador (projéteis inimigos)
- Efeitos visuais de dano
- Sistema de sniper bullets

### MazePlayerCollision.cs
**Colisões do jogador** incluindo:
- Colisão com inimigos normais
- Colisão com inimigos avançados
- Sistema de escudo
- Proteção ao spawn
- Eventos de desabilitação de ataques

### MazePlayerProtection.cs
**Proteção ao spawn** incluindo:
- Timer de proteção
- Ativação/desativação
- Verificação de status

## Como Usar

### No ProceduralMaze.cs
```csharp
void Update()
{
    // Entrada do jogador (movimento + disparo)
    MazePlayerUtils.HandlePlayerInput(this);
    
    // Movimentação dos projéteis
    MazePlayerUtils.MoveBullets(this);
    
    // Colisões do jogador
    MazePlayerUtils.HandleCollisions(this);
}
```

### Ativar proteção ao spawn
```csharp
// Ao respawnar o jogador
MazePlayerUtils.ActivateSpawnProtection();
```

## Benefícios da Modularização

1. **Separação de Responsabilidades**: Cada componente tem uma função específica
2. **Facilidade de Manutenção**: Bugs e features podem ser isolados
3. **Reutilização**: Componentes podem ser usados independentemente
4. **Testabilidade**: Cada componente pode ser testado separadamente
5. **Expansibilidade**: Novas features podem ser adicionadas sem afetar outros componentes

## Dependências

- `MazeTouchControls` - Controles touch
- `MazeWeatherSystem` - Modificadores de clima
- `MazeEventSystem` - Sistema de eventos
- `MazeShaderEffects` - Efeitos visuais
- `MazeVisualEffects` - Efeitos visuais
- `MazePowerUpUtils` - Sistema de power-ups
- `MazeAdvancedEnemies` - Inimigos avançados
- `MazeStatistics` - Sistema de estatísticas
- `MazeAchievements` - Sistema de achievements
- `MazeMissions` - Sistema de missões
- `AudioManager` - Sistema de áudio
- `MazeHUD` - Interface do usuário

## Compatibilidade

O sistema mantém total compatibilidade com o código existente. Todas as chamadas para `MazePlayerUtils` continuam funcionando normalmente, mas agora são delegadas para os componentes apropriados. 