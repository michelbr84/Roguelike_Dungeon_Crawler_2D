using UnityEngine;

public static class MazeStatusHUD
{
    public static void Draw(ProceduralMaze mazeObj, string statusMessage, float statusMessageTime)
    {
        // --- Mensagem de status centralizada ---
        if (!string.IsNullOrEmpty(statusMessage) && (Time.time - statusMessageTime < 2.5f))
        {
            GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
            messageStyle.fontSize = 36;
            messageStyle.fontStyle = FontStyle.Bold;
            messageStyle.normal.textColor = Color.green;
            messageStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 240, 30, 480, 60), statusMessage, messageStyle);
        }
    }
} 