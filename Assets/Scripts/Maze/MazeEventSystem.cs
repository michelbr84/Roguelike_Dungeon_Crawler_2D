using UnityEngine;
using System.Collections.Generic;

public static class MazeEventSystem
{
    // Tipos de eventos
    public enum EventType
    {
        TreasureHunt,       // Caça ao tesouro - Encontrar item especial
        EnemyRush,          // Invasão de inimigos - Muitos inimigos aparecem
        PowerSurge,         // Surto de energia - Power-ups mais poderosos
        TimeChallenge,      // Desafio de tempo - Completar objetivo em tempo limitado
        BossSpawn,          // Aparição de boss - Boss especial aparece
        ResourceRush,       // Surto de recursos - Muitos recursos aparecem
        MazeShift,          // Mudança de labirinto - Labirinto muda dinamicamente
        LuckyStreak,        // Sequência de sorte - Bônus temporários
        SurvivalMode,       // Modo sobrevivência - Inimigos infinitos
        PeacefulTime        // Tempo de paz - Inimigos não atacam
    }
    
    // Classe base para eventos
    [System.Serializable]
    public class GameEvent
    {
        public EventType type;
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
        
        public GameEvent(EventType eventType)
        {
            type = eventType;
            isActive = false;
            SetDefaultsForType(eventType);
        }
        
        private void SetDefaultsForType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.TreasureHunt:
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
                    
                case EventType.EnemyRush:
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
                    
                case EventType.PowerSurge:
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
                    
                case EventType.TimeChallenge:
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
                    
                case EventType.BossSpawn:
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
                    
                case EventType.ResourceRush:
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
                    
                case EventType.MazeShift:
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
                    
                case EventType.LuckyStreak:
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
                    
                case EventType.SurvivalMode:
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
                    
                case EventType.PeacefulTime:
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
    
    // Eventos disponíveis
    private static List<GameEvent> availableEvents = new List<GameEvent>();
    private static GameEvent activeEvent = null;
    private static float eventCheckTimer = 0f;
    private static float eventCheckInterval = 30f; // Verificar a cada 30 segundos
    
    // Inicializar sistema
    public static void Initialize()
    {
        InitializeEvents();
        eventCheckTimer = 0f;
    }
    
    // Inicializar eventos
    private static void InitializeEvents()
    {
        availableEvents.Clear();
        
        // Criar todos os tipos de eventos
        availableEvents.Add(new GameEvent(EventType.TreasureHunt));
        availableEvents.Add(new GameEvent(EventType.EnemyRush));
        availableEvents.Add(new GameEvent(EventType.PowerSurge));
        availableEvents.Add(new GameEvent(EventType.TimeChallenge));
        availableEvents.Add(new GameEvent(EventType.BossSpawn));
        availableEvents.Add(new GameEvent(EventType.ResourceRush));
        availableEvents.Add(new GameEvent(EventType.MazeShift));
        availableEvents.Add(new GameEvent(EventType.LuckyStreak));
        availableEvents.Add(new GameEvent(EventType.SurvivalMode));
        availableEvents.Add(new GameEvent(EventType.PeacefulTime));
    }
    
    // Atualizar sistema
    public static void Update(float deltaTime)
    {
        // Atualizar evento ativo
        if (activeEvent != null && activeEvent.isActive)
        {
            if (Time.time >= activeEvent.endTime)
            {
                EndEvent();
            }
        }
        
        // Verificar se deve iniciar novo evento
        eventCheckTimer += deltaTime;
        if (eventCheckTimer >= eventCheckInterval)
        {
            eventCheckTimer = 0f;
            CheckForNewEvent();
        }
    }
    
    // Verificar se deve iniciar novo evento
    private static void CheckForNewEvent()
    {
        if (activeEvent != null && activeEvent.isActive) return;
        
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        
        foreach (var gameEvent in availableEvents)
        {
            if (currentLevel >= gameEvent.minLevel && Random.Range(0f, 1f) < gameEvent.probability)
            {
                StartEvent(gameEvent);
                break;
            }
        }
    }
    
    // Iniciar evento
    public static void StartEvent(GameEvent gameEvent)
    {
        activeEvent = gameEvent;
        gameEvent.isActive = true;
        gameEvent.startTime = Time.time;
        gameEvent.endTime = Time.time + gameEvent.duration;
        
        // Mostrar mensagem
        MazeHUD.ShowStatusMessage($"Evento iniciado: {gameEvent.name}!");
        MazeHUD.ShowStatusMessage(gameEvent.description);
        
        // Aplicar efeitos do evento
        ApplyEventEffects(gameEvent);
    }
    
    // Finalizar evento
    public static void EndEvent()
    {
        if (activeEvent != null)
        {
            // Remover efeitos do evento
            RemoveEventEffects(activeEvent);
            
            MazeHUD.ShowStatusMessage($"Evento finalizado: {activeEvent.name}");
            
            activeEvent.isActive = false;
            activeEvent = null;
        }
    }
    
    // Aplicar efeitos do evento
    private static void ApplyEventEffects(GameEvent gameEvent)
    {
        // Os efeitos são aplicados através dos modificadores
        // que são consultados pelos outros sistemas
    }
    
    // Remover efeitos do evento
    private static void RemoveEventEffects(GameEvent gameEvent)
    {
        // Remover efeitos aplicados
    }
    
    // Obter evento ativo
    public static GameEvent GetActiveEvent()
    {
        return activeEvent;
    }
    
    // Verificar se há evento ativo
    public static bool IsEventActive()
    {
        return activeEvent != null && activeEvent.isActive;
    }
    
    // Obter tempo restante do evento
    public static float GetEventTimeRemaining()
    {
        if (activeEvent != null && activeEvent.isActive)
        {
            return Mathf.Max(0f, activeEvent.endTime - Time.time);
        }
        return 0f;
    }
    
    // Obter modificadores do evento ativo
    public static float GetEnemySpawnModifier()
    {
        return activeEvent != null && activeEvent.isActive ? activeEvent.enemySpawnModifier : 1f;
    }
    
    public static float GetPowerUpModifier()
    {
        return activeEvent != null && activeEvent.isActive ? activeEvent.powerUpModifier : 1f;
    }
    
    public static float GetResourceModifier()
    {
        return activeEvent != null && activeEvent.isActive ? activeEvent.resourceModifier : 1f;
    }
    
    public static float GetPlayerSpeedModifier()
    {
        return activeEvent != null && activeEvent.isActive ? activeEvent.playerSpeedModifier : 1f;
    }
    
    public static float GetPlayerDamageModifier()
    {
        return activeEvent != null && activeEvent.isActive ? activeEvent.playerDamageModifier : 1f;
    }
    
    public static bool AreEnemyAttacksDisabled()
    {
        return activeEvent != null && activeEvent.isActive && activeEvent.disableEnemyAttacks;
    }
    
    public static bool AreInfiniteEnemiesEnabled()
    {
        return activeEvent != null && activeEvent.isActive && activeEvent.enableInfiniteEnemies;
    }
    
    public static bool AreMazeChangesEnabled()
    {
        return activeEvent != null && activeEvent.isActive && activeEvent.enableMazeChanges;
    }
    
    // Forçar início de evento específico
    public static void ForceEvent(EventType eventType)
    {
        foreach (var gameEvent in availableEvents)
        {
            if (gameEvent.type == eventType)
            {
                StartEvent(gameEvent);
                break;
            }
        }
    }
    
    // Obter nome do tipo de evento
    public static string GetEventTypeName(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.TreasureHunt: return "Caça ao Tesouro";
            case EventType.EnemyRush: return "Invasão de Inimigos";
            case EventType.PowerSurge: return "Surto de Energia";
            case EventType.TimeChallenge: return "Desafio de Tempo";
            case EventType.BossSpawn: return "Aparição de Boss";
            case EventType.ResourceRush: return "Surto de Recursos";
            case EventType.MazeShift: return "Mudança de Labirinto";
            case EventType.LuckyStreak: return "Sequência de Sorte";
            case EventType.SurvivalMode: return "Modo Sobrevivência";
            case EventType.PeacefulTime: return "Tempo de Paz";
            default: return "Evento";
        }
    }
    
    // Obter cor do tipo de evento
    public static Color GetEventTypeColor(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.TreasureHunt: return Color.yellow;
            case EventType.EnemyRush: return Color.red;
            case EventType.PowerSurge: return Color.cyan;
            case EventType.TimeChallenge: return Color.orange;
            case EventType.BossSpawn: return Color.magenta;
            case EventType.ResourceRush: return Color.green;
            case EventType.MazeShift: return Color.blue;
            case EventType.LuckyStreak: return Color.yellow;
            case EventType.SurvivalMode: return Color.red;
            case EventType.PeacefulTime: return Color.green;
            default: return Color.white;
        }
    }
    
    // Desbloquear novo tipo de evento
    public static void UnlockEvent(EventType eventType)
    {
        // Verificar se já existe
        foreach (var gameEvent in availableEvents)
        {
            if (gameEvent.type == eventType) return;
        }
        
        // Adicionar novo evento
        availableEvents.Add(new GameEvent(eventType));
        
        string eventName = GetEventTypeName(eventType);
        MazeHUD.ShowStatusMessage($"Novo evento desbloqueado: {eventName}!");
    }
    
    // Gerar evento aleatório como recompensa
    public static EventType GenerateRandomEventReward()
    {
        EventType[] allTypes = {
            EventType.TreasureHunt, EventType.EnemyRush, EventType.PowerSurge,
            EventType.TimeChallenge, EventType.BossSpawn, EventType.ResourceRush,
            EventType.MazeShift, EventType.LuckyStreak, EventType.SurvivalMode,
            EventType.PeacefulTime
        };
        
        return allTypes[Random.Range(0, allTypes.Length)];
    }
    
    // Gerar evento aleatório (alias para compatibilidade)
    public static void GenerateRandomWeather()
    {
        // Este método é um alias para compatibilidade com o sistema de clima
        // Na verdade, eventos são gerados automaticamente pelo sistema
        CheckForNewEvent();
    }
    
    // Salvar estado dos eventos
    public static void SaveEventState()
    {
        if (activeEvent != null && activeEvent.isActive)
        {
            PlayerPrefs.SetInt("ActiveEventType", (int)activeEvent.type);
            PlayerPrefs.SetFloat("EventStartTime", activeEvent.startTime);
            PlayerPrefs.SetFloat("EventEndTime", activeEvent.endTime);
        }
        else
        {
            PlayerPrefs.SetInt("ActiveEventType", -1);
        }
        
        PlayerPrefs.Save();
    }
    
    // Carregar estado dos eventos
    public static void LoadEventState()
    {
        int activeEventType = PlayerPrefs.GetInt("ActiveEventType", -1);
        
        if (activeEventType >= 0)
        {
            EventType eventType = (EventType)activeEventType;
            
            foreach (var gameEvent in availableEvents)
            {
                if (gameEvent.type == eventType)
                {
                    activeEvent = gameEvent;
                    gameEvent.isActive = true;
                    gameEvent.startTime = PlayerPrefs.GetFloat("EventStartTime", Time.time);
                    gameEvent.endTime = PlayerPrefs.GetFloat("EventEndTime", Time.time);
                    
                    // Verificar se o evento ainda está ativo
                    if (Time.time >= gameEvent.endTime)
                    {
                        EndEvent();
                    }
                    break;
                }
            }
        }
    }
} 