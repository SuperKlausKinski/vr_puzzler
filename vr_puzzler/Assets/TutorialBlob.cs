using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace VRPuzzler
{
    public class TutorialBlob : MonoBehaviour
    {

        public GameObject[] TutorialStep;
        public Animator TutorialBlob_Animator;
        public Animator Door_Animator;
        public AudioClip ClickSound;
        //---------------------------------------------------------------------
        private int m_currentTutorialStep;
        private UnityAction listenForChange;
        //---------------------------------------------------------------------
        void Awake()
        {
            foreach (GameObject _go in TutorialStep)
            {
                _go.SetActive(false);
            }
            TutorialBlob_Animator.gameObject.SetActive(false);
            listenForChange = new UnityAction(StateChanged);
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", listenForChange);
        }
        //---------------------------------------------------------------------
        void Start()
        {
            Debug.Log("START TUTORIAL OBJECT");
            
        }
        //---------------------------------------------------------------------
        public void CardClicked(string _cardType)
        {
            if (GameFSM.Instance.Gamestate != GameFSM.GAMESTATES.INTRO) { return; }
            /// if card type tutorial, play switch animation and replace card with the next one, increment tutorial steps by one
            if (_cardType == "TUTORIAL")
            {
                if (m_currentTutorialStep < TutorialStep.Length-1)
                {
                    TutorialBlob_Animator.SetTrigger("SWITCH");

                    // Shortly disable input
                    InputController.Instance.DisableInput();
                    DOVirtual.DelayedCall(1f, () => InputController.Instance.EnableInput());

                    // turn off old step, increment steps, turn on new
                    TutorialStep[m_currentTutorialStep].SetActive(false);
                    m_currentTutorialStep++;
                    TutorialStep[m_currentTutorialStep].SetActive(true);
                }
                else
                {
                    TutorialBlob_Animator.SetTrigger("HIDE");
                    InputController.Instance.DisableInput();
                    DOVirtual.DelayedCall(2.5f,()=> EventManager.Instance.InvokeEvent("TUTORIAL_COMPLETED"));                 
                }
            }
            else if (_cardType == "WIN")
            {
                // show you won card with restart message
            }
        }
        //---------------------------------------------------------------------
        private void StateChanged()
        {
            switch (GameFSM.Instance.Gamestate)
            {
                case (GameFSM.GAMESTATES.INTRO):
                    Debug.Log("START TUTORIAL");
                    StartTutorial();
                    break;
                case (GameFSM.GAMESTATES.GAME):
                    CleanUpTutorial();
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    StartWin();
                    break;
            }
        }
        //---------------------------------------------------------------------
        private void StartTutorial()
        {
            m_currentTutorialStep = 0;
            TutorialStep[m_currentTutorialStep].SetActive(true);
            
            Door_Animator.SetTrigger("OPEN");
            DOVirtual.DelayedCall(1f, () => TutorialBlob_Animator.gameObject.SetActive(true));
            DOVirtual.DelayedCall(1f, () => TutorialBlob_Animator.SetTrigger("SHOW"));
        }
        //---------------------------------------------------------------------
        private void CleanUpTutorial()
        {
            Door_Animator.SetTrigger("CLOSE");
            TutorialBlob_Animator.gameObject.SetActive(false);
        }
        //---------------------------------------------------------------------
        private void StartWin()
        {

        }
        //---------------------------------------------------------------------
    }
}
