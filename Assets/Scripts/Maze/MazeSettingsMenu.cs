using UnityEngine;

public static class MazeSettingsMenu
{
    // Estados do menu
    public enum SettingsState
    {
        Hidden,
        Main,
        Audio,
        Visual,
        Gameplay,
        Controls
    }
    
    private static SettingsState currentState = SettingsState.Hidden;
    private static bool isVisible = false;
    
    // Configurações atuais
    private static MazeSaveSystem.GameSettings currentSettings;
    
    // Dimensões do menu
    private static float menuWidth = 400f;
    private static float menuHeight = 500f;
    private static float menuX;
    private static float menuY;
    
    // Inicializar menu
    public static void Initialize()
    {
        currentSettings = MazeSaveSystem.LoadSettings();
        CalculateMenuPosition();
    }
    
    // Calcular posição do menu
    private static void CalculateMenuPosition()
    {
        menuX = (Screen.width - menuWidth) / 2f;
        menuY = (Screen.height - menuHeight) / 2f;
    }
    
    // Mostrar/ocultar menu
    public static void ToggleMenu()
    {
        isVisible = !isVisible;
        if (isVisible)
        {
            currentState = SettingsState.Main;
            CalculateMenuPosition();
        }
    }
    
    // Verificar se menu está visível
    public static bool IsVisible()
    {
        return isVisible;
    }
    
    // Renderizar menu
    public static void RenderMenu()
    {
        if (!isVisible) return;
        
        // Fundo escuro
        Color oldColor = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = oldColor;
        
        // Container do menu
        GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
        GUI.DrawTexture(new Rect(menuX, menuY, menuWidth, menuHeight), Texture2D.whiteTexture);
        
        // Borda
        GUI.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        GUI.DrawTexture(new Rect(menuX + 2, menuY + 2, menuWidth - 4, menuHeight - 4), Texture2D.whiteTexture);
        
        GUI.color = Color.white;
        
        // Renderizar conteúdo baseado no estado
        switch (currentState)
        {
            case SettingsState.Main:
                RenderMainMenu();
                break;
            case SettingsState.Audio:
                RenderAudioMenu();
                break;
            case SettingsState.Visual:
                RenderVisualMenu();
                break;
            case SettingsState.Gameplay:
                RenderGameplayMenu();
                break;
            case SettingsState.Controls:
                RenderControlsMenu();
                break;
        }
    }
    
    // Menu principal
    private static void RenderMainMenu()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 24;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 18;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        
        // Título
        GUI.Label(new Rect(menuX, menuY + 20, menuWidth, 40), "CONFIGURAÇÕES", titleStyle);
        
        // Botões
        float buttonY = menuY + 100;
        float buttonHeight = 40;
        float buttonSpacing = 50;
        
        if (GUI.Button(new Rect(menuX + 50, buttonY, menuWidth - 100, buttonHeight), "🎵 Áudio", buttonStyle))
        {
            currentState = SettingsState.Audio;
        }
        
        buttonY += buttonSpacing;
        if (GUI.Button(new Rect(menuX + 50, buttonY, menuWidth - 100, buttonHeight), "🎨 Visuais", buttonStyle))
        {
            currentState = SettingsState.Visual;
        }
        
        buttonY += buttonSpacing;
        if (GUI.Button(new Rect(menuX + 50, buttonY, menuWidth - 100, buttonHeight), "🎮 Gameplay", buttonStyle))
        {
            currentState = SettingsState.Gameplay;
        }
        
        buttonY += buttonSpacing;
        if (GUI.Button(new Rect(menuX + 50, buttonY, menuWidth - 100, buttonHeight), "🎯 Controles", buttonStyle))
        {
            currentState = SettingsState.Controls;
        }
        
        buttonY += buttonSpacing + 20;
        if (GUI.Button(new Rect(menuX + 50, buttonY, menuWidth - 100, buttonHeight), "❌ Fechar", buttonStyle))
        {
            ToggleMenu();
        }
    }
    
    // Menu de áudio
    private static void RenderAudioMenu()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.normal.textColor = Color.white;
        
        // Título
        GUI.Label(new Rect(menuX, menuY + 20, menuWidth, 40), "CONFIGURAÇÕES DE ÁUDIO", titleStyle);
        
        float sliderY = menuY + 80;
        float sliderHeight = 30;
        float sliderSpacing = 50;
        
        // Volume Master
        GUI.Label(new Rect(menuX + 20, sliderY, 200, sliderHeight), "Volume Master", labelStyle);
        currentSettings.masterVolume = GUI.HorizontalSlider(new Rect(menuX + 220, sliderY + 10, 150, 20), currentSettings.masterVolume, 0f, 1f);
        GUI.Label(new Rect(menuX + 380, sliderY, 50, sliderHeight), $"{Mathf.RoundToInt(currentSettings.masterVolume * 100)}%", labelStyle);
        
        sliderY += sliderSpacing;
        
        // Volume Música
        GUI.Label(new Rect(menuX + 20, sliderY, 200, sliderHeight), "Volume Música", labelStyle);
        currentSettings.musicVolume = GUI.HorizontalSlider(new Rect(menuX + 220, sliderY + 10, 150, 20), currentSettings.musicVolume, 0f, 1f);
        GUI.Label(new Rect(menuX + 380, sliderY, 50, sliderHeight), $"{Mathf.RoundToInt(currentSettings.musicVolume * 100)}%", labelStyle);
        
        sliderY += sliderSpacing;
        
        // Volume SFX
        GUI.Label(new Rect(menuX + 20, sliderY, 200, sliderHeight), "Volume SFX", labelStyle);
        currentSettings.sfxVolume = GUI.HorizontalSlider(new Rect(menuX + 220, sliderY + 10, 150, 20), currentSettings.sfxVolume, 0f, 1f);
        GUI.Label(new Rect(menuX + 380, sliderY, 50, sliderHeight), $"{Mathf.RoundToInt(currentSettings.sfxVolume * 100)}%", labelStyle);
        
        // Botões
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 16;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        
        float buttonY = menuY + menuHeight - 80;
        
        if (GUI.Button(new Rect(menuX + 20, buttonY, 100, 40), "Salvar", buttonStyle))
        {
            MazeSaveSystem.SaveSettings(currentSettings);
        }
        
        if (GUI.Button(new Rect(menuX + 140, buttonY, 100, 40), "Voltar", buttonStyle))
        {
            currentState = SettingsState.Main;
        }
        
        if (GUI.Button(new Rect(menuX + 260, buttonY, 100, 40), "Padrão", buttonStyle))
        {
            currentSettings = new MazeSaveSystem.GameSettings();
        }
    }
    
    // Menu de visuais
    private static void RenderVisualMenu()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.normal.textColor = Color.white;
        
        // Título
        GUI.Label(new Rect(menuX, menuY + 20, menuWidth, 40), "CONFIGURAÇÕES VISUAIS", titleStyle);
        
        float optionY = menuY + 80;
        float optionHeight = 40;
        float optionSpacing = 50;
        
        // Screen Shake
        GUI.Label(new Rect(menuX + 20, optionY, 200, optionHeight), "Screen Shake", labelStyle);
        currentSettings.screenShakeEnabled = GUI.Toggle(new Rect(menuX + 220, optionY + 10, 20, 20), currentSettings.screenShakeEnabled, "");
        
        optionY += optionSpacing;
        
        // Efeitos de Partículas
        GUI.Label(new Rect(menuX + 20, optionY, 200, optionHeight), "Efeitos de Partículas", labelStyle);
        currentSettings.particleEffectsEnabled = GUI.Toggle(new Rect(menuX + 220, optionY + 10, 20, 20), currentSettings.particleEffectsEnabled, "");
        
        optionY += optionSpacing;
        
        // Controles Touch
        GUI.Label(new Rect(menuX + 20, optionY, 200, optionHeight), "Controles Touch", labelStyle);
        currentSettings.touchControlsEnabled = GUI.Toggle(new Rect(menuX + 220, optionY + 10, 20, 20), currentSettings.touchControlsEnabled, "");
        
        // Botões
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 16;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        
        float buttonY = menuY + menuHeight - 80;
        
        if (GUI.Button(new Rect(menuX + 20, buttonY, 100, 40), "Salvar", buttonStyle))
        {
            MazeSaveSystem.SaveSettings(currentSettings);
        }
        
        if (GUI.Button(new Rect(menuX + 140, buttonY, 100, 40), "Voltar", buttonStyle))
        {
            currentState = SettingsState.Main;
        }
        
        if (GUI.Button(new Rect(menuX + 260, buttonY, 100, 40), "Padrão", buttonStyle))
        {
            currentSettings = new MazeSaveSystem.GameSettings();
        }
    }
    
    // Menu de gameplay
    private static void RenderGameplayMenu()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.normal.textColor = Color.white;
        
        // Título
        GUI.Label(new Rect(menuX, menuY + 20, menuWidth, 40), "CONFIGURAÇÕES DE GAMEPLAY", titleStyle);
        
        float optionY = menuY + 80;
        float optionHeight = 40;
        float optionSpacing = 50;
        
        // Dificuldade
        GUI.Label(new Rect(menuX + 20, optionY, 200, optionHeight), "Dificuldade", labelStyle);
        
        string[] difficulties = { "Fácil", "Médio", "Difícil" };
        currentSettings.difficultyLevel = GUI.SelectionGrid(new Rect(menuX + 220, optionY, 150, 30), currentSettings.difficultyLevel - 1, difficulties, 3) + 1;
        
        // Botões
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 16;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        
        float buttonY = menuY + menuHeight - 80;
        
        if (GUI.Button(new Rect(menuX + 20, buttonY, 100, 40), "Salvar", buttonStyle))
        {
            MazeSaveSystem.SaveSettings(currentSettings);
        }
        
        if (GUI.Button(new Rect(menuX + 140, buttonY, 100, 40), "Voltar", buttonStyle))
        {
            currentState = SettingsState.Main;
        }
        
        if (GUI.Button(new Rect(menuX + 260, buttonY, 100, 40), "Padrão", buttonStyle))
        {
            currentSettings = new MazeSaveSystem.GameSettings();
        }
    }
    
    // Menu de controles
    private static void RenderControlsMenu()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.white;
        
        // Título
        GUI.Label(new Rect(menuX, menuY + 20, menuWidth, 40), "CONTROLES", titleStyle);
        
        float infoY = menuY + 80;
        float infoSpacing = 25;
        
        // Informações dos controles
        GUI.Label(new Rect(menuX + 20, infoY, menuWidth - 40, 30), "🎮 Teclado:", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "WASD / Setas: Mover", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "Espaço: Atirar", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "T: Teleport", labelStyle);
        
        infoY += infoSpacing + 10;
        GUI.Label(new Rect(menuX + 20, infoY, menuWidth - 40, 30), "📱 Touch (Mobile):", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "Joystick: Mover", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "🔫: Atirar", labelStyle);
        infoY += infoSpacing;
        GUI.Label(new Rect(menuX + 40, infoY, menuWidth - 60, 20), "⚡: Teleport", labelStyle);
        
        // Botões
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 16;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        
        float buttonY = menuY + menuHeight - 80;
        
        if (GUI.Button(new Rect(menuX + 140, buttonY, 100, 40), "Voltar", buttonStyle))
        {
            currentState = SettingsState.Main;
        }
    }
} 