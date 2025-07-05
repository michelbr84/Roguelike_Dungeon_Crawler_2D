using UnityEngine;

public static class MazeVisualEffectRenderer
{
    public static void ApplyScreenShake()
    {
        Vector2 shakeOffset = MazeShaderEffects.GetScreenShakeOffset();
        if (shakeOffset != Vector2.zero)
        {
            GUI.matrix = Matrix4x4.TRS(shakeOffset, Quaternion.identity, Vector3.one);
        }
    }

    public static void RenderGlowAndParticles(float cellSize)
    {
        MazeShaderEffects.RenderGlowEffects(cellSize);
        MazeShaderEffects.RenderParticleTrails();
        MazeVisualEffects.RenderEffects();
    }

    public static void ResetScreenShake(Matrix4x4 shakeMatrix, Vector2 shakeOffset)
    {
        if (shakeOffset != Vector2.zero)
        {
            GUI.matrix = shakeMatrix;
        }
    }
} 