using UnityEngine;

public static class MazeDifficultySystem
{
    // Níveis de dificuldade
    public enum DifficultyLevel
    {
        Easy = 1,
        Normal = 2,
        Hard = 3,
        Expert = 4,
        Nightmare = 5
    }
    
    // Configurações de dificuldade
    [System.Serializable]
    public class DifficultySettings
    {
        public DifficultyLevel level;
        public string name;
        public string description;
        public float enemySpeedMultiplier;
        public float enemySpawnRateMultiplier;
        public float powerUpSpawnRateMultiplier;
        public int startingLives;
        public int startingAmmo;
        public float playerSpeedMultiplier;
        public float bulletSpeedMultiplier;
        public bool advancedEnemiesEnabled;
        public bool bossEnabled;
        public float scoreMultiplier;
        public Color difficultyColor;
        
        public DifficultySettings(DifficultyLevel lvl)
        {
            level = lvl;
            SetDefaultsForLevel(lvl);
        }
        
        private void SetDefaultsForLevel(DifficultyLevel lvl)
        {
            switch (lvl)
            {
                case DifficultyLevel.Easy:
                    name = "Fácil";
                    description = "Ideal para iniciantes";
                    enemySpeedMultiplier = 0.7f;
                    enemySpawnRateMultiplier = 0.6f;
                    powerUpSpawnRateMultiplier = 1.5f;
                    startingLives = 5;
                    startingAmmo = 15;
                    playerSpeedMultiplier = 1.2f;
                    bulletSpeedMultiplier = 1.1f;
                    advancedEnemiesEnabled = false;
                    bossEnabled = false;
                    scoreMultiplier = 0.8f;
                    difficultyColor = Color.green;
                    break;
                    
                case DifficultyLevel.Normal:
                    name = "Normal";
                    description = "Experiência balanceada";
                    enemySpeedMultiplier = 1.0f;
                    enemySpawnRateMultiplier = 1.0f;
                    powerUpSpawnRateMultiplier = 1.0f;
                    startingLives = 3;
                    startingAmmo = 10;
                    playerSpeedMultiplier = 1.0f;
                    bulletSpeedMultiplier = 1.0f;
                    advancedEnemiesEnabled = true;
                    bossEnabled = true;
                    scoreMultiplier = 1.0f;
                    difficultyColor = Color.yellow;
                    break;
                    
                case DifficultyLevel.Hard:
                    name = "Difícil";
                    description = "Para jogadores experientes";
                    enemySpeedMultiplier = 1.3f;
                    enemySpawnRateMultiplier = 1.4f;
                    powerUpSpawnRateMultiplier = 0.7f;
                    startingLives = 2;
                    startingAmmo = 8;
                    playerSpeedMultiplier = 0.9f;
                    bulletSpeedMultiplier = 0.9f;
                    advancedEnemiesEnabled = true;
                    bossEnabled = true;
                    scoreMultiplier = 1.5f;
                    difficultyColor = Color.red;
                    break;
                    
                case DifficultyLevel.Expert:
                    name = "Expert";
                    description = "Desafio extremo";
                    enemySpeedMultiplier = 1.6f;
                    enemySpawnRateMultiplier = 1.8f;
                    powerUpSpawnRateMultiplier = 0.5f;
                    startingLives = 1;
                    startingAmmo = 5;
                    playerSpeedMultiplier = 0.8f;
                    bulletSpeedMultiplier = 0.8f;
                    advancedEnemiesEnabled = true;
                    bossEnabled = true;
                    scoreMultiplier = 2.0f;
                    difficultyColor = Color.magenta;
                    break;
                    
                case DifficultyLevel.Nightmare:
                    name = "Pesadelo";
                    description = "Apenas para os melhores";
                    enemySpeedMultiplier = 2.0f;
                    enemySpawnRateMultiplier = 2.5f;
                    powerUpSpawnRateMultiplier = 0.3f;
                    startingLives = 1;
                    startingAmmo = 3;
                    playerSpeedMultiplier = 0.7f;
                    bulletSpeedMultiplier = 0.7f;
                    advancedEnemiesEnabled = true;
                    bossEnabled = true;
                    scoreMultiplier = 3.0f;
                    difficultyColor = Color.black;
                    break;
            }
        }
    }
    
    // Configurações atuais
    private static DifficultySettings currentDifficulty;
    
    // Inicializar sistema
    public static void Initialize()
    {
        // Carregar dificuldade salva ou usar Normal como padrão
        int savedDifficulty = PlayerPrefs.GetInt("DifficultyLevel", (int)DifficultyLevel.Normal);
        currentDifficulty = new DifficultySettings((DifficultyLevel)savedDifficulty);
    }
    
    // Aplicar configurações de dificuldade ao maze
    public static void ApplyDifficultySettings(ProceduralMaze maze)
    {
        if (currentDifficulty == null)
            Initialize();
            
        // Aplicar configurações básicas
        maze.startingLives = currentDifficulty.startingLives;
        maze.lives = currentDifficulty.startingLives;
        maze.startingAmmo = currentDifficulty.startingAmmo;
        maze.ammo = currentDifficulty.startingAmmo;
        
        // Aplicar multiplicadores
        maze.enemySpeedMultiplier = currentDifficulty.enemySpeedMultiplier;
        maze.powerUpSpawnRate = currentDifficulty.powerUpSpawnRateMultiplier;
        maze.playerSpeedMultiplier = currentDifficulty.playerSpeedMultiplier;
        maze.bulletSpeedMultiplier = currentDifficulty.bulletSpeedMultiplier;
        
        // Configurar inimigos avançados
        if (!currentDifficulty.advancedEnemiesEnabled)
        {
            // Desabilitar inimigos avançados
        }
        
        // Configurar boss
        if (!currentDifficulty.bossEnabled)
        {
            // Desabilitar boss
        }
    }
    
    // Obter configurações atuais
    public static DifficultySettings GetCurrentDifficulty()
    {
        if (currentDifficulty == null)
            Initialize();
        return currentDifficulty;
    }
    
    // Alterar dificuldade
    public static void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = new DifficultySettings(level);
        PlayerPrefs.SetInt("DifficultyLevel", (int)level);
        PlayerPrefs.Save();
    }
    
    // Obter multiplicador de score baseado na dificuldade
    public static float GetScoreMultiplier()
    {
        return currentDifficulty?.scoreMultiplier ?? 1.0f;
    }
    
    // Verificar se inimigos avançados estão habilitados
    public static bool AreAdvancedEnemiesEnabled()
    {
        return currentDifficulty?.advancedEnemiesEnabled ?? true;
    }
    
    // Verificar se boss está habilitado
    public static bool IsBossEnabled()
    {
        return currentDifficulty?.bossEnabled ?? true;
    }
    
    // Obter multiplicador de velocidade dos inimigos
    public static float GetEnemySpeedMultiplier()
    {
        return currentDifficulty?.enemySpeedMultiplier ?? 1.0f;
    }
    
    // Obter multiplicador de spawn de inimigos
    public static float GetEnemySpawnRateMultiplier()
    {
        return currentDifficulty?.enemySpawnRateMultiplier ?? 1.0f;
    }
    
    // Obter multiplicador de spawn de power-ups
    public static float GetPowerUpSpawnRateMultiplier()
    {
        return currentDifficulty?.powerUpSpawnRateMultiplier ?? 1.0f;
    }
    
    // Obter todas as dificuldades disponíveis
    public static DifficultySettings[] GetAllDifficulties()
    {
        return new DifficultySettings[]
        {
            new DifficultySettings(DifficultyLevel.Easy),
            new DifficultySettings(DifficultyLevel.Normal),
            new DifficultySettings(DifficultyLevel.Hard),
            new DifficultySettings(DifficultyLevel.Expert),
            new DifficultySettings(DifficultyLevel.Nightmare)
        };
    }
    
    // Obter próxima dificuldade
    public static DifficultyLevel GetNextDifficulty()
    {
        if (currentDifficulty == null)
            return DifficultyLevel.Normal;
            
        int currentIndex = (int)currentDifficulty.level;
        int nextIndex = Mathf.Min(currentIndex + 1, 5);
        return (DifficultyLevel)nextIndex;
    }
    
    // Obter dificuldade anterior
    public static DifficultyLevel GetPreviousDifficulty()
    {
        if (currentDifficulty == null)
            return DifficultyLevel.Normal;
            
        int currentIndex = (int)currentDifficulty.level;
        int prevIndex = Mathf.Max(currentIndex - 1, 1);
        return (DifficultyLevel)prevIndex;
    }
    
    // Verificar se pode desbloquear próxima dificuldade
    public static bool CanUnlockNextDifficulty(int currentLevel, int currentScore)
    {
        if (currentDifficulty == null)
            return false;
            
        switch (currentDifficulty.level)
        {
            case DifficultyLevel.Easy:
                return currentLevel >= 5;
            case DifficultyLevel.Normal:
                return currentLevel >= 10 && currentScore >= 2000;
            case DifficultyLevel.Hard:
                return currentLevel >= 15 && currentScore >= 5000;
            case DifficultyLevel.Expert:
                return currentLevel >= 20 && currentScore >= 10000;
            case DifficultyLevel.Nightmare:
                return false; // Sempre desbloqueado
            default:
                return false;
        }
    }
    
    // Obter descrição da dificuldade atual
    public static string GetCurrentDifficultyDescription()
    {
        if (currentDifficulty == null)
            return "Normal - Experiência balanceada";
            
        return $"{currentDifficulty.name} - {currentDifficulty.description}";
    }
    
    // Obter cor da dificuldade atual
    public static Color GetCurrentDifficultyColor()
    {
        return currentDifficulty?.difficultyColor ?? Color.yellow;
    }
} 