using UnityEngine;

public static class MazeCharacterSystem
{
    // Classes disponíveis
    public enum CharacterClass
    {
        Warrior,    // Guerreiro - Mais vida, escudo melhorado
        Archer,     // Arqueiro - Mais munição, tiro duplo
        Mage        // Mago - Power-ups especiais, teleporte melhorado
    }
    
    // Configurações de cada classe
    [System.Serializable]
    public class ClassStats
    {
        public CharacterClass characterClass;
        public string name;
        public string description;
        public int baseHealth;
        public int baseAmmo;
        public float healthMultiplier;
        public float ammoMultiplier;
        public float damageMultiplier;
        public float speedMultiplier;
        public bool hasSpecialAbility;
        public string specialAbilityName;
        public string specialAbilityDescription;
        
        public ClassStats(CharacterClass cClass)
        {
            characterClass = cClass;
            SetDefaultsForClass(cClass);
        }
        
        private void SetDefaultsForClass(CharacterClass cClass)
        {
            switch (cClass)
            {
                case CharacterClass.Warrior:
                    name = "Guerreiro";
                    description = "Tank resistente com escudo melhorado";
                    baseHealth = 5;
                    baseAmmo = 8;
                    healthMultiplier = 1.5f;
                    ammoMultiplier = 0.8f;
                    damageMultiplier = 1.2f;
                    speedMultiplier = 0.9f;
                    hasSpecialAbility = true;
                    specialAbilityName = "Escudo de Ferro";
                    specialAbilityDescription = "Escudo dura 50% mais tempo";
                    break;
                    
                case CharacterClass.Archer:
                    name = "Arqueiro";
                    description = "Especialista em tiro com munição extra";
                    baseHealth = 3;
                    baseAmmo = 15;
                    healthMultiplier = 0.8f;
                    ammoMultiplier = 1.5f;
                    damageMultiplier = 1.0f;
                    speedMultiplier = 1.1f;
                    hasSpecialAbility = true;
                    specialAbilityName = "Tiro Duplo";
                    specialAbilityDescription = "Sempre atira duas balas";
                    break;
                    
                case CharacterClass.Mage:
                    name = "Mago";
                    description = "Mestre dos power-ups e teleporte";
                    baseHealth = 2;
                    baseAmmo = 6;
                    healthMultiplier = 0.7f;
                    ammoMultiplier = 0.6f;
                    damageMultiplier = 0.8f;
                    speedMultiplier = 1.0f;
                    hasSpecialAbility = true;
                    specialAbilityName = "Teleporte Mágico";
                    specialAbilityDescription = "Teleporte sem cooldown";
                    break;
            }
        }
    }
    
    // Classe atual do jogador
    private static CharacterClass currentClass = CharacterClass.Warrior;
    private static ClassStats currentClassStats;
    
    // Inicializar sistema
    public static void Initialize()
    {
        // Carregar classe salva ou usar Warrior como padrão
        int savedClass = PlayerPrefs.GetInt("CharacterClass", (int)CharacterClass.Warrior);
        SetCharacterClass((CharacterClass)savedClass);
    }
    
    // Definir classe do personagem
    public static void SetCharacterClass(CharacterClass characterClass)
    {
        currentClass = characterClass;
        currentClassStats = new ClassStats(characterClass);
        PlayerPrefs.SetInt("CharacterClass", (int)characterClass);
        PlayerPrefs.Save();
    }
    
    // Obter classe atual
    public static CharacterClass GetCurrentClass()
    {
        return currentClass;
    }
    
    // Obter estatísticas da classe atual
    public static ClassStats GetCurrentClassStats()
    {
        if (currentClassStats == null)
            Initialize();
        return currentClassStats;
    }
    
    // Aplicar estatísticas da classe ao maze
    public static void ApplyClassStats(ProceduralMaze maze)
    {
        if (currentClassStats == null)
            Initialize();
            
        // Aplicar multiplicadores
        maze.startingLives = Mathf.RoundToInt(maze.startingLives * currentClassStats.healthMultiplier);
        maze.lives = maze.startingLives;
        maze.startingAmmo = Mathf.RoundToInt(maze.startingAmmo * currentClassStats.ammoMultiplier);
        maze.ammo = maze.startingAmmo;
        
        // Aplicar multiplicadores de velocidade
        maze.playerSpeedMultiplier *= currentClassStats.speedMultiplier;
        maze.bulletSpeedMultiplier *= currentClassStats.damageMultiplier;
    }
    
    // Verificar habilidades especiais
    public static bool HasSpecialAbility()
    {
        return currentClassStats?.hasSpecialAbility ?? false;
    }
    
    // Aplicar habilidades especiais
    public static void ApplySpecialAbility(ProceduralMaze maze)
    {
        if (!HasSpecialAbility()) return;
        
        switch (currentClass)
        {
            case CharacterClass.Warrior:
                // Escudo melhorado
                maze.shieldDuration *= 1.5f;
                break;
                
            case CharacterClass.Archer:
                // Tiro duplo sempre ativo
                maze.doubleShotActive = true;
                maze.doubleShotTimer = float.MaxValue; // Nunca expira
                break;
                
            case CharacterClass.Mage:
                // Teleporte sempre disponível
                maze.teleportAvailable = true;
                break;
        }
    }
    
    // Obter todas as classes disponíveis
    public static ClassStats[] GetAllClasses()
    {
        return new ClassStats[]
        {
            new ClassStats(CharacterClass.Warrior),
            new ClassStats(CharacterClass.Archer),
            new ClassStats(CharacterClass.Mage)
        };
    }
    
    // Verificar se classe está desbloqueada
    public static bool IsClassUnlocked(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                return true; // Sempre desbloqueado
                
            case CharacterClass.Archer:
                return PlayerPrefs.GetInt("ArcherUnlocked", 0) == 1;
                
            case CharacterClass.Mage:
                return PlayerPrefs.GetInt("MageUnlocked", 0) == 1;
                
            default:
                return false;
        }
    }
    
    // Desbloquear classe
    public static void UnlockClass(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Archer:
                PlayerPrefs.SetInt("ArcherUnlocked", 1);
                break;
                
            case CharacterClass.Mage:
                PlayerPrefs.SetInt("MageUnlocked", 1);
                break;
        }
        PlayerPrefs.Save();
    }
    
    // Obter nome da classe atual
    public static string GetCurrentClassName()
    {
        return currentClassStats?.name ?? "Guerreiro";
    }
    
    // Obter descrição da classe atual
    public static string GetCurrentClassDescription()
    {
        return currentClassStats?.description ?? "Classe padrão";
    }
} 