using UnityEngine;
using System.Collections.Generic;

public static class MazeShaderEffects
{
    // Efeitos de glow
    private static Dictionary<Vector2Int, GlowEffect> activeGlows = new Dictionary<Vector2Int, GlowEffect>();
    
    public class GlowEffect
    {
        public Vector2Int position;
        public Color color;
        public float intensity;
        public float maxIntensity;
        public float duration;
        public float maxDuration;
        public GlowType type;
        
        public GlowEffect(Vector2Int pos, Color col, float intens, float dur, GlowType t)
        {
            position = pos;
            color = col;
            intensity = intens;
            maxIntensity = intens;
            duration = dur;
            maxDuration = dur;
            type = t;
        }
    }
    
    public enum GlowType
    {
        PowerUp,
        Player,
        Boss,
        Warning
    }
    
    // Atualizar todos os efeitos
    public static void UpdateEffects()
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();
        
        foreach (var kvp in activeGlows)
        {
            var glow = kvp.Value;
            glow.duration -= Time.deltaTime;
            
            if (glow.duration <= 0f)
            {
                toRemove.Add(kvp.Key);
            }
            else
            {
                // Pulsar intensidade
                float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 8f);
                glow.intensity = glow.maxIntensity * pulse;
            }
        }
        
        foreach (var pos in toRemove)
        {
            activeGlows.Remove(pos);
        }
    }
    
    // Renderizar efeitos de glow
    public static void RenderGlowEffects(float cellSize)
    {
        foreach (var glow in activeGlows.Values)
        {
            RenderGlow(glow, cellSize);
        }
    }
    
    // Renderizar um efeito de glow específico
    private static void RenderGlow(GlowEffect glow, float cellSize)
    {
        Rect cellRect = new Rect(glow.position.x * cellSize, glow.position.y * cellSize, cellSize, cellSize);
        
        // Calcular tamanho do glow baseado na intensidade
        float glowSize = cellSize * (1f + glow.intensity * 0.5f);
        float glowOffset = (glowSize - cellSize) / 2f;
        
        Rect glowRect = new Rect(
            cellRect.x - glowOffset,
            cellRect.y - glowOffset,
            glowSize,
            glowSize
        );
        
        // Renderizar glow
        Color glowColor = glow.color;
        glowColor.a = glow.intensity * 0.3f;
        
        Color oldColor = GUI.color;
        GUI.color = glowColor;
        
        // Múltiplas camadas para efeito de glow
        for (int i = 0; i < 3; i++)
        {
            float layerSize = glowSize + i * 4f;
            float layerOffset = (layerSize - cellSize) / 2f;
            Rect layerRect = new Rect(
                cellRect.x - layerOffset,
                cellRect.y - layerOffset,
                layerSize,
                layerSize
            );
            
            float layerAlpha = glow.intensity * 0.1f / (i + 1);
            Color layerColor = glow.color;
            layerColor.a = layerAlpha;
            GUI.color = layerColor;
            
            GUI.DrawTexture(layerRect, Texture2D.whiteTexture);
        }
        
        GUI.color = oldColor;
    }
    
    // Criar efeito de glow
    public static void CreateGlowEffect(Vector2Int position, Color color, float intensity, float duration, GlowType type)
    {
        activeGlows[position] = new GlowEffect(position, color, intensity, duration, type);
    }
    
    // Criar efeito de glow de power-up
    public static void CreatePowerUpGlow(Vector2Int position)
    {
        CreateGlowEffect(position, Color.yellow, 1f, 2f, GlowType.PowerUp);
    }
    
    // Criar efeito de glow do jogador
    public static void CreatePlayerGlow(Vector2Int position)
    {
        CreateGlowEffect(position, Color.cyan, 0.8f, 1f, GlowType.Player);
    }
    
    // Criar efeito de glow de boss
    public static void CreateBossGlow(Vector2Int position)
    {
        CreateGlowEffect(position, Color.red, 1.2f, 3f, GlowType.Boss);
    }
    
    // Criar efeito de glow de aviso
    public static void CreateWarningGlow(Vector2Int position)
    {
        CreateGlowEffect(position, Color.orange, 1f, 1.5f, GlowType.Warning);
    }
    
    // Efeitos de distorção de tela
    private static float screenShakeIntensity = 0f;
    private static float screenShakeDuration = 0f;
    
    // Aplicar shake na tela
    public static void ApplyScreenShake(float intensity, float duration)
    {
        screenShakeIntensity = intensity;
        screenShakeDuration = duration;
    }
    
    // Obter offset de shake
    public static Vector2 GetScreenShakeOffset()
    {
        if (screenShakeDuration <= 0f)
            return Vector2.zero;
            
        screenShakeDuration -= Time.deltaTime;
        
        if (screenShakeDuration <= 0f)
        {
            screenShakeIntensity = 0f;
            return Vector2.zero;
        }
        
        float shake = screenShakeIntensity * (screenShakeDuration / 0.5f);
        return new Vector2(
            Mathf.Sin(Time.time * 50f) * shake,
            Mathf.Cos(Time.time * 30f) * shake
        );
    }
    
    // Efeitos de partículas avançadas
    private static List<ParticleTrail> particleTrails = new List<ParticleTrail>();
    
    public class ParticleTrail
    {
        public Vector2 position;
        public Vector2 velocity;
        public float life;
        public float maxLife;
        public Color color;
        public float size;
        public TrailType type;
        
        public ParticleTrail(Vector2 pos, Vector2 vel, float duration, Color col, float s, TrailType t)
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
    
    public enum TrailType
    {
        Player,
        Bullet,
        Enemy
    }
    
    // Atualizar trilhas de partículas
    public static void UpdateParticleTrails()
    {
        for (int i = particleTrails.Count - 1; i >= 0; i--)
        {
            var trail = particleTrails[i];
            trail.position += trail.velocity * Time.deltaTime;
            trail.life -= Time.deltaTime;
            
            if (trail.life <= 0f)
            {
                particleTrails.RemoveAt(i);
            }
        }
    }
    
    // Renderizar trilhas de partículas
    public static void RenderParticleTrails()
    {
        foreach (var trail in particleTrails)
        {
            float alpha = trail.life / trail.maxLife;
            Color renderColor = trail.color;
            renderColor.a = alpha;
            
            GUIStyle style = new GUIStyle();
            style.normal.textColor = renderColor;
            style.fontSize = Mathf.RoundToInt(trail.size);
            style.alignment = TextAnchor.MiddleCenter;
            
            string symbol = GetTrailSymbol(trail.type);
            GUI.Label(new Rect(trail.position.x, trail.position.y, trail.size, trail.size), symbol, style);
        }
    }
    
    private static string GetTrailSymbol(TrailType type)
    {
        switch (type)
        {
            case TrailType.Player: return "✨";
            case TrailType.Bullet: return "•";
            case TrailType.Enemy: return "💫";
            default: return "•";
        }
    }
    
    // Criar trilha de partícula
    public static void CreateParticleTrail(Vector2 position, Vector2 velocity, Color color, TrailType type)
    {
        particleTrails.Add(new ParticleTrail(position, velocity, 0.5f, color, 12f, type));
    }
    
    // Limpar todos os efeitos
    public static void ClearAllEffects()
    {
        activeGlows.Clear();
        particleTrails.Clear();
        screenShakeIntensity = 0f;
        screenShakeDuration = 0f;
    }
} 