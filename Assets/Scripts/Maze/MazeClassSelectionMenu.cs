using UnityEngine;

public static class MazeClassSelectionMenu
{
    private static bool isVisible = false;
    private static MazeCharacterSystem.CharacterClass selectedClass = MazeCharacterSystem.CharacterClass.Warrior;
    
    public static void ShowMenu()
    {
        isVisible = true;
        ProceduralMaze.gameState = GameState.ClassSelection;
    }
    
    public static void HideMenu()
    {
        isVisible = false;
        ProceduralMaze.gameState = GameState.Menu;
    }
    
    public static void RenderMenu()
    {
        if (!isVisible) return;
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.cyan;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 18;
        infoStyle.normal.textColor = Color.white;
        infoStyle.alignment = TextAnchor.MiddleLeft;
        infoStyle.wordWrap = true;

        int w = 800, h = 600;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(20);
        GUILayout.Label("SELECIONE SUA CLASSE", titleStyle);
        GUILayout.Space(30);

        // Classes disponíveis
        var allClasses = MazeCharacterSystem.GetAllClasses();
        
        for (int i = 0; i < allClasses.Length; i++)
        {
            var classStats = allClasses[i];
            bool isUnlocked = MazeCharacterSystem.IsClassUnlocked(classStats.characterClass);
            bool isSelected = selectedClass == classStats.characterClass;
            
            // Cor baseada no status
            Color originalColor = GUI.color;
            if (!isUnlocked)
            {
                GUI.color = Color.gray;
            }
            else if (isSelected)
            {
                GUI.color = Color.yellow;
            }
            
            // Botão da classe
            if (GUILayout.Button(classStats.name, buttonStyle, GUILayout.Height(50)))
            {
                if (isUnlocked)
                {
                    selectedClass = classStats.characterClass;
                }
            }
            
            GUI.color = originalColor;
            
            // Informações da classe
            if (isSelected)
            {
                GUILayout.Space(10);
                GUILayout.Label(classStats.description, infoStyle);
                GUILayout.Label($"Vida: +{(classStats.healthMultiplier - 1f) * 100:F0}%", infoStyle);
                GUILayout.Label($"Munição: +{(classStats.ammoMultiplier - 1f) * 100:F0}%", infoStyle);
                GUILayout.Label($"Dano: +{(classStats.damageMultiplier - 1f) * 100:F0}%", infoStyle);
                GUILayout.Label($"Velocidade: +{(classStats.speedMultiplier - 1f) * 100:F0}%", infoStyle);
                
                if (classStats.hasSpecialAbility)
                {
                    GUILayout.Space(10);
                    GUILayout.Label($"Habilidade Especial: {classStats.specialAbilityName}", infoStyle);
                    GUILayout.Label(classStats.specialAbilityDescription, infoStyle);
                }
                
                GUILayout.Space(20);
            }
            
            GUILayout.Space(10);
        }
        
        // Botões de ação
        GUILayout.Space(20);
        
        if (GUILayout.Button("Confirmar Seleção", buttonStyle, GUILayout.Height(40)))
        {
            MazeCharacterSystem.SetCharacterClass(selectedClass);
            HideMenu();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(40)))
        {
            HideMenu();
        }

        GUILayout.EndArea();
    }
} 