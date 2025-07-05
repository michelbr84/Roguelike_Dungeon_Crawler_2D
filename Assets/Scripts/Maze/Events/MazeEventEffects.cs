namespace Events
{
    public static class MazeEventEffects
{
    public static void ApplyEventEffects(MazeGameEvent gameEvent)
    {
        // Os efeitos são aplicados através dos modificadores
        // que são consultados pelos outros sistemas
        // Exemplo: gameEvent.enemySpawnModifier é lido por MazeEnemyUtils
        // gameEvent.powerUpModifier é lido por MazePowerUpUtils, etc.
        
        // Aqui você pode adicionar lógica adicional se necessário:
        // - Notificar outros sistemas sobre mudanças
        // - Aplicar efeitos visuais
        // - Modificar configurações globais
    }

    public static void RemoveEventEffects(MazeGameEvent gameEvent)
    {
        // Remover efeitos aplicados
        // Volta ao normal ou remove modificadores
        
        // Aqui você pode adicionar lógica adicional se necessário:
        // - Notificar outros sistemas sobre o fim do evento
        // - Remover efeitos visuais
        // - Restaurar configurações originais
    }
}
} 