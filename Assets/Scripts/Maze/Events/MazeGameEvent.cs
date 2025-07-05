using System;

namespace Events
{
    [Serializable]
    public class MazeGameEvent
{
    public MazeEventTypes.EventType type;
    public string name;
    public string description;
    public float duration;
    public float probability;
    public int minLevel;
    public bool isActive;
    public float startTime;
    public float endTime;
    
    // Efeitos do evento
    public float enemySpawnModifier;
    public float powerUpModifier;
    public float resourceModifier;
    public float playerSpeedModifier;
    public float playerDamageModifier;
    public bool disableEnemyAttacks;
    public bool enableInfiniteEnemies;
    public bool enableMazeChanges;
    
    public MazeGameEvent(MazeEventTypes.EventType eventType)
    {
        type = eventType;
        isActive = false;
        SetDefaultsForType(eventType);
    }
    
    private void SetDefaultsForType(MazeEventTypes.EventType eventType)
    {
        switch (eventType)
        {
            case MazeEventTypes.EventType.TreasureHunt:
                name = "Caça ao Tesouro";
                description = "Encontre o tesouro escondido no labirinto!";
                duration = 60f;
                probability = 0.15f;
                minLevel = 3;
                enemySpawnModifier = 0.5f;
                powerUpModifier = 2f;
                resourceModifier = 1.5f;
                playerSpeedModifier = 1.2f;
                playerDamageModifier = 1f;
                break;
                
            case MazeEventTypes.EventType.EnemyRush:
                name = "Invasão de Inimigos";
                description = "Uma horda de inimigos invadiu o labirinto!";
                duration = 45f;
                probability = 0.2f;
                minLevel = 5;
                enemySpawnModifier = 3f;
                powerUpModifier = 0.5f;
                resourceModifier = 1f;
                playerSpeedModifier = 1f;
                playerDamageModifier = 1.3f;
                break;
                
            case MazeEventTypes.EventType.PowerSurge:
                name = "Surto de Energia";
                description = "Power-ups são mais poderosos!";
                duration = 30f;
                probability = 0.18f;
                minLevel = 2;
                enemySpawnModifier = 1f;
                powerUpModifier = 3f;
                resourceModifier = 1f;
                playerSpeedModifier = 1.1f;
                playerDamageModifier = 1.2f;
                break;
                
            case MazeEventTypes.EventType.TimeChallenge:
                name = "Desafio de Tempo";
                description = "Complete o objetivo antes do tempo acabar!";
                duration = 90f;
                probability = 0.12f;
                minLevel = 8;
                enemySpawnModifier = 1.5f;
                powerUpModifier = 1.5f;
                resourceModifier = 2f;
                playerSpeedModifier = 1.3f;
                playerDamageModifier = 1.1f;
                break;
                
            case MazeEventTypes.EventType.BossSpawn:
                name = "Aparição de Boss";
                description = "Um boss poderoso apareceu!";
                duration = 120f;
                probability = 0.08f;
                minLevel = 10;
                enemySpawnModifier = 0.5f;
                powerUpModifier = 1f;
                resourceModifier = 1f;
                playerSpeedModifier = 1f;
                playerDamageModifier = 1.5f;
                break;
                
            case MazeEventTypes.EventType.ResourceRush:
                name = "Surto de Recursos";
                description = "Recursos valiosos aparecem em abundância!";
                duration = 40f;
                probability = 0.16f;
                minLevel = 4;
                enemySpawnModifier = 1f;
                powerUpModifier = 1f;
                resourceModifier = 4f;
                playerSpeedModifier = 1f;
                playerDamageModifier = 1f;
                break;
                
            case MazeEventTypes.EventType.MazeShift:
                name = "Mudança de Labirinto";
                description = "O labirinto está mudando dinamicamente!";
                duration = 50f;
                probability = 0.1f;
                minLevel = 12;
                enemySpawnModifier = 1.2f;
                powerUpModifier = 1.2f;
                resourceModifier = 1.2f;
                playerSpeedModifier = 0.9f;
                playerDamageModifier = 1f;
                enableMazeChanges = true;
                break;
                
            case MazeEventTypes.EventType.LuckyStreak:
                name = "Sequência de Sorte";
                description = "Você está com muita sorte!";
                duration = 35f;
                probability = 0.14f;
                minLevel = 6;
                enemySpawnModifier = 0.8f;
                powerUpModifier = 2.5f;
                resourceModifier = 2f;
                playerSpeedModifier = 1.2f;
                playerDamageModifier = 1.4f;
                break;
                
            case MazeEventTypes.EventType.SurvivalMode:
                name = "Modo Sobrevivência";
                description = "Inimigos infinitos! Sobreviva o máximo possível!";
                duration = 180f;
                probability = 0.06f;
                minLevel = 15;
                enemySpawnModifier = 5f;
                powerUpModifier = 0.3f;
                resourceModifier = 0.5f;
                playerSpeedModifier = 1f;
                playerDamageModifier = 1.2f;
                enableInfiniteEnemies = true;
                break;
                
            case MazeEventTypes.EventType.PeacefulTime:
                name = "Tempo de Paz";
                description = "Os inimigos não estão atacando!";
                duration = 25f;
                probability = 0.11f;
                minLevel = 1;
                enemySpawnModifier = 0.2f;
                powerUpModifier = 1f;
                resourceModifier = 1f;
                playerSpeedModifier = 1f;
                playerDamageModifier = 1f;
                disableEnemyAttacks = true;
                break;
        }
    }
}
} 