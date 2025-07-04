using UnityEngine;
using System.Collections.Generic;

public static class MazeVisualEffects
{
    // Efeitos de partículas simples (usando GUI)
    private static List<ParticleEffect> activeParticles = new List<ParticleEffect>();
    
    public class ParticleEffect
    {
        public Vector2 position;
        public Vector2 velocity;
        public float life;
        public float maxLife;
        public Color color;
        public float size;
        public ParticleType type;
        
        public ParticleEffect(Vector2 pos, Vector2 vel, float duration, Color col, float s, ParticleType t)
        {
            position = pos;
            velocity = vel;
            life = duration;
            maxLife = duration;
            color = col;
            size = s;
            type = t;
        }
    }
    
    public enum ParticleType
    {
        PowerUpCollect,
        EnemyDeath,
        PlayerHit,
        ShieldBlock,
        Teleport,
        ScorePopup
    }
    
    // Atualizar todos os efeitos visuais
    public static void UpdateEffects()
    {
        for (int i = activeParticles.Count - 1; i >= 0; i--)
        {
            var particle = activeParticles[i];
            particle.position += particle.velocity * Time.deltaTime;
            particle.life -= Time.deltaTime;
            
            // Aplicar gravidade para alguns tipos
            if (particle.type == ParticleType.EnemyDeath || particle.type == ParticleType.PlayerHit)
            {
                particle.velocity.y -= 50f * Time.deltaTime; // Gravidade
            }
            
            if (particle.life <= 0f)
            {
                activeParticles.RemoveAt(i);
            }
        }
    }
    
    // Renderizar todos os efeitos
    public static void RenderEffects()
    {
        foreach (var particle in activeParticles)
        {
            float alpha = particle.life / particle.maxLife;
            Color renderColor = particle.color;
            renderColor.a = alpha;
            
            GUIStyle style = new GUIStyle();
            style.normal.textColor = renderColor;
            style.fontSize = Mathf.RoundToInt(particle.size);
            style.alignment = TextAnchor.MiddleCenter;
            
            string symbol = GetParticleSymbol(particle.type);
            GUI.Label(new Rect(particle.position.x, particle.position.y, particle.size, particle.size), symbol, style);
        }
    }
    
    private static string GetParticleSymbol(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.PowerUpCollect: return "★";
            case ParticleType.EnemyDeath: return "✖";
            case ParticleType.PlayerHit: return "💥";
            case ParticleType.ShieldBlock: return "🛡";
            case ParticleType.Teleport: return "✨";
            case ParticleType.ScorePopup: return "+";
            default: return "•";
        }
    }
    
    // Criar efeito de coleta de power-up
    public static void CreatePowerUpCollectEffect(Vector2Int position, float cellSize)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 100f;
            Color color = new Color(1f, 1f, 0f, 1f); // Amarelo
            activeParticles.Add(new ParticleEffect(worldPos, velocity, 1.5f, color, 20f, ParticleType.PowerUpCollect));
        }
    }
    
    // Criar efeito de morte de inimigo
    public static void CreateEnemyDeathEffect(Vector2Int position, float cellSize)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 80f;
            Color color = new Color(1f, 0f, 0f, 1f); // Vermelho
            activeParticles.Add(new ParticleEffect(worldPos, velocity, 2f, color, 16f, ParticleType.EnemyDeath));
        }
    }
    
    // Criar efeito de hit do jogador
    public static void CreatePlayerHitEffect(Vector2Int position, float cellSize)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 60f;
            Color color = new Color(1f, 0.5f, 0f, 1f); // Laranja
            activeParticles.Add(new ParticleEffect(worldPos, velocity, 1.2f, color, 18f, ParticleType.PlayerHit));
        }
    }
    
    // Criar efeito de bloqueio de escudo
    public static void CreateShieldBlockEffect(Vector2Int position, float cellSize)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 120f;
            Color color = new Color(0f, 1f, 1f, 1f); // Ciano
            activeParticles.Add(new ParticleEffect(worldPos, velocity, 1f, color, 14f, ParticleType.ShieldBlock));
        }
    }
    
    // Criar efeito de teleport
    public static void CreateTeleportEffect(Vector2Int position, float cellSize)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        
        for (int i = 0; i < 16; i++)
        {
            float angle = i * 22.5f * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 150f;
            Color color = new Color(0.5f, 0f, 1f, 1f); // Roxo
            activeParticles.Add(new ParticleEffect(worldPos, velocity, 2.5f, color, 12f, ParticleType.Teleport));
        }
    }
    
    // Criar efeito de popup de score
    public static void CreateScorePopupEffect(Vector2Int position, float cellSize, int score)
    {
        Vector2 worldPos = new Vector2(position.x * cellSize, position.y * cellSize);
        Vector2 velocity = new Vector2(0f, -50f); // Move para cima
        Color color = new Color(0f, 1f, 0f, 1f); // Verde
        activeParticles.Add(new ParticleEffect(worldPos, velocity, 2f, color, 24f, ParticleType.ScorePopup));
    }
    
    // Limpar todos os efeitos
    public static void ClearAllEffects()
    {
        activeParticles.Clear();
    }
} 