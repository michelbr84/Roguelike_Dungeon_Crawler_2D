using UnityEngine;

public static class MazeMetaHUD
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

        // Ajusta Y baseado nas estatísticas básicas
        y += 36 * 4; // Enemies, Score, Level, Record

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
    }
} 