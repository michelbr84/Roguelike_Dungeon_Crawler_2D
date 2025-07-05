// MazeStatsMenu.cs
using UnityEngine;

public static class MazeStatsMenu
{
    public static void Draw()
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
        GUILayout.Label("ESTATÍSTICAS", titleStyle);
        GUILayout.Space(20);

        var stats = MazeStatistics.GetPlayerStats();
        if (stats != null)
        {
            GUILayout.Label($"Rank: {MazeStatistics.GetPlayerRank()}", infoStyle);
            GUILayout.Label($"Jogos: {stats.totalGamesPlayed}", infoStyle);
            GUILayout.Label($"Vitórias: {stats.totalWins}", infoStyle);
            GUILayout.Label($"Derrotas: {stats.totalLosses}", infoStyle);
            GUILayout.Label($"Tempo Total: {MazeStatistics.GetFormattedPlayTime()}", infoStyle);
            GUILayout.Label($"Score Máximo: {stats.highestScore}", infoStyle);
            GUILayout.Label($"Nível Máximo: {stats.highestLevel}", infoStyle);
            GUILayout.Label($"Inimigos Mortos: {stats.totalEnemiesKilled}", infoStyle);
            GUILayout.Label($"Power-ups: {stats.totalPowerUpsCollected}", infoStyle);
            GUILayout.Label($"Precisão: {MazeStatistics.GetFormattedAccuracy()}", infoStyle);
            GUILayout.Label($"Taxa de Sobrevivência: {MazeStatistics.GetFormattedSurvivalRate()}", infoStyle);
            GUILayout.Label($"Conquistas: {MazeAchievements.GetUnlockedAchievementsCount()}", infoStyle);
        }
        else
        {
            GUILayout.Label("Nenhuma estatística disponível.", infoStyle);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(36)))
        {
            MazeMenu.HideStats();
        }

        GUILayout.EndArea();
    }
} 