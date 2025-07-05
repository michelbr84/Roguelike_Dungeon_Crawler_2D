using UnityEngine;

public static class MazeEnemyRenderer
{
    public static void DrawEnemies(ProceduralMaze mazeObj, float cellSize)
    {
        // Inimigos normais com rotação baseada na direção
        for (int i = 0; i < mazeObj.enemies.Count; i++)
        {
            var e = mazeObj.enemies[i];
            Rect cellRect = new Rect(e.x * cellSize, e.y * cellSize, cellSize, cellSize);
            float rotation = MazeEnemyUtils.GetEnemyRotation(i);
            Matrix4x4 oldMatrix = GUI.matrix;
            if (mazeObj.enemyTexture)
            {
                Vector2 pivot = new Vector2(cellRect.x + cellRect.width / 2, cellRect.y + cellRect.height / 2);
                GUIUtility.RotateAroundPivot(rotation, pivot);
                GUI.DrawTexture(cellRect, mazeObj.enemyTexture, ScaleMode.ScaleToFit);
                GUI.matrix = oldMatrix;
            }
            else
            {
                Color oldColor = GUI.color;
                GUI.color = mazeObj.enemyColor;
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
            if (enemy.direction == Vector2Int.up) angle = 180f;
            else if (enemy.direction == Vector2Int.right) angle = 90f;
            else if (enemy.direction == Vector2Int.down) angle = 0f;
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
                GUI.color = Color.red;
                GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth, healthBarHeight), Texture2D.whiteTexture);
                GUI.color = Color.green;
                float healthPercent = enemy.health / enemy.maxHealth;
                GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth * healthPercent, healthBarHeight), Texture2D.whiteTexture);
            }
            GUI.color = oldColor;
        }
    }

    private static Color GetAdvancedEnemyColor(MazeAdvancedEnemies.AdvancedEnemyType type)
    {
        switch (type)
        {
            case MazeAdvancedEnemies.AdvancedEnemyType.Boss: return new Color(0.8f, 0.2f, 0.2f);
            case MazeAdvancedEnemies.AdvancedEnemyType.Sniper: return new Color(0.2f, 0.8f, 0.2f);
            case MazeAdvancedEnemies.AdvancedEnemyType.Kamikaze: return new Color(0.8f, 0.8f, 0.2f);
            case MazeAdvancedEnemies.AdvancedEnemyType.Spawner: return new Color(0.8f, 0.2f, 0.8f);
            default: return Color.white;
        }
    }

    private static string GetAdvancedEnemySymbol(MazeAdvancedEnemies.AdvancedEnemyType type)
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