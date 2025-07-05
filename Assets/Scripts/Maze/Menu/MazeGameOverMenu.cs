// MazeGameOverMenu.cs
using UnityEngine;

public static class MazeGameOverMenu
{
    public static void Draw(int score, int record, int lives)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 52;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.red;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 32;
        infoStyle.normal.textColor = Color.yellow;
        infoStyle.alignment = TextAnchor.UpperCenter;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 28;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        int w = 540, h = 340;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(16);
        GUILayout.Label("GAME OVER", titleStyle);
        GUILayout.Space(12);

        GUILayout.Label($"Score: {score}", infoStyle);
        GUILayout.Label($"Recorde: {record}", infoStyle);
        GUILayout.Label($"Vidas: {lives}", infoStyle); // sempre mostra 0 vidas

        GUILayout.Space(16);

        if (GUILayout.Button("Jogar Novamente", buttonStyle, GUILayout.Height(48)))
        {
            ProceduralMaze.RestartGameFromGameOver();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Sair", buttonStyle, GUILayout.Height(36)))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        GUILayout.EndArea();
    }
} 