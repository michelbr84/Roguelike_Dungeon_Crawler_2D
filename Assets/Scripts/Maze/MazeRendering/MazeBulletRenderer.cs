using UnityEngine;

public static class MazeBulletRenderer
{
    public static void DrawBullets(ProceduralMaze mazeObj, float cellSize)
    {
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
    }
} 