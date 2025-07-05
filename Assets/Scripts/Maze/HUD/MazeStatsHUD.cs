using UnityEngine;

public static class MazeStatsHUD
{
    public static void Draw(ProceduralMaze mazeObj)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 28;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.UpperRight;

        int margin = 24;
        int iconSize = 36;
        int padding = 8;

        // Calcula x inicial no canto direito
        int labelWidth = 340;
        int rightX = Screen.width - margin;
        int y = 8 + iconSize + padding + iconSize + padding - 8; // Posição após vidas e munição

        // Ajusta Y baseado nos power-ups ativos
        if (mazeObj.shieldActive && mazeObj.shieldIcon != null)
        {
            y += iconSize + 8 + 18; // Shield
        }

        // Ajusta Y baseado nos power-ups temporários
        if (mazeObj.doubleShotActive) y += iconSize + 6;
        if (mazeObj.tripleShotActive) y += iconSize + 6;
        if (mazeObj.speedBoostActive) y += iconSize + 6;
        if (mazeObj.invisibilityActive) y += iconSize + 6;
        if (mazeObj.scoreBoosterActive) y += iconSize + 6;

        // --- Outros stats (alinhados à direita) ---
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Enemies: {mazeObj.enemies.Count}", style); y += 36;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Score: {MazeHUD.score}", style); y += 36;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Level: {MazeHUD.currentLevel}", style); y += 36;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth + 60, 40), $"Record: {mazeObj.scoreRecord}", style);
    }
} 