using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public static class MazeStatistics
{
    // Estatísticas gerais
    [Serializable]
    public class PlayerStats
    {
        public int totalGamesPlayed;
        public int totalWins;
        public int totalLosses;
        public float totalPlayTime;
        public int totalScore;
        public int highestScore;
        public int highestLevel;
        public int totalEnemiesKilled;
        public int totalPowerUpsCollected;
        public int totalDeaths;
        public int totalShotsFired;
        public int totalShotsHit;
        public int totalTeleportsUsed;
        public int totalShieldsUsed;
        public int totalSpeedBoostsUsed;
        public int totalInvisibilityUsed;
        public int totalPerfectLevels;
        public int totalBossesKilled;
        public int totalMissionsCompleted;
        public float averageScorePerGame;
        public float averageLevelPerGame;
        public float accuracyRate;
        public float survivalRate;
        public DateTime firstPlayDate;
        public DateTime lastPlayDate;
        
        public PlayerStats()
        {
            totalGamesPlayed = 0;
            totalWins = 0;
            totalLosses = 0;
            totalPlayTime = 0f;
            totalScore = 0;
            highestScore = 0;
            highestLevel = 0;
            totalEnemiesKilled = 0;
            totalPowerUpsCollected = 0;
            totalDeaths = 0;
            totalShotsFired = 0;
            totalShotsHit = 0;
            totalTeleportsUsed = 0;
            totalShieldsUsed = 0;
            totalSpeedBoostsUsed = 0;
            totalInvisibilityUsed = 0;
            totalPerfectLevels = 0;
            totalBossesKilled = 0;
            totalMissionsCompleted = 0;
            averageScorePerGame = 0f;
            averageLevelPerGame = 0f;
            accuracyRate = 0f;
            survivalRate = 0f;
            firstPlayDate = DateTime.Now;
            lastPlayDate = DateTime.Now;
        }
    }
    
    // Estatísticas da sessão atual
    [Serializable]
    public class SessionStats
    {
        public float sessionStartTime;
        public float currentSessionTime;
        public int sessionScore;
        public int sessionLevel;
        public int sessionKills;
        public int sessionPowerUps;
        public int sessionDeaths;
        public int sessionShotsFired;
        public int sessionShotsHit;
        public int sessionTeleports;
        public int sessionShields;
        public int sessionSpeedBoosts;
        public int sessionInvisibility;
        public bool isPerfectLevel;
        public int killStreak;
        public int maxKillStreak;
        
        public SessionStats()
        {
            sessionStartTime = Time.time;
            currentSessionTime = 0f;
            sessionScore = 0;
            sessionLevel = 1;
            sessionKills = 0;
            sessionPowerUps = 0;
            sessionDeaths = 0;
            sessionShotsFired = 0;
            sessionShotsHit = 0;
            sessionTeleports = 0;
            sessionShields = 0;
            sessionSpeedBoosts = 0;
            sessionInvisibility = 0;
            isPerfectLevel = true;
            killStreak = 0;
            maxKillStreak = 0;
        }
    }
    
    // Estatísticas atuais
    private static PlayerStats playerStats;
    private static SessionStats sessionStats;
    
    // Inicializar sistema
    public static void Initialize()
    {
        LoadPlayerStats();
        sessionStats = new SessionStats();
    }
    
    // Carregar estatísticas do jogador
    private static void LoadPlayerStats()
    {
        string statsJson = PlayerPrefs.GetString("PlayerStats", "");
        if (!string.IsNullOrEmpty(statsJson))
        {
            try
            {
                playerStats = JsonUtility.FromJson<PlayerStats>(statsJson);
            }
            catch
            {
                playerStats = new PlayerStats();
            }
        }
        else
        {
            playerStats = new PlayerStats();
        }
    }
    
    // Salvar estatísticas do jogador
    private static void SavePlayerStats()
    {
        if (playerStats != null)
        {
            string statsJson = JsonUtility.ToJson(playerStats);
            PlayerPrefs.SetString("PlayerStats", statsJson);
            PlayerPrefs.Save();
        }
    }
    
    // Atualizar estatísticas da sessão
    public static void UpdateSessionStats()
    {
        if (sessionStats != null)
        {
            sessionStats.currentSessionTime = Time.time - sessionStats.sessionStartTime;
        }
    }
    
    // Eventos para atualizar estatísticas
    
    public static void OnGameStarted()
    {
        sessionStats = new SessionStats();
        playerStats.totalGamesPlayed++;
        playerStats.lastPlayDate = DateTime.Now;
        SavePlayerStats();
    }
    
    public static void OnGameWon(int finalScore, int finalLevel)
    {
        playerStats.totalWins++;
        playerStats.totalScore += finalScore;
        playerStats.highestScore = Mathf.Max(playerStats.highestScore, finalScore);
        playerStats.highestLevel = Mathf.Max(playerStats.highestLevel, finalLevel);
        
        // Atualizar médias
        playerStats.averageScorePerGame = (float)playerStats.totalScore / playerStats.totalGamesPlayed;
        playerStats.averageLevelPerGame = (float)playerStats.highestLevel / playerStats.totalGamesPlayed;
        
        SavePlayerStats();
    }
    
    public static void OnGameLost()
    {
        playerStats.totalLosses++;
        SavePlayerStats();
    }
    
    public static void OnEnemyKilled()
    {
        playerStats.totalEnemiesKilled++;
        sessionStats.sessionKills++;
        sessionStats.killStreak++;
        sessionStats.maxKillStreak = Mathf.Max(sessionStats.maxKillStreak, sessionStats.killStreak);
        SavePlayerStats();
    }
    
    public static void OnPlayerDeath()
    {
        playerStats.totalDeaths++;
        sessionStats.sessionDeaths++;
        sessionStats.killStreak = 0;
        sessionStats.isPerfectLevel = false;
        SavePlayerStats();
    }
    
    public static void OnPowerUpCollected()
    {
        playerStats.totalPowerUpsCollected++;
        sessionStats.sessionPowerUps++;
        SavePlayerStats();
    }
    
    public static void OnShotFired()
    {
        playerStats.totalShotsFired++;
        sessionStats.sessionShotsFired++;
        SavePlayerStats();
    }
    
    public static void OnShotHit()
    {
        playerStats.totalShotsHit++;
        sessionStats.sessionShotsHit++;
        
        // Atualizar taxa de precisão
        if (playerStats.totalShotsFired > 0)
        {
            playerStats.accuracyRate = (float)playerStats.totalShotsHit / playerStats.totalShotsFired;
        }
        
        SavePlayerStats();
    }
    
    public static void OnTeleportUsed()
    {
        playerStats.totalTeleportsUsed++;
        sessionStats.sessionTeleports++;
        SavePlayerStats();
    }
    
    public static void OnShieldUsed()
    {
        playerStats.totalShieldsUsed++;
        sessionStats.sessionShields++;
        SavePlayerStats();
    }
    
    public static void OnSpeedBoostUsed()
    {
        playerStats.totalSpeedBoostsUsed++;
        sessionStats.sessionSpeedBoosts++;
        SavePlayerStats();
    }
    
    public static void OnInvisibilityUsed()
    {
        playerStats.totalInvisibilityUsed++;
        sessionStats.sessionInvisibility++;
        SavePlayerStats();
    }
    
    public static void OnPerfectLevel()
    {
        playerStats.totalPerfectLevels++;
        SavePlayerStats();
    }
    
    public static void OnBossKilled()
    {
        playerStats.totalBossesKilled++;
        SavePlayerStats();
    }
    
    public static void OnMissionCompleted()
    {
        playerStats.totalMissionsCompleted++;
        SavePlayerStats();
    }
    
    public static void OnLevelCompleted(int level, int score)
    {
        sessionStats.sessionLevel = level;
        sessionStats.sessionScore = score;
        
        if (sessionStats.isPerfectLevel)
        {
            OnPerfectLevel();
        }
        
        SavePlayerStats();
    }
    
    // Obter estatísticas do jogador
    public static PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
    
    // Obter estatísticas da sessão
    public static SessionStats GetSessionStats()
    {
        return sessionStats;
    }
    
    // Obter estatísticas formatadas para exibição
    public static string GetFormattedPlayTime()
    {
        if (playerStats == null) return "0h 0m";
        
        int totalMinutes = Mathf.FloorToInt(playerStats.totalPlayTime / 60f);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        
        return $"{hours}h {minutes}m";
    }
    
    public static string GetFormattedAccuracy()
    {
        if (playerStats == null || playerStats.totalShotsFired == 0) return "0%";
        
        float accuracy = (float)playerStats.totalShotsHit / playerStats.totalShotsFired * 100f;
        return $"{accuracy:F1}%";
    }
    
    public static string GetFormattedSurvivalRate()
    {
        if (playerStats == null || playerStats.totalGamesPlayed == 0) return "0%";
        
        float survivalRate = (float)playerStats.totalWins / playerStats.totalGamesPlayed * 100f;
        return $"{survivalRate:F1}%";
    }
    
    public static string GetFormattedAverageScore()
    {
        if (playerStats == null) return "0";
        
        return playerStats.averageScorePerGame.ToString("F0");
    }
    
    public static string GetFormattedAverageLevel()
    {
        if (playerStats == null) return "0";
        
        return playerStats.averageLevelPerGame.ToString("F1");
    }
    
    // Obter ranking do jogador
    public static string GetPlayerRank()
    {
        if (playerStats == null) return "Novato";
        
        int totalAchievements = MazeAchievements.GetUnlockedAchievementsCount();
        int totalScore = playerStats.highestScore;
        int totalLevels = playerStats.highestLevel;
        
        if (totalScore >= 50000 && totalLevels >= 50 && totalAchievements >= 15)
            return "Lenda";
        else if (totalScore >= 25000 && totalLevels >= 30 && totalAchievements >= 10)
            return "Mestre";
        else if (totalScore >= 10000 && totalLevels >= 20 && totalAchievements >= 5)
            return "Veterano";
        else if (totalScore >= 5000 && totalLevels >= 10 && totalAchievements >= 3)
            return "Experiente";
        else if (totalScore >= 1000 && totalLevels >= 5)
            return "Iniciante";
        else
            return "Novato";
    }
    
    // Resetar todas as estatísticas
    public static void ResetAllStats()
    {
        playerStats = new PlayerStats();
        sessionStats = new SessionStats();
        SavePlayerStats();
    }
    
    // Exportar estatísticas
    public static string ExportStats()
    {
        if (playerStats == null) return "";
        
        return JsonUtility.ToJson(playerStats, true);
    }

    // Exportar estatísticas para JSON
    public static void ExportStatsToJson(string path)
    {
        if (playerStats == null) return;
        string json = JsonUtility.ToJson(playerStats, true);
        File.WriteAllText(path, json);
    }
    // Importar estatísticas de JSON
    public static void ImportStatsFromJson(string path)
    {
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        var stats = JsonUtility.FromJson<PlayerStats>(json);
        if (stats != null)
        {
            playerStats = stats;
            SavePlayerStats();
        }
    }
} 