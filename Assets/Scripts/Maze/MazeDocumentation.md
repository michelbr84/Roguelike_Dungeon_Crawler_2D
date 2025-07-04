# Documentação - Roguelike Dungeon Crawler 2D

## 📋 Visão Geral

Este é um jogo roguelike 2D com geração procedural de labirintos, desenvolvido em Unity. O jogo possui um sistema modular bem organizado que facilita a manutenção e expansão.

## 🎮 Como Jogar

### Controles
- **WASD/Setas**: Movimento do jogador
- **Mouse**: Mirar e atirar
- **Touch (Mobile)**: Joystick virtual + botões
- **ESC**: Pausar/Voltar ao menu
- **F1-F6**: Controles de debug (apenas em editor)

### Objetivo
- Navegue pelo labirinto
- Elimine inimigos
- Colete power-ups
- Encontre a saída para avançar de nível
- Sobreviva o máximo possível

## 🏗️ Arquitetura do Sistema

### Estrutura de Arquivos
```
Scripts/
├── ProceduralMaze.cs          # Classe principal do jogo
├── GameState.cs               # Enum de estados do jogo
├── AudioManager.cs            # Sistema de áudio
├── Maze/
│   ├── MazeGenerationUtils.cs     # Geração de labirintos
│   ├── MazeEnemyUtils.cs          # Sistema de inimigos
│   ├── MazePlayerUtils.cs         # Sistema do jogador
│   ├── MazePowerUpUtils.cs        # Sistema de power-ups
│   ├── MazeDifficultyUtils.cs     # Sistema de dificuldade
│   ├── MazeInitialization.cs      # Inicialização do jogo
│   ├── MazeTutorial.cs            # Sistema de tutorial
│   ├── MazeDebugSystem.cs         # Sistema de debug
│   └── MazeBuildSystem.cs         # Sistema de build
├── Game/
│   ├── UI/
│   │   └── Hud/
│   │       ├── BaseHud.cs         # HUD base
│   │       └── IHud.cs            # Interface HUD
│   └── Controls/
│       └── Joystick.cs            # Controle mobile
└── MazeMenu.cs                # Sistema de menus
```

## 🎵 Sistema de Áudio

### Múltiplas Músicas
O jogo suporta múltiplas músicas de fundo:

1. **Configurar no AudioManager:**
   - Adicione músicas no array `additionalMusic`
   - A música principal vai em `backgroundMusic`

2. **Controles:**
   - Menu de configurações: Botões "Anterior" e "Próxima"
   - Tecla F6: Próxima música (debug)
   - Troca automática baseada na dificuldade

3. **Métodos disponíveis:**
   ```csharp
   AudioManager.Instance.NextMusic();
   AudioManager.Instance.PreviousMusic();
   AudioManager.Instance.SetMusic(index);
   AudioManager.Instance.SetMusicByDifficulty(level);
   ```

## 🐛 Sistema de Debug

### Ativação
- **Editor**: Automaticamente ativo
- **Build**: Desabilitado por padrão

### Controles
- **F1**: Alternar informações de debug
- **F2**: Pular para próximo nível
- **F3**: Adicionar vida
- **F4**: Adicionar munição
- **F5**: Adicionar score
- **F6**: Validar sistema

### Informações Exibidas
- FPS, Estado do jogo, Nível, Score
- Vidas, Munição, Posição do jogador
- Informações do maze, inimigos, power-ups
- Controles de reset (tutorial, stats, ranking)

## 🏗️ Sistema de Build

### Menu de Build (Unity Editor)
- **Build > Build Windows**: Build para Windows
- **Build > Build Android**: Build para Android
- **Build > Build WebGL**: Build para Web
- **Build > Build All Platforms**: Build para todas as plataformas
- **Build > Clean Builds**: Limpar builds antigas
- **Build > Validate Build**: Validar configuração

### Configurações de Qualidade
- **Build > Set High Quality**: Qualidade alta
- **Build > Set Medium Quality**: Qualidade média
- **Build > Set Low Quality**: Qualidade baixa

## 💾 Sistema de Save/Load

### Dados Salvos
- Configurações do jogo
- Estatísticas do jogador
- Conquistas desbloqueadas
- Ranking local
- Progresso do tutorial

### Exportação/Importação
- **JSON**: Formato padrão para backup
- **PlayerPrefs**: Armazenamento local
- **Menu**: Opções de export/import disponíveis

## 🎯 Sistema de Dificuldade

### Níveis Disponíveis
1. **Fácil**: Inimigos lentos, muitos power-ups
2. **Normal**: Balanceamento padrão
3. **Difícil**: Inimigos mais rápidos, menos power-ups
4. **Expert**: Inimigos avançados, IA melhorada
5. **Pesadelo**: Máximo desafio

### Configurações por Nível
- Velocidade dos inimigos
- Frequência de spawn
- Quantidade de power-ups
- Dano dos inimigos
- Música de fundo

## 🏆 Sistema de Conquistas

### Tipos de Conquistas
- **Kills**: Eliminar inimigos
- **Power-ups**: Coletar power-ups
- **Score**: Alcançar pontuações
- **Níveis**: Completar níveis
- **Especiais**: Conquistas únicas

### Progresso
- Tracking automático
- Salvamento persistente
- Exibição no menu de estatísticas

## 📊 Sistema de Estatísticas

### Dados Rastreados
- Tempo total de jogo
- Nível mais alto alcançado
- Score mais alto
- Inimigos eliminados
- Power-ups coletados
- Conquistas desbloqueadas

### Visualização
- Menu de estatísticas detalhado
- Gráficos de progresso
- Comparação com recordes

## 🎮 Sistema de Tutorial

### Passos do Tutorial
1. **Boas-vindas**: Introdução ao jogo
2. **Movimento**: Como se mover
3. **Tiro**: Como atirar
4. **Power-ups**: Como usar power-ups
5. **Objetivo**: Como vencer
6. **Conclusão**: Finalização

### Controles
- **Clique**: Avançar
- **ESC**: Pular tutorial
- **Reset**: Disponível no debug

## 🔧 Configuração

### AudioManager
1. Adicione o script ao GameObject
2. Configure as músicas no array `additionalMusic`
3. Configure os volumes padrão
4. Ajuste as configurações de áudio

### ProceduralMaze
1. Configure o tamanho do maze
2. Ajuste as cores e texturas
3. Configure a dificuldade inicial
4. Ajuste os parâmetros de spawn

### Controles Mobile
1. Configure o joystick no prefab
2. Ajuste a sensibilidade
3. Configure os botões de ação
4. Teste em diferentes dispositivos

## 🚀 Próximos Passos

### Melhorias Sugeridas
1. **Sistema de Classes**: Guerreiro, Arqueiro, Mago
2. **Equipamentos**: Armas, armaduras, acessórios
3. **Crafting**: Criar itens com recursos
4. **Pets**: Companheiros que ajudam
5. **Eventos**: Encontros especiais

### Otimizações
1. **Performance**: Object pooling, LOD
2. **Cache**: Carregamento inteligente
3. **Compressão**: Reduzir tamanho do build
4. **Streaming**: Carregamento progressivo

## 📝 Notas de Desenvolvimento

- O código está bem documentado e modular
- Fácil adição de novas features
- Sistema de debug robusto
- Preparado para múltiplas plataformas
- Base sólida para expansões

## 🐛 Troubleshooting

### Problemas Comuns
1. **Música não toca**: Verificar AudioManager e configurações
2. **Controles não funcionam**: Verificar Input System
3. **Save não funciona**: Verificar PlayerPrefs e permissões
4. **Performance baixa**: Verificar configurações de qualidade

### Debug
- Use F1 para ver informações de debug
- Use F6 para validar o sistema
- Verifique o console para erros
- Teste em diferentes dispositivos 