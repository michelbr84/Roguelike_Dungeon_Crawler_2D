using UnityEngine;

public static class MazePlayerRenderer
{
    public static void DrawPlayer(ProceduralMaze mazeObj, float cellSize)
    {
        Rect cellRect = new Rect(mazeObj.playerPos.x * cellSize, mazeObj.playerPos.y * cellSize, cellSize, cellSize);

        float angle = 0;
        if      (mazeObj.playerDir == Vector2Int.up)    angle = 0f;
        else if (mazeObj.playerDir == Vector2Int.right) angle = 90f;
        else if (mazeObj.playerDir == Vector2Int.down)  angle = 180f;
        else if (mazeObj.playerDir == Vector2Int.left)  angle = 270f;

        // Efeito especial: shield ativo (contorno piscante)
        bool showShield = mazeObj.shieldActive && mazeObj.shieldTimer > 0f;

        // Animação breve ao coletar power-up (ex: escala)
        bool isPowerUpFlash = mazeObj.powerUpFlashTimer > 0f;
        float scale = 1f;
        if (isPowerUpFlash)
            scale = 1.25f - (mazeObj.powerUpFlashTimer / mazeObj.powerUpFlashDuration) * 0.25f;

        Matrix4x4 matrixBackup = GUI.matrix;
        Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);

        // Gira
        GUIUtility.RotateAroundPivot(angle, pivot);
        // Escala
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), pivot);

        // Shield visual: círculo de cor e/ou contorno piscando
        if (showShield)
        {
            float pulse = 0.7f + 0.3f * Mathf.Sin(Time.time * 10f);
            Color shieldCol = new Color(0.3f, 0.9f, 1.0f, pulse);
            Color prevCol = GUI.color;
            GUI.color = shieldCol;
            float radius = cellRect.width * 0.62f * scale;
            GUI.DrawTexture(new Rect(pivot.x - radius / 2, pivot.y - radius / 2, radius, radius), Texture2D.whiteTexture, ScaleMode.ScaleToFit);
            GUI.color = prevCol;
        }

        // Efeito de invisibilidade
        bool isInvisible = mazeObj.invisibilityActive && mazeObj.invisibilityTimer > 0f;
        if (isInvisible)
        {
            float alpha = 0.3f + 0.4f * Mathf.Sin(Time.time * 8f); // Piscada suave
            Color invisColor = new Color(1f, 1f, 1f, alpha);
            Color prevCol = GUI.color;
            GUI.color = invisColor;
            if (mazeObj.playerTexture)
            {
                GUI.DrawTexture(cellRect, mazeObj.playerTexture, ScaleMode.ScaleToFit);
            }
            else
            {
                Vector2 center = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                float size = cellRect.width * 0.35f * scale;
                Vector2[] verts = new Vector2[3];
                verts[0] = center + Rotate2D(new Vector2(0, -size), angle);
                verts[1] = center + Rotate2D(new Vector2(-size * 0.9f, size * 0.9f), angle);
                verts[2] = center + Rotate2D(new Vector2(size * 0.9f, size * 0.9f), angle);
                GUI.color = new Color(0.5f, 0.5f, 0.5f, alpha);
                for (int i = 0; i < 3; i++)
                {
                    GUI.DrawTexture(new Rect(verts[i].x - size * 0.12f, verts[i].y - size * 0.12f, size * 0.24f, size * 0.24f), Texture2D.whiteTexture);
                }
            }
            GUI.color = prevCol;
        }
        else
        {
            // Player sprite normal
            if (mazeObj.playerTexture)
            {
                GUI.DrawTexture(cellRect, mazeObj.playerTexture, ScaleMode.ScaleToFit);
            }
            else
            {
                Vector2 center = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                float size = cellRect.width * 0.35f * scale;
                Vector2[] verts = new Vector2[3];
                verts[0] = center + Rotate2D(new Vector2(0, -size), angle);
                verts[1] = center + Rotate2D(new Vector2(-size * 0.9f, size * 0.9f), angle);
                verts[2] = center + Rotate2D(new Vector2(size * 0.9f, size * 0.9f), angle);
                Color oldColor = GUI.color;
                GUI.color = isPowerUpFlash ? Color.yellow : mazeObj.playerColor;
                for (int i = 0; i < 3; i++)
                {
                    GUI.DrawTexture(new Rect(verts[i].x - size * 0.12f, verts[i].y - size * 0.12f, size * 0.24f, size * 0.24f), Texture2D.whiteTexture);
                }
                GUI.color = oldColor;
            }
        }
        GUI.matrix = matrixBackup;
    }

    private static Vector2 Rotate2D(Vector2 v, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float ca = Mathf.Cos(rad), sa = Mathf.Sin(rad);
        return new Vector2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
    }
} 