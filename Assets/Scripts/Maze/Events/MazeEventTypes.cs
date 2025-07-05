using UnityEngine;

namespace Events
{
    public static class MazeEventTypes
{
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

    public static Color GetEventTypeColor(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.TreasureHunt: return Color.yellow;
            case EventType.EnemyRush: return Color.red;
            case EventType.PowerSurge: return Color.cyan;
            case EventType.TimeChallenge: return new Color(1f, 0.6f, 0.1f); // Laranja
            case EventType.BossSpawn: return Color.magenta;
            case EventType.ResourceRush: return Color.green;
            case EventType.MazeShift: return Color.blue;
            case EventType.LuckyStreak: return Color.yellow;
            case EventType.SurvivalMode: return Color.red;
            case EventType.PeacefulTime: return Color.green;
            default: return Color.white;
        }
    }
}
} 