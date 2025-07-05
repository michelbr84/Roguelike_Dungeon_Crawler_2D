// Assets/Scripts/Maze/MazePlayerUtils.cs
using UnityEngine;

/// <summary>
/// Orquestrador principal do sistema de jogador.
/// Delega tarefas para componentes especializados.
/// </summary>
public static class MazePlayerUtils
{
    /// <summary>
    /// Checa se a posição está dentro dos limites do maze.
    /// </summary>
    public static bool IsValid(ProceduralMaze mazeObj, Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < mazeObj.width && pos.y >= 0 && pos.y < mazeObj.height;
    }

    /// <summary>
    /// Entrada do jogador (movimento e disparo). Chamar no Update principal.
    /// </summary>
    public static void HandlePlayerInput(ProceduralMaze mazeObj)
    {
        MazePlayerMovement.HandleInput(mazeObj);
    }

    /// <summary>
    /// Move os projéteis (balas) disparados pelo player. Chamar em Update.
    /// </summary>
    public static void MoveBullets(ProceduralMaze mazeObj)
    {
        MazePlayerBullets.MoveBullets(mazeObj);
    }

    /// <summary>
    /// Colisões principais: se player colidiu com inimigo (considera shield).
    /// </summary>
    public static void HandleCollisions(ProceduralMaze mazeObj)
    {
        MazePlayerCollision.HandleCollisions(mazeObj);
    }

    /// <summary>
    /// Ativar proteção ao spawnar.
    /// </summary>
    public static void ActivateSpawnProtection()
    {
        MazePlayerProtection.ActivateSpawnProtection();
    }

    /// <summary>
    /// Verificar se está protegido ao spawnar.
    /// </summary>
    public static bool IsSpawnProtected()
    {
        return MazePlayerProtection.IsSpawnProtected();
    }
}
