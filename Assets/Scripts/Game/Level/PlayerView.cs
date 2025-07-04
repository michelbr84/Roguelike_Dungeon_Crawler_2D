using UnityEngine;

namespace Game.Level
{
    /// <summary>
    /// Controla o jogador e sua lógica principal.
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        [Header("Configurações do Jogador")]
        public float moveSpeed = 5f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                _rb.useGravity = false; // Exemplo: para 2D/TopDown
            }
        }

        private void Update()
        {
            // Exemplo de movimentação simples (WASD/arrows)
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            _rb.linearVelocity = dir * moveSpeed;
        }

        /// <summary>
        /// Método de exemplo para ações do jogador.
        /// </summary>
        public void Jump()
        {
            if (_rb != null)
                _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
