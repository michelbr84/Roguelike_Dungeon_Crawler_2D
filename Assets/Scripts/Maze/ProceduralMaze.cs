using UnityEngine;
using System.Collections.Generic;

public class ProceduralMaze : MonoBehaviour
{
    [Header("Debug & Configuração Rápida")]
    [Tooltip("Iniciar o jogo com todos os power-ups ativos para testes.")]
    public bool startWithAllPowerUps = false;
    [Tooltip("Ativar modo debug (mostra informações extras na tela).")]
    public bool debugMode = false;
    [Tooltip("Pular tutorial ao iniciar.")]
    public bool skipTutorial = false;
    [Space]

    [Header("Sistema de Clima (Weather)")]
    [Tooltip("Ativar sistema de clima dinâmico.")]
    public bool enableWeatherSystem = true;
    [Tooltip("Tipos de clima disponíveis no jogo.")]
    public List<string> weatherTypes = new List<string> { "Clear", "Rain", "Fog", "Storm", "Snow", "Sandstorm", "Darkness", "Wind" };
    [Tooltip("Duração mínima de cada clima (segundos).")]
    [Min(1)] public float minWeatherDuration = 30f;
    [Tooltip("Duração máxima de cada clima (segundos).")]
    [Min(1)] public float maxWeatherDuration = 90f;
    [Space]

    [Header("Sistema de Pets/Companions")]
    [Tooltip("Ativar sistema de pets/companions.")]
    public bool enablePetSystem = true;
    [Tooltip("Lista de pets disponíveis para o jogador.")]
    public List<string> availablePets = new List<string> { "Wolf", "Fairy", "Golem", "Dragon", "Robot" };
    [Tooltip("Pet inicial do jogador (deixe vazio para nenhum).")]
    public string startingPet = "";
    [Space]

    [Header("Sistema de Crafting")]
    [Tooltip("Ativar sistema de crafting de equipamentos.")]
    public bool enableCraftingSystem = true;
    [Tooltip("Recursos iniciais do jogador para crafting.")]
    public int startingIron = 0;
    public int startingGold = 0;
    public int startingCrystal = 0;
    public int startingEssence = 0;
    public int startingFragment = 0;
    [Space]

    [Header("Sistema de Eventos Aleatórios")]
    [Tooltip("Ativar sistema de eventos aleatórios.")]
    public bool enableEventSystem = true;
    [Tooltip("Chance de evento ocorrer por minuto (0 a 1).")]
    [Range(0f,1f)] public float eventChancePerMinute = 0.2f;
    [Tooltip("Lista de tipos de eventos possíveis.")]
    public List<string> eventTypes = new List<string> { "Treasure", "Ambush", "Merchant", "Trap", "Blessing", "Curse", "Puzzle", "Boss", "Rescue", "SecretRoom" };
    [Space]

    // Melhorias em campos existentes
    [Header("Maze Settings")]
    [Tooltip("Largura do labirinto (número de células).")]
    [Min(5)] public int width = 10;
    [Tooltip("Altura do labirinto (número de células).")]
    [Min(5)] public int height = 10;
    [Tooltip("Probabilidade de uma célula ser parede (0 = sem paredes, 1 = só paredes).")]
    [Range(0f, 1f)] public float wallProbability = 0.2f;
    [Space]

    [Header("Movement Keys")]
    [Tooltip("Tecla para mover para cima.")]
    public KeyCode upKey = KeyCode.UpArrow;
    [Tooltip("Tecla para mover para baixo.")]
    public KeyCode downKey = KeyCode.DownArrow;
    [Tooltip("Tecla para mover para a esquerda.")]
    public KeyCode leftKey = KeyCode.LeftArrow;
    [Tooltip("Tecla para mover para a direita.")]
    public KeyCode rightKey = KeyCode.RightArrow;
    [Tooltip("Tecla para atirar.")]
    public KeyCode shootKey = KeyCode.Space;
    [Tooltip("Tecla para teleporte.")]
    public KeyCode teleportKey = KeyCode.T;
    [Space]

    [Header("Axis Multipliers (1 = frente, -1 = trás)")]
    [Tooltip("Horizontal: 1 = Para onde o jogador OLHA, -1 = Oposto ao que o jogador olha")]
    public int horizontalMultiplier = -1;
    [Tooltip("Vertical: 1 = Para onde o jogador OLHA, -1 = Oposto ao que o jogador olha")]
    public int verticalMultiplier = 1;
    [Space]

    [Header("Visual Settings")]
    [Tooltip("Cor de fundo do labirinto.")]
    public Color backgroundColor = new Color(0.12f, 0.12f, 0.12f, 1f);
    [Tooltip("Cor das paredes.")]
    public Color wallColor = Color.gray;
    [Tooltip("Cor do jogador.")]
    public Color playerColor = Color.green;
    [Tooltip("Cor da saída.")]
    public Color exitColor = Color.red;
    [Tooltip("Cor dos inimigos.")]
    public Color enemyColor = Color.magenta;
    [Tooltip("Cor dos projéteis.")]
    public Color bulletColor = Color.yellow;
    [Space]

    // Texturas do Maze (Cell/Objects)
    public Texture2D backgroundTexture;
    public Texture2D wallTexture;
    public Texture2D playerTexture;
    public Texture2D exitTexture;
    public Texture2D enemyTexture;
    public Texture2D bulletTexture;

    // Power-ups sprites (coletáveis no maze)
    public Texture2D ammoTexture;
    public Texture2D extraLifeTexture;
    public Texture2D shieldTexture;

    [Header("Power-Up Textures")]
    public Texture2D doubleShotTexture;
    public Texture2D tripleShotTexture;
    public Texture2D speedBoostTexture;
    public Texture2D invisibilityTexture;
    public Texture2D teleportTexture;
    public Texture2D scoreBoosterTexture;

    // HUD Icons
    [Header("HUD Icons")]
    public Texture2D lifeIcon;
    public Texture2D ammoIcon;
    public Texture2D shieldIcon;

    [Header("Enemies & Bullets")]
    public int enemyCount = 3;
    public float enemyMoveInterval = 0.8f;

    // Maze
    public int[,] maze;
    public Vector2Int playerPos;
    public Vector2Int exitPos;
    public Vector2Int playerDir = Vector2Int.down;

    // Enemies & Bullets
    public List<Vector2Int> enemies = new List<Vector2Int>();
    public float enemyMoveTimer = 0f;

    public class Bullet
    {
        public Vector2Int pos;
        public Vector2Int dir;
        public Bullet(Vector2Int pos, Vector2Int dir) { this.pos = pos; this.dir = dir; }
    }
    public List<Bullet> bullets = new List<Bullet>();

    // PowerUps
    public List<PowerUp> powerUps = new List<PowerUp>();

    // Score & Level & Record
    [HideInInspector] public int score = 0;
    [HideInInspector] public int currentLevel = 1;
    [HideInInspector] public int scoreRecord = 0;

    // VIDAS
    [HideInInspector] public int lives = 3;
    public int startingLives = 3;

    // MUNIÇÃO
    [HideInInspector] public int ammo = 10;
    public int startingAmmo = 10;
    public int maxAmmo = 25;

    // --- ESCUDO (Shield) ---
    [Header("Shield Power-Up")]
    [HideInInspector] public bool shieldActive = false;
    [HideInInspector] public float shieldTimer = 0f;
    public float shieldDuration = 5f;

    // --- DOUBLE SHOT ---
    [Header("Double Shot Power-Up")]
    [HideInInspector] public bool doubleShotActive = false;
    [HideInInspector] public float doubleShotTimer = 0f;
    public float doubleShotDuration = 7f;

    // --- TRIPLE SHOT ---
    [Header("Triple Shot Power-Up")]
    [HideInInspector] public bool tripleShotActive = false;
    [HideInInspector] public float tripleShotTimer = 0f;
    public float tripleShotDuration = 5f;

    // --- SPEED BOOST ---
    [Header("Speed Boost Power-Up")]
    [HideInInspector] public bool speedBoostActive = false;
    [HideInInspector] public float speedBoostTimer = 0f;
    public float speedBoostDuration = 6f;
    public float speedBoostMultiplier = 1.7f;

    // --- INVISIBILITY ---
    [Header("Invisibility Power-Up")]
    [HideInInspector] public bool invisibilityActive = false;
    [HideInInspector] public float invisibilityTimer = 0f;
    public float invisibilityDuration = 5f;

    // --- TELEPORT ---
    [Header("Teleport Power-Up")]
    [HideInInspector] public bool teleportAvailable = false;

    // --- SCORE BOOSTER ---
    [Header("Score Booster Power-Up")]
    [HideInInspector] public bool scoreBoosterActive = false;
    [HideInInspector] public float scoreBoosterTimer = 0f;
    public float scoreBoosterDuration = 8f;
    public float scoreBoosterMultiplier = 2f;

    // Efeito visual breve ao coletar power-up
    [HideInInspector] public float powerUpFlashTimer = 0f;
    public float powerUpFlashDuration = 0.3f;

    // GameState integration
    public static GameState gameState = GameState.Menu;
    private static ProceduralMaze instance;

    // --- Multiplicadores de Dificuldade ---
    [Header("Multiplicadores de Dificuldade")]
    public float enemySpeedMultiplier = 1.0f;
    public float powerUpSpawnRate = 1.0f;
    public float playerSpeedMultiplier = 1.0f;
    public float bulletSpeedMultiplier = 1.0f;

    [Header("Texturas de Inimigos Avançados")]
    [Tooltip("Textura do Boss.")]
    public Texture2D bossTexture;
    [Tooltip("Textura do Sniper.")]
    public Texture2D sniperTexture;
    [Tooltip("Textura do Kamikaze.")]
    public Texture2D kamikazeTexture;
    [Tooltip("Textura do Spawner.")]
    public Texture2D spawnerTexture;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Inicializar sistemas
        MazeDebugSystem.Initialize();
        MazeStatistics.Initialize();
        
        // Carregar configurações
        LoadSettings();
        
        // Inicializar jogo
        InitializeGame();
    }

    void Update()
    {
        if (gameState == GameState.Playing)
        {
            MazeInputHandler.HandleGameInput(this);
            MazeGameplayLoop.UpdateGameplay(this);
            
            // Atualizar novos sistemas
            MazeWeatherSystem.Update(Time.deltaTime);
            MazeEventSystem.Update(Time.deltaTime);
            MazePetSystem.UpdatePets(this, Time.deltaTime);
        }
        else if (gameState == GameState.Menu)
        {
            MazeInputHandler.HandleMenuInput();
        }
        else if (gameState == GameState.Tutorial)
        {
            MazeTutorial.HandleTutorialInput();
        }
        else if (gameState == GameState.GameOver)
        {
            MazeInputHandler.HandleGameOverInput();
        }
        // Garante que ao sair das configurações, retorna para o menu de pausa
        else if (gameState == GameState.Settings && !MazeSettingsMenu.IsVisible())
        {
            gameState = GameState.Paused;
        }
    }

    private void OnGUI()
    {
        // Renderizar informações de debug
        MazeDebugSystem.RenderDebugInfo(this);
        
        // Controles de debug
        if (MazeDebugSystem.IsDebugMode())
        {
            HandleDebugInput();
        }
        
        // Renderizar menus baseados no estado
        switch (gameState)
        {
            case GameState.Menu:
                MazeMenu.RenderMenu(this);
                break;
            case GameState.Playing:
                MazeRendering.DrawMaze(this);
                MazeHUD.RenderHUD(this);
                break;
            case GameState.Paused:
                MazeMenu.RenderPauseMenu(this);
                break;
            case GameState.GameOver:
                MazeMenu.RenderGameOverMenu(this);
                break;
            case GameState.Victory:
                MazeMenu.RenderVictoryMenu(this);
                break;
            case GameState.Settings:
                MazeSettingsMenu.RenderSettingsMenu(this);
                break;
            case GameState.Statistics:
                MazeStatisticsMenu.RenderStatisticsMenu(this);
                break;
            case GameState.Tutorial:
                MazeTutorial.RenderTutorial(this);
                break;
            case GameState.ClassSelection:
                MazeClassSelectionMenu.RenderMenu();
                break;
        }
    }
    
    private void HandleDebugInput()
    {
        // Teclas de debug
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MazeDebugSystem.ToggleDebugInfo();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            // Pular para próximo nível
            NextLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            // Adicionar vida
            lives++;
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            // Adicionar munição
            ammo += 10;
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // Adicionar score
            score += 1000;
        }
        
        if (Input.GetKeyDown(KeyCode.F6))
        {
            // Validar sistema
            MazeDebugSystem.ValidateSystem();
        }
    }

    // *** DELEGAÇÃO PARA MazeGameLoop.cs ***
    public static void StartGameFromMenu() => MazeGameLoop.StartGameFromMenu();
    public static void RestartGameFromGameOver() => MazeGameLoop.RestartGameFromGameOver();
    public void NextLevel() => MazeGameLoop.NextLevel(this);
    public void LoseLifeAndRespawn() => MazeGameLoop.LoseLifeAndRespawn(this);
    public void GameOver() => MazeGameLoop.GameOver(this);
    public void CollectPowerUp(PowerUpType type) => MazeGameLoop.CollectPowerUp(this, type);
    
    // Carregar configurações
    private void LoadSettings()
    {
        var settings = MazeSaveSystem.LoadSettings();
        if (settings != null)
        {
            // Aplicar configurações carregadas
            if (AudioManager.Instance)
            {
                AudioManager.Instance.SetMusicVolume(settings.musicVolume);
                AudioManager.Instance.SetSFXVolume(settings.sfxVolume);
            }
            
            // Aplicar dificuldade
            MazeDifficultySystem.SetDifficulty((MazeDifficultySystem.DifficultyLevel)settings.difficultyLevel);
        }
    }
    
    // Inicializar jogo
    private void InitializeGame()
    {
        // Inicializar sistemas
        MazeInitialization.InitializeGame(this);
        
        // Inicializar sistemas de classe e equipamentos
        MazeCharacterSystem.Initialize();
        MazeEquipmentSystem.Initialize();
        
        // Aplicar estatísticas de classe
        MazeCharacterSystem.ApplyClassStats(this);
        MazeCharacterSystem.ApplySpecialAbility(this);
        
        // Aplicar equipamentos
        MazeEquipmentSystem.ApplyEquipmentStats(this);
        MazeEquipmentSystem.ApplySpecialEffects(this);
        
        // Carregar recorde
        scoreRecord = PlayerPrefs.GetInt("ScoreRecord", 0);
        
        // Verificar se deve mostrar tutorial
        if (MazeTutorial.ShouldShowTutorial())
        {
            gameState = GameState.Tutorial;
        }
        else
        {
            gameState = GameState.Menu;
        }
    }
    
    // Métodos de compatibilidade
    public static void RestartGame()
    {
        if (instance != null)
        {
            instance.LoseLifeAndRespawn();
        }
    }
    
    public static void ReturnToMenu()
    {
        gameState = GameState.Menu;
    }
}
 