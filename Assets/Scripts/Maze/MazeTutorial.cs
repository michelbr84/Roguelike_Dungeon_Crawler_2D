using UnityEngine;
using System.Collections.Generic;

public static class MazeTutorial
{
    // Estados do tutorial
    public enum TutorialStep
    {
        Welcome,
        Movement,
        Shooting,
        PowerUps,
        Enemies,
        Objectives,
        Complete
    }
    
    private static TutorialStep currentStep = TutorialStep.Welcome;
    private static bool isActive = false;
    private static bool hasBeenShown = false;
    
    // Textos do tutorial
    private static readonly Dictionary<TutorialStep, string> tutorialTexts = new Dictionary<TutorialStep, string>
    {
        { TutorialStep.Welcome, "Bem-vindo ao Maze Game!\n\nEste tutorial vai te ensinar os controles básicos.\n\nPressione ESPAÇO para continuar." },
        { TutorialStep.Movement, "MOVIMENTO:\n\nUse as SETAS ou WASD para mover.\n\nO jogador sempre mira na direção do movimento.\n\nPressione ESPAÇO para continuar." },
        { TutorialStep.Shooting, "TIRO:\n\nPressione ESPAÇO para atirar.\n\nVocê tem munição limitada!\n\nAcerte os inimigos para ganhar pontos.\n\nPressione ESPAÇO para continuar." },
        { TutorialStep.PowerUps, "POWER-UPS:\n\n🔫 Munição: +5 munições\n❤️ Vida: +1 vida\n🛡️ Escudo: Proteção temporária\n⚡ Velocidade: Movimento mais rápido\n👻 Invisibilidade: Inimigos não te veem\n🚀 Teleport: Teletransporte seguro\n\nPressione ESPAÇO para continuar." },
        { TutorialStep.Enemies, "INIMIGOS:\n\nEvite tocar nos inimigos vermelhos!\n\nAlguns inimigos especiais aparecem:\n👹 Boss: Mais resistente\n🎯 Sniper: Atira de longe\n💣 Kamikaze: Corre para você\n🕷️ Spawner: Cria mais inimigos\n\nPressione ESPAÇO para continuar." },
        { TutorialStep.Objectives, "OBJETIVOS:\n\n🏁 Encontre a saída (vermelha) para avançar.\n\n📊 Complete missões para bônus.\n\n🏆 Quebre seu recorde!\n\nPressione ESPAÇO para começar!" },
        { TutorialStep.Complete, "Tutorial completo!\n\nBoa sorte!\n\nPressione ESPAÇO para jogar." }
    };
    
    // Inicializar tutorial
    public static void Initialize()
    {
        hasBeenShown = PlayerPrefs.GetInt("TutorialShown", 0) == 1;
    }
    
    // Verificar se deve mostrar tutorial
    public static bool ShouldShowTutorial()
    {
        return !hasBeenShown;
    }
    
    // Iniciar tutorial
    public static void StartTutorial()
    {
        isActive = true;
        currentStep = TutorialStep.Welcome;
        ProceduralMaze.gameState = GameState.Tutorial;
    }
    
    // Verificar se tutorial está ativo
    public static bool IsActive()
    {
        return isActive;
    }
    
    // Processar input do tutorial
    public static void HandleTutorialInput()
    {
        if (!isActive) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipTutorial();
        }
    }
    
    // Próximo passo do tutorial
    private static void NextStep()
    {
        currentStep++;
        
        if (currentStep > TutorialStep.Complete)
        {
            CompleteTutorial();
        }
    }
    
    // Pular tutorial
    private static void SkipTutorial()
    {
        CompleteTutorial();
    }
    
    // Completar tutorial
    private static void CompleteTutorial()
    {
        isActive = false;
        hasBeenShown = true;
        PlayerPrefs.SetInt("TutorialShown", 1);
        PlayerPrefs.Save();
        
        ProceduralMaze.gameState = GameState.Menu;
    }
    
    // Renderizar tutorial
    public static void RenderTutorial()
    {
        if (!isActive) return;
        
        // Fundo escuro
        Color oldColor = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = oldColor;
        
        // Container do tutorial
        float tutorialWidth = 600f;
        float tutorialHeight = 400f;
        float tutorialX = (Screen.width - tutorialWidth) / 2f;
        float tutorialY = (Screen.height - tutorialHeight) / 2f;
        
        GUI.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        GUI.DrawTexture(new Rect(tutorialX, tutorialY, tutorialWidth, tutorialHeight), Texture2D.whiteTexture);
        
        // Borda
        GUI.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        GUI.DrawTexture(new Rect(tutorialX + 2, tutorialY + 2, tutorialWidth - 4, tutorialHeight - 4), Texture2D.whiteTexture);
        
        GUI.color = Color.white;
        
        // Título
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 28;
        titleStyle.normal.textColor = Color.cyan;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(tutorialX, tutorialY + 20, tutorialWidth, 40), "TUTORIAL", titleStyle);
        
        // Texto do tutorial
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 18;
        textStyle.normal.textColor = Color.white;
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.wordWrap = true;
        
        string tutorialText = tutorialTexts[currentStep];
        GUI.Label(new Rect(tutorialX + 20, tutorialY + 80, tutorialWidth - 40, tutorialHeight - 120), tutorialText, textStyle);
        
        // Controles
        GUIStyle controlStyle = new GUIStyle();
        controlStyle.fontSize = 14;
        controlStyle.normal.textColor = Color.gray;
        controlStyle.alignment = TextAnchor.MiddleCenter;
        
        GUI.Label(new Rect(tutorialX, tutorialY + tutorialHeight - 60, tutorialWidth, 30), "ESPAÇO: Continuar | ESC: Pular", controlStyle);
        
        // Indicador de progresso
        float progress = (float)currentStep / (float)TutorialStep.Complete;
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(tutorialX + 20, tutorialY + tutorialHeight - 20, (tutorialWidth - 40) * progress, 8), Texture2D.whiteTexture);
        GUI.color = Color.white;
    }
    
    // Resetar tutorial (para debug)
    public static void ResetTutorial()
    {
        hasBeenShown = false;
        PlayerPrefs.SetInt("TutorialShown", 0);
        PlayerPrefs.Save();
    }
    
    // Método para compatibilidade com ProceduralMaze
    public static void RenderTutorial(ProceduralMaze maze)
    {
        RenderTutorial();
    }
} 