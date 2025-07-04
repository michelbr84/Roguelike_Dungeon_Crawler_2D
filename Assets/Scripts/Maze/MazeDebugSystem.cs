using UnityEngine;

public static class MazeDebugSystem
{
    private static bool debugMode = false;
    private static bool showDebugInfo = false;
    
    // Inicializar sistema de debug
    public static void Initialize()
    {
        // Debug mode só em editor ou com tecla especial
        #if UNITY_EDITOR
        debugMode = true;
        #endif
    }
    
    // Verificar se debug está ativo
    public static bool IsDebugMode()
    {
        return debugMode;
    }
    
    // Alternar modo debug
    public static void ToggleDebugMode()
    {
        debugMode = !debugMode;
    }
    
    // Alternar informações de debug
    public static void ToggleDebugInfo()
    {
        showDebugInfo = !showDebugInfo;
    }
    
    // Renderizar informações de debug
    public static void RenderDebugInfo(ProceduralMaze maze)
    {
        if (!debugMode || !showDebugInfo) return;
        
        GUIStyle debugStyle = new GUIStyle();
        debugStyle.fontSize = 14;
        debugStyle.normal.textColor = Color.yellow;
        debugStyle.fontStyle = FontStyle.Bold;
        
        float y = 10;
        float lineHeight = 20;
        
        // Informações do jogo
        GUI.Label(new Rect(10, y, 300, lineHeight), $"FPS: {Mathf.RoundToInt(1f / Time.deltaTime)}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Estado: {ProceduralMaze.gameState}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Nível: {maze.currentLevel}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Score: {maze.score}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Vidas: {maze.lives}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Munição: {maze.ammo}", debugStyle);
        y += lineHeight;
        
        // Informações do maze
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Maze: {maze.width}x{maze.height}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Player: {maze.playerPos}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Exit: {maze.exitPos}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Inimigos: {maze.enemies.Count}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Power-ups: {maze.powerUps.Count}", debugStyle);
        y += lineHeight;
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Tiros: {maze.bullets.Count}", debugStyle);
        y += lineHeight;
        
        // Informações de performance
        GUI.Label(new Rect(10, y, 300, lineHeight), $"Tempo: {Time.time:F1}s", debugStyle);
        y += lineHeight;
        
        // Controles de debug
        if (GUI.Button(new Rect(10, y, 100, 25), "Reset Tutorial"))
        {
            MazeTutorial.ResetTutorial();
        }
        y += 30;
        
        if (GUI.Button(new Rect(10, y, 100, 25), "Reset Stats"))
        {
            MazeStatistics.ResetAllStats();
        }
        y += 30;
        
        if (GUI.Button(new Rect(10, y, 100, 25), "Reset Ranking"))
        {
            MazeSaveSystem.ClearRanking();
        }
        y += 30;
        
        if (GUI.Button(new Rect(10, y, 100, 25), "Próxima Música"))
        {
            if (AudioManager.Instance) AudioManager.Instance.NextMusic();
        }
    }
    
    // Log de debug
    public static void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[MazeDebug] {message}");
        }
    }
    
    // Log de erro
    public static void LogError(string message)
    {
        if (debugMode)
        {
            Debug.LogError($"[MazeDebug] {message}");
        }
    }
    
    // Verificar integridade do sistema
    public static void ValidateSystem()
    {
        if (!debugMode) return;
        
        Log("Validando sistema...");
        
        // Verificar se todos os sistemas estão inicializados
        if (AudioManager.Instance == null)
            LogError("AudioManager não encontrado!");
        
        // Verificar configurações
        var settings = MazeSaveSystem.LoadSettings();
        if (settings == null)
            LogError("Configurações não carregadas!");
        
        // Verificar estatísticas
        var stats = MazeStatistics.GetPlayerStats();
        if (stats == null)
            LogError("Estatísticas não carregadas!");
        
        Log("Validação concluída!");
    }
} 