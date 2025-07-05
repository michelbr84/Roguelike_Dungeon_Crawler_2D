// Este arquivo foi dividido em componentes modulares
// Veja a nova estrutura em Scripts/Maze/Events/
// 
// Para compatibilidade, este arquivo agora redireciona para os novos componentes
// mas você deve migrar gradualmente para usar os novos arquivos diretamente

// Redirecionamentos para compatibilidade
public static class MazeEventSystem
{
    // Redirecionar para o novo sistema modular
    public static void Initialize() => Events.MazeEventSystem.Initialize();
    public static void Update(float deltaTime) => Events.MazeEventSystem.Update(deltaTime);
    public static Events.MazeGameEvent GetActiveEvent() => Events.MazeEventSystem.GetActiveEvent();
    public static bool IsEventActive() => Events.MazeEventSystem.IsEventActive();
    public static float GetEventTimeRemaining() => Events.MazeEventSystem.GetEventTimeRemaining();
    public static float GetEnemySpawnModifier() => Events.MazeEventSystem.GetEnemySpawnModifier();
    public static float GetPowerUpModifier() => Events.MazeEventSystem.GetPowerUpModifier();
    public static float GetResourceModifier() => Events.MazeEventSystem.GetResourceModifier();
    public static float GetPlayerSpeedModifier() => Events.MazeEventSystem.GetPlayerSpeedModifier();
    public static float GetPlayerDamageModifier() => Events.MazeEventSystem.GetPlayerDamageModifier();
    public static bool AreEnemyAttacksDisabled() => Events.MazeEventSystem.AreEnemyAttacksDisabled();
    public static bool AreInfiniteEnemiesEnabled() => Events.MazeEventSystem.AreInfiniteEnemiesEnabled();
    public static bool AreMazeChangesEnabled() => Events.MazeEventSystem.AreMazeChangesEnabled();
    public static void ForceEvent(Events.MazeEventTypes.EventType eventType) => Events.MazeEventSystem.ForceEvent(eventType);
    public static void UnlockEvent(Events.MazeEventTypes.EventType eventType) => Events.MazeEventSystem.UnlockEvent(eventType);
    public static Events.MazeEventTypes.EventType GenerateRandomEventReward() => Events.MazeEventSystem.GenerateRandomEventReward();
    public static void GenerateRandomWeather() => Events.MazeEventSystem.GenerateRandomWeather();
    public static void SaveEventState() => Events.MazeEventSystem.SaveEventState();
    public static void LoadEventState() => Events.MazeEventSystem.LoadEventState();
    
    // Aliases para compatibilidade com código existente
    public static string GetEventTypeName(Events.MazeEventTypes.EventType eventType) => Events.MazeEventTypes.GetEventTypeName(eventType);
    public static UnityEngine.Color GetEventTypeColor(Events.MazeEventTypes.EventType eventType) => Events.MazeEventTypes.GetEventTypeColor(eventType);
    
    // Aliases para tipos antigos (deprecated)
    public enum EventType { TreasureHunt, EnemyRush, PowerSurge, TimeChallenge, BossSpawn, ResourceRush, MazeShift, LuckyStreak, SurvivalMode, PeacefulTime }
    public class GameEvent { } // Classe vazia para compatibilidade
} 