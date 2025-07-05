using UnityEngine;
using System.Collections.Generic;

namespace Events
{
    public static class MazeEventSystem
{
    // Eventos disponíveis
    private static List<MazeGameEvent> availableEvents = new List<MazeGameEvent>();
    private static MazeGameEvent activeEvent = null;
    private static float eventCheckTimer = 0f;
    private static float eventCheckInterval = 30f; // Verificar a cada 30 segundos
    
    // Inicializar sistema
    public static void Initialize()
    {
        availableEvents.Clear();
        foreach (MazeEventTypes.EventType type in System.Enum.GetValues(typeof(MazeEventTypes.EventType)))
            availableEvents.Add(new MazeGameEvent(type));
        eventCheckTimer = 0f;
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
    public static void StartEvent(MazeGameEvent gameEvent)
    {
        activeEvent = gameEvent;
        gameEvent.isActive = true;
        gameEvent.startTime = Time.time;
        gameEvent.endTime = Time.time + gameEvent.duration;
        
        // Mostrar mensagem
        MazeHUD.ShowStatusMessage($"Evento iniciado: {gameEvent.name}!");
        MazeHUD.ShowStatusMessage(gameEvent.description);
        
        // Aplicar efeitos do evento
        MazeEventEffects.ApplyEventEffects(gameEvent);
    }
    
    // Finalizar evento
    public static void EndEvent()
    {
        if (activeEvent != null)
        {
            // Remover efeitos do evento
            MazeEventEffects.RemoveEventEffects(activeEvent);
            
            MazeHUD.ShowStatusMessage($"Evento finalizado: {activeEvent.name}");
            
            activeEvent.isActive = false;
            activeEvent = null;
        }
    }
    
    // Obter evento ativo
    public static MazeGameEvent GetActiveEvent()
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
    public static void ForceEvent(MazeEventTypes.EventType eventType)
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
    
    // Desbloquear novo tipo de evento
    public static void UnlockEvent(MazeEventTypes.EventType eventType)
    {
        MazeEventHelpers.UnlockEvent(availableEvents, eventType);
    }
    
    // Gerar evento aleatório como recompensa
    public static MazeEventTypes.EventType GenerateRandomEventReward()
    {
        return MazeEventHelpers.GenerateRandomEventReward();
    }
    
    // Gerar evento aleatório (alias para compatibilidade)
    public static void GenerateRandomWeather()
    {
        MazeEventHelpers.GenerateRandomWeather();
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
            MazeEventTypes.EventType eventType = (MazeEventTypes.EventType)activeEventType;
            
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
} 