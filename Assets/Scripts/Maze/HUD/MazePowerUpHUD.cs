using UnityEngine;

public static class MazePowerUpHUD
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
        int rightX = Screen.width - margin;
        int y = 8 + iconSize + padding + iconSize + padding - 8; // Posição após vidas e munição

        // --- Shield ativo (ícone, barra de tempo, piscada) ---
        if (mazeObj.shieldActive && mazeObj.shieldIcon != null)
        {
            // Piscada quando faltam menos de 1.2s
            bool blink = (mazeObj.shieldTimer <= 1.2f) && (Mathf.FloorToInt(Time.time * 8) % 2 == 0);
            float iconAlpha = blink ? 0.35f : 1f;
            Color prevCol = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, iconAlpha);

            // Ícone do escudo
            float shieldIconX = rightX - iconSize;
            GUI.DrawTexture(new Rect(shieldIconX, y, iconSize, iconSize), mazeObj.shieldIcon, ScaleMode.ScaleToFit);

            // Barra de tempo visual (logo abaixo do ícone)
            float shieldBarMaxWidth = iconSize;
            float shieldBarHeight = 8f;
            float shieldPerc = Mathf.Clamp01(mazeObj.shieldTimer / mazeObj.shieldDuration);
            Color barColor = (shieldPerc > 0.5f) ? Color.cyan : (shieldPerc > 0.2f ? Color.yellow : Color.red);
            GUI.color = barColor;
            GUI.DrawTexture(new Rect(shieldIconX, y + iconSize + 2, shieldBarMaxWidth * shieldPerc, shieldBarHeight), Texture2D.whiteTexture);
            GUI.color = prevCol;

            // Tempo numérico opcional (pequeno)
            GUIStyle shieldTimeStyle = new GUIStyle(style);
            shieldTimeStyle.fontSize = 17;
            shieldTimeStyle.normal.textColor = Color.cyan;
            shieldTimeStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(shieldIconX, y + iconSize + 12, iconSize, 20), $"{mazeObj.shieldTimer:0.0}s", shieldTimeStyle);

            y += iconSize + (int)shieldBarHeight + 18;
        }

        // --- Power-ups temporários (DoubleShot, TripleShot, SpeedBoost, Invisibility, ScoreBooster) ---
        float powerupIconX = rightX - iconSize;
        float powerupY = y;
        float barMaxWidth = iconSize;
        float barHeight = 8f;
        GUIStyle timerStyle = new GUIStyle(style);
        timerStyle.fontSize = 17;
        timerStyle.normal.textColor = Color.white;
        timerStyle.alignment = TextAnchor.UpperCenter;

        if (mazeObj.doubleShotActive)
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(powerupIconX - 60, powerupY, 60, iconSize), "2x Shot", timerStyle);
            float perc = Mathf.Clamp01(mazeObj.doubleShotTimer / mazeObj.doubleShotDuration);
            GUI.color = Color.yellow;
            GUI.DrawTexture(new Rect(powerupIconX, powerupY, barMaxWidth * perc, barHeight), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(powerupIconX, powerupY + 10, iconSize, 20), $"{mazeObj.doubleShotTimer:0.0}s", timerStyle);
            powerupY += iconSize + 6;
        }
        if (mazeObj.tripleShotActive)
        {
            GUI.color = Color.magenta;
            GUI.Label(new Rect(powerupIconX - 60, powerupY, 60, iconSize), "3x Shot", timerStyle);
            float perc = Mathf.Clamp01(mazeObj.tripleShotTimer / mazeObj.tripleShotDuration);
            GUI.color = Color.magenta;
            GUI.DrawTexture(new Rect(powerupIconX, powerupY, barMaxWidth * perc, barHeight), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(powerupIconX, powerupY + 10, iconSize, 20), $"{mazeObj.tripleShotTimer:0.0}s", timerStyle);
            powerupY += iconSize + 6;
        }
        if (mazeObj.speedBoostActive)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(powerupIconX - 60, powerupY, 60, iconSize), "Speed", timerStyle);
            float perc = Mathf.Clamp01(mazeObj.speedBoostTimer / mazeObj.speedBoostDuration);
            GUI.color = Color.cyan;
            GUI.DrawTexture(new Rect(powerupIconX, powerupY, barMaxWidth * perc, barHeight), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(powerupIconX, powerupY + 10, iconSize, 20), $"{mazeObj.speedBoostTimer:0.0}s", timerStyle);
            powerupY += iconSize + 6;
        }
        if (mazeObj.invisibilityActive)
        {
            GUI.color = Color.gray;
            GUI.Label(new Rect(powerupIconX - 60, powerupY, 60, iconSize), "Invis", timerStyle);
            float perc = Mathf.Clamp01(mazeObj.invisibilityTimer / mazeObj.invisibilityDuration);
            GUI.color = Color.gray;
            GUI.DrawTexture(new Rect(powerupIconX, powerupY, barMaxWidth * perc, barHeight), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(powerupIconX, powerupY + 10, iconSize, 20), $"{mazeObj.invisibilityTimer:0.0}s", timerStyle);
            powerupY += iconSize + 6;
        }
        if (mazeObj.scoreBoosterActive)
        {
            GUI.color = Color.green;
            GUI.Label(new Rect(powerupIconX - 60, powerupY, 60, iconSize), "Score x2", timerStyle);
            float perc = Mathf.Clamp01(mazeObj.scoreBoosterTimer / mazeObj.scoreBoosterDuration);
            GUI.color = Color.green;
            GUI.DrawTexture(new Rect(powerupIconX, powerupY, barMaxWidth * perc, barHeight), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(powerupIconX, powerupY + 10, iconSize, 20), $"{mazeObj.scoreBoosterTimer:0.0}s", timerStyle);
            powerupY += iconSize + 6;
        }
        GUI.color = Color.white;

        // --- Proteção ao spawnar ---
        if (MazePlayerUtils.IsSpawnProtected())
        {
            GUIStyle protectionStyle = new GUIStyle(style);
            protectionStyle.fontSize = 24;
            protectionStyle.normal.textColor = Color.cyan;
            protectionStyle.alignment = TextAnchor.UpperCenter;
            
            // Piscada para chamar atenção
            bool blink = (Mathf.FloorToInt(Time.time * 4) % 2 == 0);
            if (blink)
            {
                GUI.Label(new Rect(Screen.width / 2 - 120, Screen.height - 80, 240, 40), "PROTEGIDO!", protectionStyle);
            }
        }
    }
} 