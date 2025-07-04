using UnityEngine;

namespace Game
{
    /// <summary>
    /// Controlador de câmera simples para seguir o jogador.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Configurações de Câmera")]
        public Transform target;      // Jogador ou objeto a seguir
        public Vector3 offset = new Vector3(0, 10, -10);
        public float smoothSpeed = 0.125f;

        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;

                transform.LookAt(target);
            }
        }

        /// <summary>
        /// Define o alvo a ser seguido pela câmera.
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
