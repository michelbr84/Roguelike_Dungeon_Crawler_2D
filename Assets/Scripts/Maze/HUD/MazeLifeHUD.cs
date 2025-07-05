using UnityEngine;

public static class MazeLifeHUD
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
        int y = 8;

        // --- Vidas (ícones ou texto) ---
        if (mazeObj.extraLifeTexture)
        {
            // Mostrar vidas inteiras
            for (int i = 0; i < mazeObj.lives; i++)
            {
                float iconX = rightX - (i + 1) * (iconSize + 2);
                GUI.DrawTexture(new Rect(iconX, y, iconSize, iconSize), mazeObj.extraLifeTexture, ScaleMode.ScaleToFit);
            }
            
            // Mostrar vida fracionária se houver
            if (mazeObj.fractionalHealth > 1f)
            {
                float fractionalIconX = rightX - (mazeObj.lives + 1) * (iconSize + 2);
                // Desenhar meio coração (metade da textura)
                Rect halfHeartRect = new Rect(fractionalIconX, y, iconSize / 2, iconSize);
                GUI.DrawTextureWithTexCoords(halfHeartRect, mazeObj.extraLifeTexture, new Rect(0, 0, 0.5f, 1f));
            }
            
            // Texto com vida fracionária
            string livesText = mazeObj.fractionalHealth > 1f ? $"{mazeObj.lives + (mazeObj.fractionalHealth - 1f):F1}" : $"{mazeObj.lives}";
            float textX = rightX - (mazeObj.lives + 1) * (iconSize + 2) - 120;
            GUI.Label(new Rect(textX, y, 120, iconSize), $"x{livesText}", style);
        }
        else
        {
            // Texto simples com vida fracionária
            string livesText = mazeObj.fractionalHealth > 1f ? $"{mazeObj.lives + (mazeObj.fractionalHealth - 1f):F1}" : $"{mazeObj.lives}";
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, iconSize), $"Lives: {livesText}", style);
        }
    }
} 