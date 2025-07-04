using UnityEngine;
using System.Collections.Generic;

public static class MazeEquipmentSystem
{
    // Tipos de equipamento
    public enum EquipmentType
    {
        Weapon,     // Arma - Aumenta dano
        Armor,      // Armadura - Aumenta defesa
        Accessory   // Acessório - Efeitos especiais
    }
    
    // Raridades
    public enum Rarity
    {
        Common,     // Comum - Branco
        Uncommon,   // Incomum - Verde
        Rare,       // Raro - Azul
        Epic,       // Épico - Roxo
        Legendary   // Lendário - Dourado
    }
    
    // Classe base para equipamentos
    [System.Serializable]
    public class Equipment
    {
        public string id;
        public string name;
        public string description;
        public EquipmentType type;
        public Rarity rarity;
        public Sprite icon;
        
        // Modificadores de estatísticas
        public float healthBonus;
        public float ammoBonus;
        public float damageBonus;
        public float speedBonus;
        public float shieldBonus;
        
        // Efeitos especiais
        public bool hasSpecialEffect;
        public string specialEffectName;
        public string specialEffectDescription;
        
        public Equipment()
        {
            id = "";
            name = "";
            description = "";
            type = EquipmentType.Weapon;
            rarity = Rarity.Common;
            healthBonus = 0f;
            ammoBonus = 0f;
            damageBonus = 0f;
            speedBonus = 0f;
            shieldBonus = 0f;
            hasSpecialEffect = false;
            specialEffectName = "";
            specialEffectDescription = "";
        }
    }
    
    // Equipamentos específicos
    public static class EquipmentDatabase
    {
        // Armas
        public static Equipment CreateSword()
        {
            return new Equipment
            {
                id = "sword",
                name = "Espada de Ferro",
                description = "Uma espada básica mas confiável",
                type = EquipmentType.Weapon,
                rarity = Rarity.Common,
                damageBonus = 0.2f
            };
        }
        
        public static Equipment CreateBow()
        {
            return new Equipment
            {
                id = "bow",
                name = "Arco Longo",
                description = "Arco que aumenta a precisão",
                type = EquipmentType.Weapon,
                rarity = Rarity.Uncommon,
                damageBonus = 0.3f,
                ammoBonus = 0.2f
            };
        }
        
        public static Equipment CreateStaff()
        {
            return new Equipment
            {
                id = "staff",
                name = "Cajado Mágico",
                description = "Cajado que amplifica power-ups",
                type = EquipmentType.Weapon,
                rarity = Rarity.Rare,
                damageBonus = 0.1f,
                hasSpecialEffect = true,
                specialEffectName = "Amplificação Mágica",
                specialEffectDescription = "Power-ups duram 25% mais tempo"
            };
        }
        
        // Armaduras
        public static Equipment CreateLeatherArmor()
        {
            return new Equipment
            {
                id = "leather_armor",
                name = "Armadura de Couro",
                description = "Armadura leve que não atrapalha o movimento",
                type = EquipmentType.Armor,
                rarity = Rarity.Common,
                healthBonus = 0.2f,
                speedBonus = 0.1f
            };
        }
        
        public static Equipment CreateChainMail()
        {
            return new Equipment
            {
                id = "chain_mail",
                name = "Cota de Malha",
                description = "Armadura resistente que oferece boa proteção",
                type = EquipmentType.Armor,
                rarity = Rarity.Uncommon,
                healthBonus = 0.4f,
                shieldBonus = 0.2f
            };
        }
        
        public static Equipment CreatePlateArmor()
        {
            return new Equipment
            {
                id = "plate_armor",
                name = "Armadura de Placas",
                description = "Armadura pesada que oferece máxima proteção",
                type = EquipmentType.Armor,
                rarity = Rarity.Rare,
                healthBonus = 0.6f,
                shieldBonus = 0.4f,
                speedBonus = -0.1f
            };
        }
        
        // Acessórios
        public static Equipment CreateRing()
        {
            return new Equipment
            {
                id = "ring",
                name = "Anel de Proteção",
                description = "Anel que oferece proteção mágica",
                type = EquipmentType.Accessory,
                rarity = Rarity.Common,
                shieldBonus = 0.1f
            };
        }
        
        public static Equipment CreateAmulet()
        {
            return new Equipment
            {
                id = "amulet",
                name = "Amuleto da Vida",
                description = "Amuleto que aumenta a vitalidade",
                type = EquipmentType.Accessory,
                rarity = Rarity.Uncommon,
                healthBonus = 0.3f,
                hasSpecialEffect = true,
                specialEffectName = "Regeneração",
                specialEffectDescription = "Regenera 1 vida a cada 5 níveis"
            };
        }
        
        public static Equipment CreateBoots()
        {
            return new Equipment
            {
                id = "boots",
                name = "Botas de Velocidade",
                description = "Botas que aumentam a agilidade",
                type = EquipmentType.Accessory,
                rarity = Rarity.Rare,
                speedBonus = 0.3f,
                hasSpecialEffect = true,
                specialEffectName = "Agilidade",
                specialEffectDescription = "Pode se mover duas vezes por turno"
            };
        }
    }
    
    // Inventário do jogador
    private static Dictionary<EquipmentType, Equipment> equippedItems = new Dictionary<EquipmentType, Equipment>();
    private static List<Equipment> inventory = new List<Equipment>();
    
    // Inicializar sistema
    public static void Initialize()
    {
        LoadEquipment();
        
        // Equipar itens padrão se não houver nada equipado
        if (equippedItems.Count == 0)
        {
            EquipItem(EquipmentDatabase.CreateSword());
            EquipItem(EquipmentDatabase.CreateLeatherArmor());
        }
    }
    
    // Carregar equipamentos salvos
    private static void LoadEquipment()
    {
        // Carregar itens equipados
        string equippedJson = PlayerPrefs.GetString("EquippedItems", "");
        if (!string.IsNullOrEmpty(equippedJson))
        {
            // Implementar deserialização se necessário
        }
        
        // Carregar inventário
        string inventoryJson = PlayerPrefs.GetString("Inventory", "");
        if (!string.IsNullOrEmpty(inventoryJson))
        {
            // Implementar deserialização se necessário
        }
    }
    
    // Salvar equipamentos
    private static void SaveEquipment()
    {
        // Salvar itens equipados
        string equippedJson = JsonUtility.ToJson(equippedItems);
        PlayerPrefs.SetString("EquippedItems", equippedJson);
        
        // Salvar inventário
        string inventoryJson = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString("Inventory", inventoryJson);
        
        PlayerPrefs.Save();
    }
    
    // Equipar item
    public static void EquipItem(Equipment equipment)
    {
        equippedItems[equipment.type] = equipment;
        SaveEquipment();
    }
    
    // Desequipar item
    public static void UnequipItem(EquipmentType type)
    {
        if (equippedItems.ContainsKey(type))
        {
            equippedItems.Remove(type);
            SaveEquipment();
        }
    }
    
    // Obter item equipado
    public static Equipment GetEquippedItem(EquipmentType type)
    {
        return equippedItems.ContainsKey(type) ? equippedItems[type] : null;
    }
    
    // Adicionar item ao inventário
    public static void AddToInventory(Equipment equipment)
    {
        inventory.Add(equipment);
        SaveEquipment();
    }
    
    // Remover item do inventário
    public static void RemoveFromInventory(Equipment equipment)
    {
        inventory.Remove(equipment);
        SaveEquipment();
    }
    
    // Obter inventário
    public static List<Equipment> GetInventory()
    {
        return new List<Equipment>(inventory);
    }
    
    // Aplicar modificadores de equipamento ao maze
    public static void ApplyEquipmentStats(ProceduralMaze maze)
    {
        float totalHealthBonus = 0f;
        float totalAmmoBonus = 0f;
        float totalDamageBonus = 0f;
        float totalSpeedBonus = 0f;
        float totalShieldBonus = 0f;
        
        // Calcular bônus totais
        foreach (var equipment in equippedItems.Values)
        {
            totalHealthBonus += equipment.healthBonus;
            totalAmmoBonus += equipment.ammoBonus;
            totalDamageBonus += equipment.damageBonus;
            totalSpeedBonus += equipment.speedBonus;
            totalShieldBonus += equipment.shieldBonus;
        }
        
        // Aplicar modificadores
        maze.startingLives = Mathf.RoundToInt(maze.startingLives * (1f + totalHealthBonus));
        maze.lives = maze.startingLives;
        maze.startingAmmo = Mathf.RoundToInt(maze.startingAmmo * (1f + totalAmmoBonus));
        maze.ammo = maze.startingAmmo;
        maze.bulletSpeedMultiplier *= (1f + totalDamageBonus);
        maze.playerSpeedMultiplier *= (1f + totalSpeedBonus);
        maze.shieldDuration *= (1f + totalShieldBonus);
    }
    
    // Aplicar efeitos especiais
    public static void ApplySpecialEffects(ProceduralMaze maze)
    {
        foreach (var equipment in equippedItems.Values)
        {
            if (!equipment.hasSpecialEffect) continue;
            
            switch (equipment.id)
            {
                case "staff":
                    // Amplificação mágica
                    maze.shieldDuration *= 1.25f;
                    maze.doubleShotDuration *= 1.25f;
                    maze.tripleShotDuration *= 1.25f;
                    maze.speedBoostDuration *= 1.25f;
                    maze.invisibilityDuration *= 1.25f;
                    maze.scoreBoosterDuration *= 1.25f;
                    break;
                    
                case "amulet":
                    // Regeneração
                    if (maze.currentLevel % 5 == 0 && maze.lives < maze.startingLives)
                    {
                        maze.lives++;
                        MazeHUD.ShowStatusMessage("Vida regenerada!");
                    }
                    break;
                    
                case "boots":
                    // Agilidade - implementar lógica de movimento duplo
                    break;
            }
        }
    }
    
    // Obter cor da raridade
    public static Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white;
            case Rarity.Uncommon: return Color.green;
            case Rarity.Rare: return Color.blue;
            case Rarity.Epic: return Color.magenta;
            case Rarity.Legendary: return Color.yellow;
            default: return Color.white;
        }
    }
    
    // Gerar equipamento aleatório
    public static Equipment GenerateRandomEquipment()
    {
        var allEquipment = new List<Equipment>
        {
            EquipmentDatabase.CreateSword(),
            EquipmentDatabase.CreateBow(),
            EquipmentDatabase.CreateStaff(),
            EquipmentDatabase.CreateLeatherArmor(),
            EquipmentDatabase.CreateChainMail(),
            EquipmentDatabase.CreatePlateArmor(),
            EquipmentDatabase.CreateRing(),
            EquipmentDatabase.CreateAmulet(),
            EquipmentDatabase.CreateBoots()
        };
        
        return allEquipment[Random.Range(0, allEquipment.Count)];
    }
} 