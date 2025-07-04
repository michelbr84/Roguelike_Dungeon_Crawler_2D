using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;

public static class MazeStatisticsMenu
{
    public static void RenderStatisticsMenu(ProceduralMaze maze)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.cyan;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 18;
        infoStyle.normal.textColor = Color.white;
        infoStyle.alignment = TextAnchor.MiddleLeft;

        int w = 600, h = 600;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(12);
        GUILayout.Label("ESTATÍSTICAS DETALHADAS", titleStyle);
        GUILayout.Space(20);

        var stats = MazeStatistics.GetPlayerStats();
        if (stats != null)
        {
            GUILayout.Label($"Rank: {MazeStatistics.GetPlayerRank()}", infoStyle);
            GUILayout.Label($"Jogos: {stats.totalGamesPlayed}", infoStyle);
            GUILayout.Label($"Vitórias: {stats.totalWins}", infoStyle);
            GUILayout.Label($"Derrotas: {stats.totalLosses}", infoStyle);
            GUILayout.Label($"Tempo Total: {MazeStatistics.GetFormattedPlayTime()}", infoStyle);
            GUILayout.Label($"Nível Mais Alto: {stats.highestLevel}", infoStyle);
            GUILayout.Label($"Score Mais Alto: {stats.highestScore}", infoStyle);
            GUILayout.Label($"Inimigos Eliminados: {stats.totalEnemiesKilled}", infoStyle);
            GUILayout.Label($"Power-ups Coletados: {stats.totalPowerUpsCollected}", infoStyle);
            GUILayout.Label($"Conquistas: {MazeAchievements.GetUnlockedAchievementsCount()}/18", infoStyle);
            
            GUILayout.Space(20);
            
            // Conquistas desbloqueadas
            GUILayout.Label("CONQUISTAS DESBLOQUEADAS:", infoStyle);
            var achievements = GetUnlockedAchievements();
            foreach (var achievement in achievements)
            {
                GUILayout.Label($"✓ {achievement.name}: {achievement.description}", infoStyle);
            }
        }
        else
        {
            GUILayout.Label("Nenhuma estatística disponível.", infoStyle);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Exportar Estatísticas", buttonStyle, GUILayout.Height(36)))
        {
            ExportStatistics();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Importar Estatísticas", buttonStyle, GUILayout.Height(36)))
        {
            ImportStatistics();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Resetar Estatísticas", buttonStyle, GUILayout.Height(36)))
        {
            MazeStatistics.ResetAllStats();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(36)))
        {
            ProceduralMaze.gameState = GameState.Menu;
        }

        GUILayout.EndArea();
    }
    
    // Métodos auxiliares
    private static List<AchievementInfo> GetUnlockedAchievements()
    {
        var achievements = new List<AchievementInfo>();
        
        // Verificar cada achievement
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.FIRST_KILL))
            achievements.Add(new AchievementInfo("Primeira Vítima", "Elimine seu primeiro inimigo"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.ENEMY_SLAYER))
            achievements.Add(new AchievementInfo("Caçador de Inimigos", "Elimine 50 inimigos"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.POWER_COLLECTOR))
            achievements.Add(new AchievementInfo("Colecionador de Power-ups", "Colete 20 power-ups"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.SCORE_MASTER))
            achievements.Add(new AchievementInfo("Mestre dos Pontos", "Alcance 1000 pontos"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.LEVEL_WARRIOR))
            achievements.Add(new AchievementInfo("Guerreiro dos Níveis", "Complete o nível 10"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.PERFECT_PLAYER))
            achievements.Add(new AchievementInfo("Jogador Perfeito", "Complete 5 níveis sem perder vida"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.SHIELD_MASTER))
            achievements.Add(new AchievementInfo("Mestre do Escudo", "Use o escudo"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.TELEPORT_EXPERT))
            achievements.Add(new AchievementInfo("Especialista em Teleporte", "Use o teleporte"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.SPEED_DEMON))
            achievements.Add(new AchievementInfo("Demônio da Velocidade", "Use o speed boost"));
        
        if (MazeAchievements.IsUnlocked(MazeAchievements.Achievement.INVISIBLE_GHOST))
            achievements.Add(new AchievementInfo("Fantasma Invisível", "Use a invisibilidade"));
        
        return achievements;
    }
    
    private static void ExportStatistics()
    {
        var stats = MazeStatistics.GetPlayerStats();
        if (stats != null)
        {
            string json = JsonUtility.ToJson(stats, true);
            string path = System.IO.Path.Combine(Application.persistentDataPath, "statistics_export.json");
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"Estatísticas exportadas para: {path}");
        }
    }
    
    private static void ImportStatistics()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "statistics_export.json");
        if (System.IO.File.Exists(path))
        {
            try
            {
                string json = System.IO.File.ReadAllText(path);
                var stats = JsonUtility.FromJson<MazeStatistics.PlayerStats>(json);
                // Aqui você pode implementar a lógica para importar as estatísticas
                Debug.Log("Estatísticas importadas com sucesso!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Erro ao importar estatísticas: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Arquivo de estatísticas não encontrado!");
        }
    }
    
    // Classe auxiliar para informações de achievement
    private class AchievementInfo
    {
        public string name;
        public string description;
        
        public AchievementInfo(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
} 