namespace Game.UI.Hud
{
    /// <summary>
    /// Interface base para componentes de HUD.
    /// </summary>
    public interface IHud
    {
        /// <summary>
        /// Atualiza a interface do HUD (override nos filhos).
        /// </summary>
        void UpdateHud();

        /// <summary>
        /// Define se o HUD está ativo ou não.
        /// </summary>
        /// <param name="value">Ativo?</param>
        void SetActive(bool value);
    }
}
