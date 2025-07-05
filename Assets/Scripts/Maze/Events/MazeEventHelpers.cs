using UnityEngine;
using System.Collections.Generic;

namespace Events
{
    public static class MazeEventHelpers
{
    public static MazeEventTypes.EventType GenerateRandomEventReward()
    {
        var allTypes = (MazeEventTypes.EventType[])System.Enum.GetValues(typeof(MazeEventTypes.EventType));
        return allTypes[Random.Range(0, allTypes.Length)];
    }

    public static void UnlockEvent(List<MazeGameEvent> availableEvents, MazeEventTypes.EventType type)
    {
        foreach (var gameEvent in availableEvents)
            if (gameEvent.type == type) return;
        
        availableEvents.Add(new MazeGameEvent(type));
        string eventName = MazeEventTypes.GetEventTypeName(type);
        MazeHUD.ShowStatusMessage($"Novo evento desbloqueado: {eventName}!");
    }

    // Gerar evento aleatório (alias para compatibilidade)
    public static void GenerateRandomWeather()
    {
        // Este método é um alias para compatibilidade com o sistema de clima
        // Na verdade, eventos são gerados automaticamente pelo sistema
        // Pode ser chamado por outros sistemas que esperam este método
    }
}
} 