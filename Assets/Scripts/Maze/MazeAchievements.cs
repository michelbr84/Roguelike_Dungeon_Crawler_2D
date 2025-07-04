using UnityEngine;
using System.Collections.Generic;

public static class MazeAchievements
{
    // Lista de achievements desbloqueados
    private static HashSet<string> unlockedAchievements = new HashSet<string>();
    
    // Estatísticas do jogador
    private static int totalEnemiesKilled = 0;
    private static int totalPowerUpsCollected = 0;
    private static int totalScore = 0;
    private static int highestLevel = 0;
    private static int perfectLevels = 0; // Níveis completados sem perder vida
    
    public static class Achievement
    {
        public const string FIRST_KILL = "first_kill";
        public const string ENEMY_SLAYER = "enemy_slayer";
        public const string POWER_COLLECTOR = "power_collector";
        public const string SCORE_MASTER = "score_master";
        public const string LEVEL_WARRIOR = "level_warrior";
        public const string PERFECT_PLAYER = "perfect_player";
        public const string SHIELD_MASTER = "shield_master";
        public const string TELEPORT_EXPERT = "teleport_expert";
        public const string SPEED_DEMON = "speed_demon";
        public const string INVISIBLE_GHOST = "invisible_ghost";
        public const string MISSION_MASTER = "mission_master";
        public const string BOSS_SLAYER = "boss_slayer";
        public const string SURVIVOR = "survivor";
        public const string SPEED_RUNNER = "speed_runner";
        public const string PACIFIST = "pacifist";
        public const string COLLECTOR = "collector";
        public const string DIFFICULTY_MASTER = "difficulty_master";
        public const string PERFECTIONIST = "perfectionist";
    }
    
    // Inicializar sistema de achievements
    public static void Initialize()
    {
        LoadAchievements();
    }
    
    // Carregar achievements salvos
    private static void LoadAchievements()
    {
        string savedAchievements = PlayerPrefs.GetString("MazeAchievements", "");
        if (!string.IsNullOrEmpty(savedAchievements))
        {
            string[] achievements = savedAchievements.Split(',');
            foreach (string achievement in achievements)
            {
                if (!string.IsNullOrEmpty(achievement))
                    unlockedAchievements.Add(achievement);
            }
        }
        
        // Carregar estatísticas
        totalEnemiesKilled = PlayerPrefs.GetInt("TotalEnemiesKilled", 0);
        totalPowerUpsCollected = PlayerPrefs.GetInt("TotalPowerUpsCollected", 0);
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        highestLevel = PlayerPrefs.GetInt("HighestLevel", 0);
        perfectLevels = PlayerPrefs.GetInt("PerfectLevels", 0);
    }
    
    // Salvar achievements
    private static void SaveAchievements()
    {
        string achievementsString = string.Join(",", unlockedAchievements);
        PlayerPrefs.SetString("MazeAchievements", achievementsString);
        PlayerPrefs.Save();
    }
    
    // Verificar se achievement está desbloqueado
    public static bool IsUnlocked(string achievementId)
    {
        return unlockedAchievements.Contains(achievementId);
    }
    
    // Desbloquear achievement
    private static void UnlockAchievement(string achievementId, string message)
    {
        if (!unlockedAchievements.Contains(achievementId))
        {
            unlockedAchievements.Add(achievementId);
            SaveAchievements();
            MazeHUD.ShowStatusMessage($"Achievement: {message}!");
        }
    }
    
    // Evento: Inimigo morto
    public static void OnEnemyKilled()
    {
        totalEnemiesKilled++;
        PlayerPrefs.SetInt("TotalEnemiesKilled", totalEnemiesKilled);
        PlayerPrefs.Save();
        
        // Primeira morte
        if (totalEnemiesKilled == 1)
        {
            UnlockAchievement(Achievement.FIRST_KILL, "Primeira Vítima");
        }
        
        // 50 inimigos mortos
        if (totalEnemiesKilled >= 50)
        {
            UnlockAchievement(Achievement.ENEMY_SLAYER, "Caçador de Inimigos");
        }
    }
    
    // Evento: Power-up coletado
    public static void OnPowerUpCollected()
    {
        totalPowerUpsCollected++;
        PlayerPrefs.SetInt("TotalPowerUpsCollected", totalPowerUpsCollected);
        PlayerPrefs.Save();
        
        // 20 power-ups coletados
        if (totalPowerUpsCollected >= 20)
        {
            UnlockAchievement(Achievement.POWER_COLLECTOR, "Colecionador de Power-ups");
        }
    }
    
    // Evento: Score atualizado
    public static void OnScoreUpdated(int newScore)
    {
        totalScore = Mathf.Max(totalScore, newScore);
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
        
        // 1000 pontos
        if (totalScore >= 1000)
        {
            UnlockAchievement(Achievement.SCORE_MASTER, "Mestre dos Pontos");
        }
    }
    
    // Evento: Nível completado
    public static void OnLevelCompleted(int level, bool perfect = false)
    {
        highestLevel = Mathf.Max(highestLevel, level);
        PlayerPrefs.SetInt("HighestLevel", highestLevel);
        
        if (perfect)
        {
            perfectLevels++;
            PlayerPrefs.SetInt("PerfectLevels", perfectLevels);
        }
        
        PlayerPrefs.Save();
        
        // Nível 10
        if (highestLevel >= 10)
        {
            UnlockAchievement(Achievement.LEVEL_WARRIOR, "Guerreiro dos Níveis");
        }
        
        // 5 níveis perfeitos
        if (perfectLevels >= 5)
        {
            UnlockAchievement(Achievement.PERFECT_PLAYER, "Jogador Perfeito");
        }
    }
    
    // Evento: Escudo usado
    public static void OnShieldUsed()
    {
        UnlockAchievement(Achievement.SHIELD_MASTER, "Mestre do Escudo");
    }
    
    // Evento: Teleport usado
    public static void OnTeleportUsed()
    {
        UnlockAchievement(Achievement.TELEPORT_EXPERT, "Especialista em Teleporte");
    }
    
    // Evento: Speed boost usado
    public static void OnSpeedBoostUsed()
    {
        UnlockAchievement(Achievement.SPEED_DEMON, "Demônio da Velocidade");
    }
    
    // Evento: Invisibilidade usada
    public static void OnInvisibilityUsed()
    {
        UnlockAchievement(Achievement.INVISIBLE_GHOST, "Fantasma Invisível");
    }
    
    // Obter estatísticas
    public static int GetTotalEnemiesKilled() => totalEnemiesKilled;
    public static int GetTotalPowerUpsCollected() => totalPowerUpsCollected;
    public static int GetTotalScore() => totalScore;
    public static int GetHighestLevel() => highestLevel;
    public static int GetPerfectLevels() => perfectLevels;
    public static int GetUnlockedAchievementsCount() => unlockedAchievements.Count;
    
    // Métodos para compatibilidade com MazeSaveSystem
    public static int GetTotalKills() => totalEnemiesKilled;
    public static int GetTotalPowerUps() => totalPowerUpsCollected;
    
    // Serializar achievements
    public static string SerializeAchievements()
    {
        return string.Join(",", unlockedAchievements);
    }
    
    // Deserializar achievements
    public static void DeserializeAchievements(string achievementsString)
    {
        if (string.IsNullOrEmpty(achievementsString))
            return;
            
        unlockedAchievements.Clear();
        string[] achievements = achievementsString.Split(',');
        foreach (string achievement in achievements)
        {
            if (!string.IsNullOrEmpty(achievement))
                unlockedAchievements.Add(achievement);
        }
    }
    
    // Evento: Missão completada
    public static void OnMissionCompleted()
    {
        // Achievement para completar missões
        int completedMissions = GetUnlockedAchievementsCount();
        if (completedMissions >= 10)
        {
            UnlockAchievement("mission_master", "Mestre das Missões");
        }
    }
    
    // Resetar todas as estatísticas (para debug)
    public static void ResetAllStats()
    {
        unlockedAchievements.Clear();
        totalEnemiesKilled = 0;
        totalPowerUpsCollected = 0;
        totalScore = 0;
        highestLevel = 0;
        perfectLevels = 0;
        
        PlayerPrefs.DeleteKey("MazeAchievements");
        PlayerPrefs.DeleteKey("TotalEnemiesKilled");
        PlayerPrefs.DeleteKey("TotalPowerUpsCollected");
        PlayerPrefs.DeleteKey("TotalScore");
        PlayerPrefs.DeleteKey("HighestLevel");
        PlayerPrefs.DeleteKey("PerfectLevels");
        PlayerPrefs.Save();
    }
} 