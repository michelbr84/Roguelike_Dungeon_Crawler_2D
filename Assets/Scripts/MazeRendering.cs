using UnityEngine;
using System.Collections.Generic;

public static class MazeRendering
{
    public static void DrawMaze(ProceduralMaze mazeObj)
    {
        float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
        
        // Aplicar screen shake
        Vector2 shakeOffset = MazeShaderEffects.GetScreenShakeOffset();
        Matrix4x4 shakeMatrix = GUI.matrix;
        if (shakeOffset != Vector2.zero)
        {
            GUI.matrix = Matrix4x4.TRS(shakeOffset, Quaternion.identity, Vector3.one);
        }

        // Fundo
        if (mazeObj.backgroundTexture)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mazeObj.backgroundTexture, ScaleMode.StretchToFill);
        }
        else
        {
            Color oldBG = GUI.color;
            GUI.color = mazeObj.backgroundColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = oldBG;
        }

        // Grid, exit e paredes
        for (int x = 0; x < mazeObj.width; x++)
        for (int y = 0; y < mazeObj.height; y++)
        {
            Rect cellRect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);
            bool isExit = (mazeObj.exitPos.x == x && mazeObj.exitPos.y == y);

            if (isExit)
            {
                if (mazeObj.exitTexture)
                {
                    GUI.DrawTexture(cellRect, mazeObj.exitTexture, ScaleMode.ScaleToFit);
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = mazeObj.exitColor;
                    GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }
            }
            else if (mazeObj.maze[x, y] == 1)
            {
                if (mazeObj.wallTexture)
                {
                    GUI.DrawTexture(cellRect, mazeObj.wallTexture, ScaleMode.ScaleToFit);
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = mazeObj.wallColor;
                    GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }
            }
        }

        // --- POWER UPS ---
        if (mazeObj.powerUps != null)
        {
            foreach (var pu in mazeObj.powerUps)
            {
                Rect cellRect = new Rect(pu.position.x * cellSize, pu.position.y * cellSize, cellSize, cellSize);
                Color puColor = Color.white;
                Texture2D puTexture = null;
                switch (pu.type)
                {
                    case PowerUpType.Ammo:
                        puColor = Color.cyan;
                        puTexture = mazeObj.ammoTexture;
                        break;
                    case PowerUpType.Life:
                        puColor = Color.red;
                        puTexture = mazeObj.extraLifeTexture;
                        break;
                    case PowerUpType.Shield:
                        puColor = Color.green;
                        puTexture = mazeObj.shieldTexture;
                        break;
                    case PowerUpType.DoubleShot:
                        puColor = Color.yellow;
                        puTexture = mazeObj.doubleShotTexture;
                        break;
                    case PowerUpType.TripleShot:
                        puColor = Color.magenta;
                        puTexture = mazeObj.tripleShotTexture;
                        break;
                    case PowerUpType.SpeedBoost:
                        puColor = Color.cyan;
                        puTexture = mazeObj.speedBoostTexture;
                        break;
                    case PowerUpType.Invisibility:
                        puColor = Color.gray;
                        puTexture = mazeObj.invisibilityTexture;
                        break;
                    case PowerUpType.Teleport:
                        puColor = Color.blue;
                        puTexture = mazeObj.teleportTexture;
                        break;
                    case PowerUpType.ScoreBooster:
                        puColor = Color.green;
                        puTexture = mazeObj.scoreBoosterTexture;
                        break;
                    default:
                        puColor = Color.white;
                        break;
                }
                if (puTexture)
                {
                    GUI.DrawTexture(cellRect, puTexture, ScaleMode.ScaleToFit);
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = puColor;
                    GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }
            }
        }

        // Inimigos normais com rotação baseada na direção
        for (int i = 0; i < mazeObj.enemies.Count; i++)
        {
            var e = mazeObj.enemies[i];
            Rect cellRect = new Rect(e.x * cellSize, e.y * cellSize, cellSize, cellSize);
            
            // Obter rotação baseada na direção do inimigo
            float rotation = MazeEnemyUtils.GetEnemyRotation(i);
            
            Matrix4x4 oldMatrix = GUI.matrix;
            if (mazeObj.enemyTexture)
            {
                // Aplicar rotação
                Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                GUIUtility.RotateAroundPivot(rotation, pivot);
                GUI.DrawTexture(cellRect, mazeObj.enemyTexture, ScaleMode.ScaleToFit);
                GUI.matrix = oldMatrix;
            }
            else
            {
                Color oldColor = GUI.color;
                GUI.color = mazeObj.enemyColor;
                
                // Aplicar rotação
                Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                GUIUtility.RotateAroundPivot(rotation, pivot);
                GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                GUI.matrix = oldMatrix;
                GUI.color = oldColor;
            }
        }
        
        // Inimigos avançados
        var advancedEnemies = MazeAdvancedEnemies.GetAdvancedEnemies();
        foreach (var enemy in advancedEnemies)
        {
            Rect cellRect = new Rect(enemy.position.x * cellSize, enemy.position.y * cellSize, cellSize, cellSize);
            Color enemyColor = GetAdvancedEnemyColor(enemy.type);
            Texture2D enemyTexture = null;
            switch (enemy.type)
            {
                case MazeAdvancedEnemies.AdvancedEnemyType.Boss:
                    enemyTexture = mazeObj.bossTexture;
                    break;
                case MazeAdvancedEnemies.AdvancedEnemyType.Sniper:
                    enemyTexture = mazeObj.sniperTexture;
                    break;
                case MazeAdvancedEnemies.AdvancedEnemyType.Kamikaze:
                    enemyTexture = mazeObj.kamikazeTexture;
                    break;
                case MazeAdvancedEnemies.AdvancedEnemyType.Spawner:
                    enemyTexture = mazeObj.spawnerTexture;
                    break;
            }
            Color oldColor = GUI.color;
            GUI.color = enemyColor;
            float angle = 0f;
            if (enemy.direction == Vector2Int.up) angle = 180f; // Corrigido: subir = 180° (olha para cima)
            else if (enemy.direction == Vector2Int.right) angle = 90f;
            else if (enemy.direction == Vector2Int.down) angle = 0f; // Corrigido: descer = 0° (olha para baixo)
            else if (enemy.direction == Vector2Int.left) angle = 270f;
            Matrix4x4 matrixBackup = GUI.matrix;
            Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
            GUIUtility.RotateAroundPivot(angle, pivot);
            if (enemyTexture)
            {
                GUI.DrawTexture(cellRect, enemyTexture, ScaleMode.ScaleToFit);
            }
            else
            {
                GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                // Desenhar símbolo do inimigo
                GUIStyle style = new GUIStyle();
                style.fontSize = Mathf.RoundToInt(cellSize * 0.6f);
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = Color.white;
                string enemySymbol = GetAdvancedEnemySymbol(enemy.type);
                GUI.Label(cellRect, enemySymbol, style);
            }
            GUI.matrix = matrixBackup;
            // Barra de vida para inimigos com mais de 1 HP
            if (enemy.maxHealth > 1)
            {
                float healthBarWidth = cellSize * 0.8f;
                float healthBarHeight = 4f;
                float healthBarX = cellRect.x + (cellSize - healthBarWidth) / 2;
                float healthBarY = cellRect.y + cellSize - 8f;
                // Fundo da barra
                GUI.color = Color.red;
                GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth, healthBarHeight), Texture2D.whiteTexture);
                // Vida atual
                GUI.color = Color.green;
                float healthPercent = enemy.health / enemy.maxHealth;
                GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth * healthPercent, healthBarHeight), Texture2D.whiteTexture);
            }
            GUI.color = oldColor;
        }

        // Player desenhado no topo das paredes, com efeitos especiais!
        DrawPlayer(mazeObj, cellSize);

        // Tiros
        foreach (var b in mazeObj.bullets)
        {
            Rect cellRect = new Rect(
                b.pos.x * cellSize + cellSize * 0.25f,
                b.pos.y * cellSize + cellSize * 0.25f,
                cellSize * 0.5f,
                cellSize * 0.5f
            );

            bool isVertical = b.dir.y != 0;
            bool isLeft = b.dir.x < 0;

            Matrix4x4 bulletMatrix = GUI.matrix;
            if (mazeObj.bulletTexture)
            {
                if (isVertical)
                {
                    Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                    GUIUtility.RotateAroundPivot(90f, pivot);
                }
                else if (isLeft)
                {
                    Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                    GUIUtility.ScaleAroundPivot(new Vector2(-1f, 1f), pivot);
                }
                GUI.DrawTexture(cellRect, mazeObj.bulletTexture, ScaleMode.ScaleToFit);
                GUI.matrix = bulletMatrix;
            }
            else
            {
                Color oldColor = GUI.color;
                GUI.color = mazeObj.bulletColor;

                if (isVertical)
                {
                    Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                    GUIUtility.RotateAroundPivot(90f, pivot);
                }
                else if (isLeft)
                {
                    Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                    GUIUtility.ScaleAroundPivot(new Vector2(-1f, 1f), pivot);
                }
                GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                GUI.matrix = bulletMatrix;
                GUI.color = oldColor;
            }
        }

        // Efeitos de shader (glow e partículas)
        MazeShaderEffects.RenderGlowEffects(cellSize);
        MazeShaderEffects.RenderParticleTrails();
        
        // Efeitos visuais (partículas)
        MazeVisualEffects.RenderEffects();
        
        // Controles touch (apenas se habilitados)
        MazeTouchControls.RenderTouchControls();
        
        // HUD
        MazeHUD.DrawHUD(mazeObj);
        MazeHUD.DrawMissions();
        
        // Menu de configurações (renderizar por cima de tudo)
        MazeSettingsMenu.RenderMenu();
        
        // Reset da matriz após screen shake
        if (shakeOffset != Vector2.zero)
        {
            GUI.matrix = shakeMatrix;
        }
    }

    // Desenha o player com visual especial para shield e animação de power-up
    static void DrawPlayer(ProceduralMaze mazeObj, float cellSize)
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

    static Vector2 Rotate2D(Vector2 v, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float ca = Mathf.Cos(rad), sa = Mathf.Sin(rad);
        return new Vector2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
    }
    
    // Obter cor do inimigo avançado
    static Color GetAdvancedEnemyColor(MazeAdvancedEnemies.AdvancedEnemyType type)
    {
        switch (type)
        {
            case MazeAdvancedEnemies.AdvancedEnemyType.Boss: return new Color(0.8f, 0.2f, 0.2f); // Vermelho escuro
            case MazeAdvancedEnemies.AdvancedEnemyType.Sniper: return new Color(0.2f, 0.8f, 0.2f); // Verde
            case MazeAdvancedEnemies.AdvancedEnemyType.Kamikaze: return new Color(0.8f, 0.8f, 0.2f); // Amarelo
            case MazeAdvancedEnemies.AdvancedEnemyType.Spawner: return new Color(0.8f, 0.2f, 0.8f); // Magenta
            default: return Color.white;
        }
    }
    
    // Obter símbolo do inimigo avançado
    static string GetAdvancedEnemySymbol(MazeAdvancedEnemies.AdvancedEnemyType type)
    {
        switch (type)
        {
            case MazeAdvancedEnemies.AdvancedEnemyType.Boss: return "👹";
            case MazeAdvancedEnemies.AdvancedEnemyType.Sniper: return "🎯";
            case MazeAdvancedEnemies.AdvancedEnemyType.Kamikaze: return "💣";
            case MazeAdvancedEnemies.AdvancedEnemyType.Spawner: return "🕷️";
            default: return "?";
        }
    }
}
