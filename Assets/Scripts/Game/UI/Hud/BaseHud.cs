using UnityEngine;

namespace Game.UI.Hud
{
    /// <summary>
    /// Classe base para todos os HUDs do jogo.
    /// </summary>
    public class BaseHud : MonoBehaviour, IHud
    {
        [Header("Exemplo de campo HUD")]
        public CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        /// <summary>
        /// Atualiza o HUD (override em filhos).
        /// </summary>
        public virtual void UpdateHud()
        {
            // Implemente nos filhos conforme necessário.
        }

        /// <summary>
        /// Ativa ou desativa o HUD.
        /// </summary>
        public virtual void SetActive(bool value)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = value ? 1 : 0;
                canvasGroup.interactable = value;
                canvasGroup.blocksRaycasts = value;
            }
            else
            {
                gameObject.SetActive(value);
            }
        }
    }
}
