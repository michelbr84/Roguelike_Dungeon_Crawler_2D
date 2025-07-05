// MazeRankingMenu.cs
using UnityEngine;
using System.Collections.Generic;

public static class MazeRankingMenu
{
    public static void Draw()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.yellow;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle entryStyle = new GUIStyle(GUI.skin.label);
        entryStyle.fontSize = 20;
        entryStyle.normal.textColor = Color.white;
        entryStyle.alignment = TextAnchor.MiddleCenter;

        int w = 500, h = 500;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(12);
        GUILayout.Label("TOP 10 RANKING", titleStyle);
        GUILayout.Space(20);

        List<int> ranking = MazeSaveSystem.GetRanking();
        for (int i = 0; i < ranking.Count; i++)
        {
            GUILayout.Label($"{i + 1}. {ranking[i]}", entryStyle);
            GUILayout.Space(4);
        }
        
        if (ranking.Count == 0)
        {
            GUILayout.Label("Sem scores ainda.", entryStyle);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Limpar Ranking", buttonStyle, GUILayout.Height(36)))
        {
            MazeSaveSystem.ClearRanking();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(36)))
        {
            MazeMenu.HideRanking();
        }

        GUILayout.EndArea();
    }
} 