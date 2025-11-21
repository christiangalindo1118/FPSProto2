using UnityEngine;
using System.Collections;


namespace KeyNetwork
{
    public class KeyGateRegulator : MonoBehaviour
    {
        [Header("Animations")] private Animator gateAnimation;
        private bool OpenGate = false;
        [SerializeField] private string openAnimationName = "GateOpen";
        [SerializeField] private string closeAnimationName = "GateClose";

        [Header("Time and UI")] [SerializeField]
        private int timeToShowUI = 1;

        [SerializeField] private GameObject showGateLockedUI = null;
        [SerializeField] private KeyList keyList = null;
        [SerializeField] private int waitTimer = 2;
        
        [SerializeField] private bool pauseInteraction = false;

        private void Awake()
        {
            gateAnimation = gameObject.GetComponent<Animator>();
        }

        public void StartAnimation()
        {
            if (keyList.hasKey)
            {
                Open_Gate();
            }
            else
            {
                StartCoroutine(showGateLocked()); 
            }
        }

        private IEnumerator StopGateInterconnection()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;

        }

        void Open_Gate()
        {
            if (!OpenGate && !pauseInteraction)
            {
                gateAnimation.Play(openAnimationName, 0, 0.0f);
                OpenGate = true;
                StartCoroutine(StopGateInterconnection());
            }
            else if (OpenGate && !pauseInteraction)
            {
                gateAnimation.Play(closeAnimationName, 0, 0.0f);
                OpenGate = false;
                StartCoroutine(StopGateInterconnection());
            }
        }

        IEnumerator showGateLocked()
        {
            showGateLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showGateLockedUI.SetActive(false);
        }
        
    }
}
