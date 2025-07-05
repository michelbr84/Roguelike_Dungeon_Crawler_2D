using UnityEngine;

public static class MazeAmmoHUD
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
        int y = 8 + iconSize + padding; // Posição após as vidas

        // --- Munição (ícones ou texto) ---
        if (mazeObj.ammoTexture)
        {
            int maxAmmoIcons = Mathf.Min(mazeObj.ammo, 8);
            float ammoIconSize = iconSize / 1.5f;
            for (int i = 0; i < maxAmmoIcons; i++)
            {
                float iconX = rightX - (i + 1) * (ammoIconSize + 2);
                GUI.DrawTexture(new Rect(iconX, y, ammoIconSize, ammoIconSize), mazeObj.ammoTexture, ScaleMode.ScaleToFit);
            }
            float textX = rightX - (maxAmmoIcons + 1) * (ammoIconSize + 2) - 120;
            GUI.Label(new Rect(textX, y, 120, iconSize), $"x{mazeObj.ammo}", style);
        }
        else
        {
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, iconSize), $"Ammo: {mazeObj.ammo}", style);
        }
    }
} 