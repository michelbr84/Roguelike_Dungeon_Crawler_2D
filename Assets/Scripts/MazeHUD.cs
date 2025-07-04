using UnityEngine;

public static class MazeHUD
{
    public static int currentLevel = 1;
    public static int score = 0;
    public static float lastVictoryTime = 0f;
    public static string statusMessage = "";
    public static float statusMessageTime = 0f;

    public static void DrawHUD(ProceduralMaze mazeObj)
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
            for (int i = 0; i < mazeObj.lives; i++)
            {
                float iconX = rightX - (i + 1) * (iconSize + 2);
                GUI.DrawTexture(new Rect(iconX, y, iconSize, iconSize), mazeObj.extraLifeTexture, ScaleMode.ScaleToFit);
            }
            float textX = rightX - (mazeObj.lives + 1) * (iconSize + 2) - 120;
            GUI.Label(new Rect(textX, y, 120, iconSize), $"x{mazeObj.lives}", style);
        }
        else
        {
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, iconSize), $"Lives: {mazeObj.lives}", style);
        }
        y += iconSize + padding;

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
        y += iconSize + padding - 8;

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

            // Legenda "Shield" opcional
            // GUI.Label(new Rect(shieldIconX - 80, y, 80, iconSize), "Shield", style);

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

        // --- Outros stats (alinhados à direita) ---
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Enemies: {mazeObj.enemies.Count}", style); y += 36;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Score: {score}", style); y += 36;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Level: {currentLevel}", style); y += 36;
        
        // Mostrar dificuldade atual
        Color oldColor = GUI.color;
        GUI.color = MazeDifficultySystem.GetCurrentDifficultyColor();
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Dificuldade: {MazeDifficultySystem.GetCurrentDifficulty().name}", style);
        GUI.color = oldColor;
        y += 36;
        
        // Mostrar classe atual
        GUI.color = Color.cyan;
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Classe: {MazeCharacterSystem.GetCurrentClassName()}", style);
        GUI.color = oldColor;
        y += 36;
        
        // Mostrar pet ativo
        var activePet = MazePetSystem.GetActivePet();
        if (activePet != null && activePet.isActive)
        {
            GUI.color = MazePetSystem.GetPetTypeColor(activePet.type);
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Pet: {activePet.name} Lv.{activePet.level}", style);
            GUI.color = oldColor;
            y += 36;
        }
        
        // Mostrar clima atual
        var currentWeather = MazeWeatherSystem.GetCurrentWeather();
        if (currentWeather.type != MazeWeatherSystem.WeatherType.Clear)
        {
            GUI.color = MazeWeatherSystem.GetWeatherTypeColor(currentWeather.type);
            float timeRemaining = MazeWeatherSystem.GetWeatherTimeRemaining();
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Clima: {currentWeather.name} ({timeRemaining:0}s)", style);
            GUI.color = oldColor;
            y += 36;
        }
        
        // Mostrar evento ativo
        var activeEvent = MazeEventSystem.GetActiveEvent();
        if (activeEvent != null && activeEvent.isActive)
        {
            GUI.color = MazeEventSystem.GetEventTypeColor(activeEvent.type);
            float eventTimeRemaining = MazeEventSystem.GetEventTimeRemaining();
            GUI.Label(new Rect(rightX - labelWidth, y, labelWidth, 40), $"Evento: {activeEvent.name} ({eventTimeRemaining:0}s)", style);
            GUI.color = oldColor;
            y += 36;
        }
        
        GUI.Label(new Rect(rightX - labelWidth, y, labelWidth + 60, 40), $"Record: {mazeObj.scoreRecord}", style); y += 36;

        // --- Mensagem de status centralizada ---
        if (!string.IsNullOrEmpty(statusMessage) && (Time.time - statusMessageTime < 2.5f))
        {
            GUIStyle messageStyle = new GUIStyle(style);
            messageStyle.fontSize = 36;
            messageStyle.normal.textColor = Color.green;
            messageStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 240, 30, 480, 60), statusMessage, messageStyle);
        }
    }

    public static void ShowStatusMessage(string msg)
    {
        statusMessage = msg;
        statusMessageTime = Time.time;
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }

    public static void NextLevel()
    {
        currentLevel++;
        ShowStatusMessage($"Level {currentLevel}!");
    }

    public static void ResetAll()
    {
        score = 0;
        currentLevel = 1;
        statusMessage = "";
        statusMessageTime = 0;
    }
    
    // Desenhar missões ativas
    public static void DrawMissions()
    {
        // Implementação do sistema de missões
        var missions = MazeMissions.GetActiveMissions();
        if (missions != null && missions.Count > 0)
        {
            GUIStyle missionStyle = new GUIStyle(GUI.skin.label);
            missionStyle.fontSize = 16;
            missionStyle.normal.textColor = Color.cyan;
            missionStyle.alignment = TextAnchor.UpperLeft;

            int y = 10;
            foreach (var mission in missions)
            {
                GUI.Label(new Rect(10, y, 300, 20), mission.description, missionStyle);
                y += 20;
            }
        }
    }
    
    // Método para compatibilidade com ProceduralMaze
    public static void RenderHUD(ProceduralMaze maze)
    {
        DrawHUD(maze);
    }
}
