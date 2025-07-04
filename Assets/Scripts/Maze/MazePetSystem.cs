using UnityEngine;
using System.Collections.Generic;

public static class MazePetSystem
{
    // Tipos de pets
    public enum PetType
    {
        FireSpirit,     // Espírito de Fogo - Ataca inimigos
        IceFairy,       // Fada de Gelo - Congela inimigos
        LightningBird,  // Pássaro Elétrico - Dano em área
        HealingCrystal, // Cristal Curativo - Cura o jogador
        ShadowWolf      // Lobo das Sombras - Coleta itens automaticamente
    }
    
    // Classe base para pets
    [System.Serializable]
    public class Pet
    {
        public PetType type;
        public string name;
        public string description;
        public int level;
        public float experience;
        public float maxExperience;
        public bool isActive;
        public Vector2Int position;
        public float lastActionTime;
        public float actionCooldown;
        
        // Habilidades
        public float damage;
        public float healAmount;
        public float range;
        public float speed;
        
        public Pet(PetType petType)
        {
            type = petType;
            level = 1;
            experience = 0f;
            maxExperience = 100f;
            isActive = false;
            position = Vector2Int.zero;
            lastActionTime = 0f;
            SetDefaultsForType(petType);
        }
        
        private void SetDefaultsForType(PetType petType)
        {
            switch (petType)
            {
                case PetType.FireSpirit:
                    name = "Espírito de Fogo";
                    description = "Ataca inimigos com bolas de fogo";
                    damage = 15f;
                    range = 3f;
                    speed = 2f;
                    actionCooldown = 2f;
                    break;
                    
                case PetType.IceFairy:
                    name = "Fada de Gelo";
                    description = "Congela inimigos temporariamente";
                    damage = 10f;
                    range = 4f;
                    speed = 1.5f;
                    actionCooldown = 3f;
                    break;
                    
                case PetType.LightningBird:
                    name = "Pássaro Elétrico";
                    description = "Causa dano em área aos inimigos";
                    damage = 20f;
                    range = 2f;
                    speed = 3f;
                    actionCooldown = 4f;
                    break;
                    
                case PetType.HealingCrystal:
                    name = "Cristal Curativo";
                    description = "Cura o jogador periodicamente";
                    healAmount = 10f;
                    range = 2f;
                    speed = 1f;
                    actionCooldown = 5f;
                    break;
                    
                case PetType.ShadowWolf:
                    name = "Lobo das Sombras";
                    description = "Coleta itens automaticamente";
                    damage = 5f;
                    range = 5f;
                    speed = 2.5f;
                    actionCooldown = 1f;
                    break;
            }
        }
        
        // Ganhar experiência
        public void GainExperience(float amount)
        {
            experience += amount;
            if (experience >= maxExperience)
            {
                LevelUp();
            }
        }
        
        // Subir de nível
        private void LevelUp()
        {
            level++;
            experience -= maxExperience;
            maxExperience *= 1.5f;
            
            // Melhorar estatísticas
            damage *= 1.2f;
            healAmount *= 1.2f;
            range *= 1.1f;
            speed *= 1.1f;
            actionCooldown *= 0.95f;
            
            MazeHUD.ShowStatusMessage($"{name} subiu para o nível {level}!");
        }
    }
    
    // Pets disponíveis
    private static List<Pet> availablePets = new List<Pet>();
    private static Pet activePet = null;
    
    // Inicializar sistema
    public static void Initialize()
    {
        LoadPets();
        
        // Criar pets padrão se não existirem
        if (availablePets.Count == 0)
        {
            CreateDefaultPets();
        }
    }
    
    // Carregar pets salvos
    private static void LoadPets()
    {
        int petCount = PlayerPrefs.GetInt("PetCount", 0);
        
        for (int i = 0; i < petCount; i++)
        {
            string petJson = PlayerPrefs.GetString($"Pet_{i}", "");
            if (!string.IsNullOrEmpty(petJson))
            {
                // Implementar deserialização se necessário
            }
        }
        
        // Carregar pet ativo
        int activePetIndex = PlayerPrefs.GetInt("ActivePet", -1);
        if (activePetIndex >= 0 && activePetIndex < availablePets.Count)
        {
            activePet = availablePets[activePetIndex];
        }
    }
    
    // Salvar pets
    private static void SavePets()
    {
        PlayerPrefs.SetInt("PetCount", availablePets.Count);
        
        for (int i = 0; i < availablePets.Count; i++)
        {
            string petJson = JsonUtility.ToJson(availablePets[i]);
            PlayerPrefs.SetString($"Pet_{i}", petJson);
        }
        
        // Salvar pet ativo
        int activePetIndex = availablePets.IndexOf(activePet);
        PlayerPrefs.SetInt("ActivePet", activePetIndex);
        
        PlayerPrefs.Save();
    }
    
    // Criar pets padrão
    private static void CreateDefaultPets()
    {
        availablePets.Add(new Pet(PetType.FireSpirit));
        availablePets.Add(new Pet(PetType.IceFairy));
        availablePets.Add(new Pet(PetType.LightningBird));
        availablePets.Add(new Pet(PetType.HealingCrystal));
        availablePets.Add(new Pet(PetType.ShadowWolf));
        
        SavePets();
    }
    
    // Ativar pet
    public static void ActivatePet(Pet pet)
    {
        activePet = pet;
        pet.isActive = true;
        SavePets();
        
        MazeHUD.ShowStatusMessage($"{pet.name} foi ativado!");
    }
    
    // Desativar pet
    public static void DeactivatePet()
    {
        if (activePet != null)
        {
            activePet.isActive = false;
            MazeHUD.ShowStatusMessage($"{activePet.name} foi desativado!");
        }
        activePet = null;
        SavePets();
    }
    
    // Obter pet ativo
    public static Pet GetActivePet()
    {
        return activePet;
    }
    
    // Obter todos os pets
    public static List<Pet> GetAllPets()
    {
        return new List<Pet>(availablePets);
    }
    
    // Atualizar pets
    public static void UpdatePets(ProceduralMaze maze, float deltaTime)
    {
        if (activePet == null || !activePet.isActive) return;
        
        // Atualizar posição do pet (seguir o jogador)
        activePet.position = maze.playerPos;
        
        // Verificar se pode agir
        if (Time.time - activePet.lastActionTime >= activePet.actionCooldown)
        {
            PerformPetAction(activePet, maze);
            activePet.lastActionTime = Time.time;
        }
    }
    
    // Executar ação do pet
    private static void PerformPetAction(Pet pet, ProceduralMaze maze)
    {
        switch (pet.type)
        {
            case PetType.FireSpirit:
                AttackNearestEnemy(pet, maze);
                break;
                
            case PetType.IceFairy:
                FreezeNearestEnemy(pet, maze);
                break;
                
            case PetType.LightningBird:
                LightningAreaAttack(pet, maze);
                break;
                
            case PetType.HealingCrystal:
                HealPlayer(pet, maze);
                break;
                
            case PetType.ShadowWolf:
                CollectNearbyItems(pet, maze);
                break;
        }
    }
    
    // Atacar inimigo mais próximo
    private static void AttackNearestEnemy(Pet pet, ProceduralMaze maze)
    {
        Vector2Int nearestEnemy = FindNearestEnemy(pet.position, maze.enemies, pet.range);
        if (nearestEnemy != Vector2Int.zero)
        {
            // Causar dano ao inimigo
            MazeEnemyUtils.DamageEnemyAtPosition(nearestEnemy, (int)pet.damage, maze);
            pet.GainExperience(10f);
            
            MazeHUD.ShowStatusMessage($"{pet.name} atacou um inimigo!");
        }
    }
    
    // Congelar inimigo mais próximo
    private static void FreezeNearestEnemy(Pet pet, ProceduralMaze maze)
    {
        Vector2Int nearestEnemy = FindNearestEnemy(pet.position, maze.enemies, pet.range);
        if (nearestEnemy != Vector2Int.zero)
        {
            // Congelar inimigo (implementar lógica de congelamento)
            pet.GainExperience(15f);
            
            MazeHUD.ShowStatusMessage($"{pet.name} congelou um inimigo!");
        }
    }
    
    // Ataque em área com raio
    private static void LightningAreaAttack(Pet pet, ProceduralMaze maze)
    {
        int enemiesHit = 0;
        
        foreach (var enemy in maze.enemies)
        {
            float distance = Vector2Int.Distance(pet.position, enemy);
            if (distance <= pet.range)
            {
                MazeEnemyUtils.DamageEnemyAtPosition(enemy, (int)pet.damage, maze);
                enemiesHit++;
            }
        }
        
        if (enemiesHit > 0)
        {
            pet.GainExperience(enemiesHit * 8f);
            MazeHUD.ShowStatusMessage($"{pet.name} atingiu {enemiesHit} inimigos!");
        }
    }
    
    // Curar jogador
    private static void HealPlayer(Pet pet, ProceduralMaze maze)
    {
        if (maze.lives < maze.startingLives)
        {
            maze.lives = Mathf.Min(maze.lives + (int)pet.healAmount, maze.startingLives);
            pet.GainExperience(20f);
            
            MazeHUD.ShowStatusMessage($"{pet.name} curou você!");
        }
    }
    
    // Coletar itens próximos
    private static void CollectNearbyItems(Pet pet, ProceduralMaze maze)
    {
        int itemsCollected = 0;
        
        // Coletar power-ups próximos
        for (int i = maze.powerUps.Count - 1; i >= 0; i--)
        {
            float distance = Vector2Int.Distance(pet.position, maze.powerUps[i].position);
            if (distance <= pet.range)
            {
                maze.CollectPowerUp(maze.powerUps[i].type);
                maze.powerUps.RemoveAt(i);
                itemsCollected++;
            }
        }
        
        if (itemsCollected > 0)
        {
            pet.GainExperience(itemsCollected * 5f);
            MazeHUD.ShowStatusMessage($"{pet.name} coletou {itemsCollected} itens!");
        }
    }
    
    // Encontrar inimigo mais próximo
    private static Vector2Int FindNearestEnemy(Vector2Int position, List<Vector2Int> enemies, float maxRange)
    {
        Vector2Int nearest = Vector2Int.zero;
        float nearestDistance = maxRange;
        
        foreach (var enemy in enemies)
        {
            float distance = Vector2Int.Distance(position, enemy);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy;
            }
        }
        
        return nearest;
    }
    
    // Obter nome do tipo de pet
    public static string GetPetTypeName(PetType petType)
    {
        switch (petType)
        {
            case PetType.FireSpirit: return "Espírito de Fogo";
            case PetType.IceFairy: return "Fada de Gelo";
            case PetType.LightningBird: return "Pássaro Elétrico";
            case PetType.HealingCrystal: return "Cristal Curativo";
            case PetType.ShadowWolf: return "Lobo das Sombras";
            default: return "Pet";
        }
    }
    
    // Obter cor do tipo de pet
    public static Color GetPetTypeColor(PetType petType)
    {
        switch (petType)
        {
            case PetType.FireSpirit: return Color.red;
            case PetType.IceFairy: return Color.cyan;
            case PetType.LightningBird: return Color.yellow;
            case PetType.HealingCrystal: return Color.green;
            case PetType.ShadowWolf: return Color.gray;
            default: return Color.white;
        }
    }
    
    // Desbloquear novo pet
    public static void UnlockPet(PetType petType)
    {
        // Verificar se já existe
        foreach (var pet in availablePets)
        {
            if (pet.type == petType) return;
        }
        
        // Adicionar novo pet
        availablePets.Add(new Pet(petType));
        SavePets();
        
        string petName = GetPetTypeName(petType);
        MazeHUD.ShowStatusMessage($"Novo pet desbloqueado: {petName}!");
    }
    
    // Gerar pet aleatório como recompensa
    public static PetType GenerateRandomPetReward()
    {
        PetType[] allTypes = { PetType.FireSpirit, PetType.IceFairy, PetType.LightningBird, PetType.HealingCrystal, PetType.ShadowWolf };
        
        // Verificar quais já estão desbloqueados
        var unlockedTypes = new List<PetType>();
        foreach (var pet in availablePets)
        {
            unlockedTypes.Add(pet.type);
        }
        
        // Filtrar tipos não desbloqueados
        var availableTypes = new List<PetType>();
        foreach (var type in allTypes)
        {
            if (!unlockedTypes.Contains(type))
            {
                availableTypes.Add(type);
            }
        }
        
        // Retornar tipo aleatório ou null se todos estiverem desbloqueados
        if (availableTypes.Count > 0)
        {
            return availableTypes[Random.Range(0, availableTypes.Count)];
        }
        
        return PetType.FireSpirit; // Fallback
    }
} 