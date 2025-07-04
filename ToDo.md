# **Procedural Roguelike Maze – To Do List**

---

## ✅ **Já Implementado**

* [x] **Geração procedural de labirinto 2D** com caminho garantido até a saída
* [x] **Configuração total via Inspector:** tamanho, cor, textura de cada elemento
* [x] **Movimentação do jogador** com teclas customizáveis, suporte a inversão de eixos
* [x] **Direção visual sempre correta** (player mira para onde anda)
* [x] **Sistema de tiro** (seguindo direção do jogador, parametrizável)
* [x] **Spawn e movimentação de inimigos**
* [x] **Sistema completo de colisão:** jogador, inimigos, tiros
* [x] **Morte e respawn do jogador** com reinício do labirinto e inimigos
* [x] **Tiros eliminam inimigos**
* [x] **Chegada à saída avança Level e regenera labirinto**
* [x] **Sistema de pontuação (score), level, e mensagens de status no HUD**
* [x] **HUD completo:** Enemies, Score, Level, Record, mensagens rápidas
* [x] **Sistema de áudio (AudioManager)** totalmente integrado e editável
* [x] **Chamada de sons para todos os eventos principais**
* [x] **Separação do código em múltiplos arquivos e utilitários** (`ProceduralMaze.cs`, `MazeGenerationUtils.cs`, `MazeEnemyUtils.cs`, `MazePowerUpUtils.cs`, `MazePlayerUtils.cs`, `MazeDifficultyUtils.cs`, `MazeRendering.cs`, `MazeHUD.cs`, `AudioManager.cs`)
* [x] **Salvamento automático do recorde** (`PlayerPrefs`)
* [x] **Progressão de fases:** maze maior/frequência de inimigos a cada avanço
* [x] **Menu de início, tela de Game Over, integração GameState**
* [x] **Código modular e expansível** (fácil adicionar features!)

---

## ⬜️ **Para um Jogo Completo/Polido (Próximos Passos)**

### **Gameplay e Balanceamento**

* [ ] **Limite real de vidas:** Game Over só quando perder todas as vidas, não a cada morte.
* [ ] **Sistema de munição limitado:** O jogador deve gerenciar recursos, tornando o desafio maior.
* [ ] **Opções no menu:** Reset de recorde, escolha de dificuldade.
* [ ] **Timer/desafio por tempo** (opcional).

### **Aprimoramento de Inimigos e IA**

* [ ] **IA avançada:** Inimigos seguem, patrulham, fogem de tiros, alternam comportamento.
* [ ] **Variedade de inimigos:** Velocidades/padrões diferentes, “chefões”, inimigos especiais.

### **Power-ups e Itens**

* [ ] **Itens colecionáveis:** Vida extra, munição, teleport, booster de score.
* [ ] **Power-ups temporários:** Tiro duplo, escudo, velocidade, invisibilidade.

### **Visual e Áudio**

* [ ] **Feedbacks visuais:** Animações para morte, vitória, spawn.
* [ ] **Controle de volumes:** Música/SFX customizáveis no menu.
* [ ] **Múltiplos temas de fundo/músicas.**
* [ ] **Partículas para eventos importantes:** tiros, mortes, portal.

### **Acessibilidade e Polimento**

* [ ] **Rebind de teclas no menu (em runtime).**
* [ ] **Adaptação para touch/mobile controls.**
* [ ] **Tutorial breve para iniciantes.**
* [ ] **Feedback claro no HUD ou popups.**
* [ ] **Testes e polimento geral:** bugs de lógica, sons duplicados, spawn/colisão, UX.

### **Extra (Se Quiser Ir Além)**

* [ ] **Exportar para WebGL/Android.**
* [ ] **Sistema de conquistas (Achievements).**
* [ ] **Ranking online (Leaderboard via API).**
* [ ] **Analytics: monitorar como os jogadores jogam.**

---

## **Resumo Visual**

```markdown
## ✅ Feito
- Maze procedural, controles editáveis, sistema de tiro e inimigos, HUD, sons, score/level, recorde, progressão real, menus, modularização.

## ⬜️ Próximos passos
- Sistema de vidas real, munição, IA avançada, power-ups, polimento visual/áudio, acessibilidade, exportação.
```

---

## **Melhor próximo passo (Plano de Execução Detalhado)**

**Implementar sistema de vidas real + tela de Game Over ao perder todas.**

### **Plano:**

1. **Vidas decrementadas apenas ao morrer:**

   * Já implementado: `lives--` e checagem de vidas no método `LoseLifeAndRespawn`.
   * Ao perder vida, apenas reseta o maze; Game Over só se as vidas chegarem a 0.

2. **Tela de Game Over:**

   * Manter GameState e tela de Game Over já implementados.
   * Garantir mensagem clara de Game Over e opção de restart.

3. **HUD sempre mostrando vidas restantes:**

   * Checar e atualizar HUD para mostrar “lives” visualmente.

4. **Balanceamento:**

   * Ajustar quantidade inicial de vidas no Inspector.
   * Testar se está divertido/desafiador (nem muito fácil, nem punitivo).

5. **(Opcional) Permitir power-up de vida extra:**

   * Garantir que o jogador pode ganhar vidas extras como item colecionável.

---

### **Por que esse passo é o melhor agora?**

* **Traz desafio real:** O jogador precisa jogar melhor para avançar.
* **Evita frustração:** Não reinicia o progresso por cada erro.
* **Prepara terreno:** Para power-ups e balanceamento futuro (itens de vida extra fazem sentido).
* **Polimento:** Dá sensação de “arcade clássico” e ritmo bom ao jogo.