using UnityEngine;
using System.Collections.Generic;

public static class MazeCraftingSystem
{
    // Tipos de recursos
    public enum ResourceType
    {
        Iron,       // Ferro - Dropped por inimigos básicos
        Gold,       // Ouro - Dropped por bosses
        Crystal,    // Cristal - Dropped por inimigos especiais
        Essence,    // Essência - Dropped por power-ups especiais
        Fragment    // Fragmento - Dropped por destruir paredes
    }
    
    // Receitas de crafting
    [System.Serializable]
    public class Recipe
    {
        public string id;
        public string name;
        public string description;
        public Dictionary<ResourceType, int> requiredResources;
        public MazeEquipmentSystem.Equipment resultEquipment;
        public bool isUnlocked;
        public int unlockLevel;
        
        public Recipe()
        {
            id = "";
            name = "";
            description = "";
            requiredResources = new Dictionary<ResourceType, int>();
            resultEquipment = null;
            isUnlocked = false;
            unlockLevel = 1;
        }
    }
    
    // Inventário de recursos
    private static Dictionary<ResourceType, int> resourceInventory = new Dictionary<ResourceType, int>();
    
    // Lista de receitas
    private static List<Recipe> recipes = new List<Recipe>();
    
    // Inicializar sistema
    public static void Initialize()
    {
        LoadResources();
        InitializeRecipes();
    }
    
    // Carregar recursos salvos
    private static void LoadResources()
    {
        resourceInventory[ResourceType.Iron] = PlayerPrefs.GetInt("Resource_Iron", 0);
        resourceInventory[ResourceType.Gold] = PlayerPrefs.GetInt("Resource_Gold", 0);
        resourceInventory[ResourceType.Crystal] = PlayerPrefs.GetInt("Resource_Crystal", 0);
        resourceInventory[ResourceType.Essence] = PlayerPrefs.GetInt("Resource_Essence", 0);
        resourceInventory[ResourceType.Fragment] = PlayerPrefs.GetInt("Resource_Fragment", 0);
    }
    
    // Salvar recursos
    private static void SaveResources()
    {
        PlayerPrefs.SetInt("Resource_Iron", resourceInventory[ResourceType.Iron]);
        PlayerPrefs.SetInt("Resource_Gold", resourceInventory[ResourceType.Gold]);
        PlayerPrefs.SetInt("Resource_Crystal", resourceInventory[ResourceType.Crystal]);
        PlayerPrefs.SetInt("Resource_Essence", resourceInventory[ResourceType.Essence]);
        PlayerPrefs.SetInt("Resource_Fragment", resourceInventory[ResourceType.Fragment]);
        PlayerPrefs.Save();
    }
    
    // Inicializar receitas
    private static void InitializeRecipes()
    {
        // Receita 1: Espada de Cristal
        var crystalSword = new Recipe
        {
            id = "crystal_sword",
            name = "Espada de Cristal",
            description = "Espada forjada com cristais mágicos",
            unlockLevel = 5,
            resultEquipment = CreateCrystalSword()
        };
        crystalSword.requiredResources[ResourceType.Iron] = 3;
        crystalSword.requiredResources[ResourceType.Crystal] = 2;
        recipes.Add(crystalSword);
        
        // Receita 2: Armadura de Ouro
        var goldArmor = new Recipe
        {
            id = "gold_armor",
            name = "Armadura de Ouro",
            description = "Armadura resistente forjada em ouro",
            unlockLevel = 8,
            resultEquipment = CreateGoldArmor()
        };
        goldArmor.requiredResources[ResourceType.Iron] = 5;
        goldArmor.requiredResources[ResourceType.Gold] = 3;
        recipes.Add(goldArmor);
        
        // Receita 3: Anel da Essência
        var essenceRing = new Recipe
        {
            id = "essence_ring",
            name = "Anel da Essência",
            description = "Anel que canaliza energia mágica",
            unlockLevel = 10,
            resultEquipment = CreateEssenceRing()
        };
        essenceRing.requiredResources[ResourceType.Gold] = 2;
        essenceRing.requiredResources[ResourceType.Essence] = 4;
        recipes.Add(essenceRing);
        
        // Receita 4: Cajado dos Fragmentos
        var fragmentStaff = new Recipe
        {
            id = "fragment_staff",
            name = "Cajado dos Fragmentos",
            description = "Cajado poderoso feito de fragmentos mágicos",
            unlockLevel = 15,
            resultEquipment = CreateFragmentStaff()
        };
        fragmentStaff.requiredResources[ResourceType.Crystal] = 3;
        fragmentStaff.requiredResources[ResourceType.Fragment] = 5;
        recipes.Add(fragmentStaff);
        
        // Receita 5: Botas de Cristal
        var crystalBoots = new Recipe
        {
            id = "crystal_boots",
            name = "Botas de Cristal",
            description = "Botas que permitem movimento ultrarrápido",
            unlockLevel = 12,
            resultEquipment = CreateCrystalBoots()
        };
        crystalBoots.requiredResources[ResourceType.Iron] = 2;
        crystalBoots.requiredResources[ResourceType.Crystal] = 3;
        recipes.Add(crystalBoots);
        
        // Verificar quais receitas estão desbloqueadas
        UpdateUnlockedRecipes();
    }
    
    // Atualizar receitas desbloqueadas
    private static void UpdateUnlockedRecipes()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        
        foreach (var recipe in recipes)
        {
            recipe.isUnlocked = currentLevel >= recipe.unlockLevel;
        }
    }
    
    // Adicionar recurso ao inventário
    public static void AddResource(ResourceType resourceType, int amount)
    {
        if (!resourceInventory.ContainsKey(resourceType))
            resourceInventory[resourceType] = 0;
            
        resourceInventory[resourceType] += amount;
        SaveResources();
        
        // Mostrar mensagem
        string resourceName = GetResourceName(resourceType);
        MazeHUD.ShowStatusMessage($"+{amount} {resourceName}");
    }
    
    // Obter quantidade de recurso
    public static int GetResourceCount(ResourceType resourceType)
    {
        return resourceInventory.ContainsKey(resourceType) ? resourceInventory[resourceType] : 0;
    }
    
    // Verificar se pode craftar receita
    public static bool CanCraftRecipe(Recipe recipe)
    {
        if (!recipe.isUnlocked) return false;
        
        foreach (var requirement in recipe.requiredResources)
        {
            if (GetResourceCount(requirement.Key) < requirement.Value)
                return false;
        }
        
        return true;
    }
    
    // Craftar item
    public static bool CraftItem(Recipe recipe)
    {
        if (!CanCraftRecipe(recipe)) return false;
        
        // Consumir recursos
        foreach (var requirement in recipe.requiredResources)
        {
            resourceInventory[requirement.Key] -= requirement.Value;
        }
        
        SaveResources();
        
        // Adicionar item ao inventário
        MazeEquipmentSystem.AddToInventory(recipe.resultEquipment);
        
        // Mostrar mensagem
        MazeHUD.ShowStatusMessage($"Item criado: {recipe.name}!");
        
        return true;
    }
    
    // Obter todas as receitas
    public static List<Recipe> GetAllRecipes()
    {
        UpdateUnlockedRecipes();
        return new List<Recipe>(recipes);
    }
    
    // Obter receitas disponíveis para craft
    public static List<Recipe> GetAvailableRecipes()
    {
        var available = new List<Recipe>();
        
        foreach (var recipe in recipes)
        {
            if (CanCraftRecipe(recipe))
                available.Add(recipe);
        }
        
        return available;
    }
    
    // Métodos para criar equipamentos especiais
    private static MazeEquipmentSystem.Equipment CreateCrystalSword()
    {
        var equipment = new MazeEquipmentSystem.Equipment
        {
            id = "crystal_sword",
            name = "Espada de Cristal",
            description = "Espada forjada com cristais mágicos",
            type = MazeEquipmentSystem.EquipmentType.Weapon,
            rarity = MazeEquipmentSystem.Rarity.Rare,
            damageBonus = 0.5f,
            hasSpecialEffect = true,
            specialEffectName = "Corte de Cristal",
            specialEffectDescription = "Chance de congelar inimigos"
        };
        return equipment;
    }
    
    private static MazeEquipmentSystem.Equipment CreateGoldArmor()
    {
        var equipment = new MazeEquipmentSystem.Equipment
        {
            id = "gold_armor",
            name = "Armadura de Ouro",
            description = "Armadura resistente forjada em ouro",
            type = MazeEquipmentSystem.EquipmentType.Armor,
            rarity = MazeEquipmentSystem.Rarity.Rare,
            healthBonus = 0.8f,
            shieldBonus = 0.6f,
            hasSpecialEffect = true,
            specialEffectName = "Proteção Dourada",
            specialEffectDescription = "Reduz dano recebido em 25%"
        };
        return equipment;
    }
    
    private static MazeEquipmentSystem.Equipment CreateEssenceRing()
    {
        var equipment = new MazeEquipmentSystem.Equipment
        {
            id = "essence_ring",
            name = "Anel da Essência",
            description = "Anel que canaliza energia mágica",
            type = MazeEquipmentSystem.EquipmentType.Accessory,
            rarity = MazeEquipmentSystem.Rarity.Epic,
            healthBonus = 0.2f,
            hasSpecialEffect = true,
            specialEffectName = "Canalização Mágica",
            specialEffectDescription = "Power-ups são 50% mais eficazes"
        };
        return equipment;
    }
    
    private static MazeEquipmentSystem.Equipment CreateFragmentStaff()
    {
        var equipment = new MazeEquipmentSystem.Equipment
        {
            id = "fragment_staff",
            name = "Cajado dos Fragmentos",
            description = "Cajado poderoso feito de fragmentos mágicos",
            type = MazeEquipmentSystem.EquipmentType.Weapon,
            rarity = MazeEquipmentSystem.Rarity.Epic,
            damageBonus = 0.3f,
            ammoBonus = 0.4f,
            hasSpecialEffect = true,
            specialEffectName = "Fragmentação",
            specialEffectDescription = "Tiros se dividem em múltiplos projéteis"
        };
        return equipment;
    }
    
    private static MazeEquipmentSystem.Equipment CreateCrystalBoots()
    {
        var equipment = new MazeEquipmentSystem.Equipment
        {
            id = "crystal_boots",
            name = "Botas de Cristal",
            description = "Botas que permitem movimento ultrarrápido",
            type = MazeEquipmentSystem.EquipmentType.Accessory,
            rarity = MazeEquipmentSystem.Rarity.Rare,
            speedBonus = 0.5f,
            hasSpecialEffect = true,
            specialEffectName = "Velocidade de Cristal",
            specialEffectDescription = "Pode se mover três vezes por turno"
        };
        return equipment;
    }
    
    // Obter nome do recurso
    public static string GetResourceName(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Iron: return "Ferro";
            case ResourceType.Gold: return "Ouro";
            case ResourceType.Crystal: return "Cristal";
            case ResourceType.Essence: return "Essência";
            case ResourceType.Fragment: return "Fragmento";
            default: return "Recurso";
        }
    }
    
    // Obter cor do recurso
    public static Color GetResourceColor(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Iron: return Color.gray;
            case ResourceType.Gold: return Color.yellow;
            case ResourceType.Crystal: return Color.cyan;
            case ResourceType.Essence: return Color.magenta;
            case ResourceType.Fragment: return Color.white;
            default: return Color.white;
        }
    }
    
    // Gerar recursos aleatórios
    public static void GenerateRandomResources(Vector2Int position)
    {
        float chance = Random.Range(0f, 1f);
        
        if (chance < 0.3f) // 30% chance
        {
            AddResource(ResourceType.Iron, Random.Range(1, 3));
        }
        else if (chance < 0.5f) // 20% chance
        {
            AddResource(ResourceType.Crystal, Random.Range(1, 2));
        }
        else if (chance < 0.6f) // 10% chance
        {
            AddResource(ResourceType.Gold, 1);
        }
        else if (chance < 0.7f) // 10% chance
        {
            AddResource(ResourceType.Essence, 1);
        }
        else if (chance < 0.8f) // 10% chance
        {
            AddResource(ResourceType.Fragment, Random.Range(1, 2));
        }
    }
} 