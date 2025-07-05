# Sistema de Menus - Estrutura Organizada

## Visão Geral

O sistema de menus foi dividido em arquivos separados para facilitar manutenção, expansão e personalização. Cada menu tem sua própria responsabilidade e pode ser modificado independentemente.

## Estrutura de Arquivos

```
Scripts/Maze/Menu/
├── MazeMenu.cs              # Orquestrador principal - controla navegação entre menus
├── MazeMainMenu.cs          # Menu principal (Start, Tutorial, Ranking, etc.)
├── MazeRankingMenu.cs       # Menu de ranking (TOP 10 scores)
├── MazeStatsMenu.cs         # Menu de estatísticas do jogador
├── MazePauseMenu.cs         # Menu de pause durante o jogo
├── MazeGameOverMenu.cs      # Tela de Game Over
├── MazeVictoryMenu.cs       # Tela de vitória
└── MazeMenuInput.cs         # Sistema de input para todos os menus
```

## Como Funciona

### MazeMenu.cs (Orquestrador)
- **Responsabilidade**: Controla qual menu está ativo e gerencia navegação
- **Métodos principais**:
  - `DrawMenu(record, lives)` - Decide qual menu mostrar
  - `ShowRanking()`, `HideRanking()` - Controla visibilidade do ranking
  - `ShowStats()`, `HideStats()` - Controla visibilidade das estatísticas
  - Métodos de compatibilidade com `ProceduralMaze`

### Menus Específicos
Cada menu tem apenas uma responsabilidade:

- **MazeMainMenu.cs**: Menu principal com todos os botões de navegação
- **MazeRankingMenu.cs**: Exibe ranking de scores e permite limpar
- **MazeStatsMenu.cs**: Mostra estatísticas detalhadas do jogador
- **MazePauseMenu.cs**: Menu de pause com opções de continuar/configurações
- **MazeGameOverMenu.cs**: Tela de fim de jogo com opções de restart/sair
- **MazeVictoryMenu.cs**: Tela de vitória com opções de próximo nível/menu

### MazeMenuInput.cs
- **Responsabilidade**: Centraliza todo o input de teclado dos menus
- **Métodos**:
  - `HandleMenuInput()` - Input para menu principal
  - `HandleGameOverInput()` - Input para tela de game over

## Benefícios da Nova Estrutura

### ✅ Manutenibilidade
- Cada menu em arquivo separado (fácil de encontrar e editar)
- Responsabilidades bem definidas
- Código mais limpo e organizado

### ✅ Expansibilidade
- Adicionar novos menus é simples (criar novo arquivo + adicionar flag no orquestrador)
- Modificar um menu não afeta os outros
- Fácil personalização visual por menu

### ✅ Compatibilidade
- **100% compatível** com código existente
- `ProceduralMaze` continua chamando `MazeMenu.RenderMenu()` normalmente
- Nenhuma mudança necessária em outros scripts

## Como Adicionar um Novo Menu

1. **Criar novo arquivo** (ex: `MazeInventoryMenu.cs`)
2. **Adicionar flag no MazeMenu.cs**:
   ```csharp
   private static bool showInventory = false;
   ```
3. **Adicionar lógica no DrawMenu()**:
   ```csharp
   if (showInventory)
   {
       MazeInventoryMenu.Draw();
       return;
   }
   ```
4. **Adicionar métodos de controle**:
   ```csharp
   public static void ShowInventory() => showInventory = true;
   public static void HideInventory() => showInventory = false;
   ```

## Exemplo de Uso

```csharp
// No ProceduralMaze.cs - continua funcionando normalmente
MazeMenu.RenderMenu(this);

// Para navegar entre menus
MazeMenu.ShowRanking();  // Mostra ranking
MazeMenu.ShowStats();    // Mostra estatísticas
MazeMenu.HideRanking();  // Volta ao menu principal
```

## Estilos e Personalização

Cada menu pode ter seus próprios estilos visuais:
- Cores diferentes para cada tipo de menu
- Tamanhos de fonte específicos
- Layouts personalizados
- Efeitos visuais únicos

Isso permite criar uma experiência visual rica e diferenciada para cada contexto do jogo. 