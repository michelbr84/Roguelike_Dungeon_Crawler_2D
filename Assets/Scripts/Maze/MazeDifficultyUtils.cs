// Assets/Scripts/Maze/MazeDifficultyUtils.cs
using UnityEngine;

public static class MazeDifficultyUtils
{
    /// <summary>
    /// Ajusta a dificuldade do labirinto dinamicamente conforme o nível atual.
    /// Se 'reset' for true, volta para os valores iniciais do jogo.
    /// </summary>
    /// <param name="mazeObj">Referência para o ProceduralMaze atual</param>
    /// <param name="reset">Se true, reseta todos os parâmetros de dificuldade</param>
    public static void AdjustDifficulty(ProceduralMaze mazeObj, bool reset)
    {
        if (reset)
        {
            mazeObj.width = 10;
            mazeObj.height = 10;
            mazeObj.enemyCount = 3;
            mazeObj.enemyMoveInterval = 0.8f;
            return;
        }

        // Aumenta tamanho do labirinto a cada 2 níveis até um máximo
        if (mazeObj.currentLevel % 2 == 0)
        {
            mazeObj.width = Mathf.Min(mazeObj.width + 2, 20);
            mazeObj.height = Mathf.Min(mazeObj.height + 2, 20);
        }
        // Aumenta o número de inimigos gradualmente, até um máximo
        mazeObj.enemyCount = Mathf.Min(mazeObj.enemyCount + 1, 15);

        // Deixa inimigos mais rápidos a cada nível, até o limite mínimo
        mazeObj.enemyMoveInterval = Mathf.Max(mazeObj.enemyMoveInterval - 0.06f, 0.22f);
    }
}
