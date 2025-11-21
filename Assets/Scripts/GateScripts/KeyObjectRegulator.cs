using UnityEngine;

namespace KeyNetwork
{
    public class KeyObjectRegulator : MonoBehaviour
    {
        [SerializeField] private bool key = false;
        [SerializeField] private bool Gate = false;

        [SerializeField] private KeyList keyList;

        private KeyGateRegulator gateObject;

        private void Start()
        {
            if (Gate)
            {
                gateObject = GetComponent<KeyGateRegulator>();
            }
        }

        public void foundObject()
        {
            if (key)
            {
                keyList.hasKey = true;

                // 🔹 Marcar también la variable local para verla en el Inspector
                key = true;

                Debug.Log("Llave recogida – KeyObjectRegulator.key = true");

                // Desactivar objeto visual
                gameObject.SetActive(false);
            }
            else if (Gate)
            {
                gateObject.StartAnimation();
            }
        }
    }
}


