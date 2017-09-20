using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

namespace VRPuzzler
{
    public class TutorialBlob : MonoBehaviour
    {
        public GameObject[] TutorialStep;
        public GameObject ContinueCard;
        public GameObject[] FinishStep;
        public Animator TutorialBlob_Animator;      
        public GameObject Door;
        public GvrAudioSource CardFlipAudioSource;
        public Canvas TutorialCanvas;
        public AudioClip ClickSound;
        public AudioClip DoorOpen;
        public AudioClip DoorClose;
        public AudioClip CardFlipAudioClip;
        public AudioClip Swosh;
        //---------------------------------------------------------------------
        private int m_currentCardStep;
        private GameObject[] m_currentStepsGameObjects;
        private UnityAction listenForChange;
        private UnityAction listenForSequenceComplete;
 
        private Animator m_door_Animator;
        private GvrAudioSource m_door_Audio;
        private Action m_callBack;
        //---------------------------------------------------------------------
        void Awake()
        {

            HideAllCards();
            TutorialBlob_Animator.gameObject.SetActive(false);
            listenForChange = new UnityAction(StateChanged);
            listenForSequenceComplete = new UnityAction(ShowContinueCard);
 
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", listenForChange);
            EventManager.Instance.StartListening("SEQUENCE_COMPLETED", listenForSequenceComplete);
            EventManager.Instance.StartListening("SEQUENCE_COMPLETED", listenForSequenceComplete);

        }
        //---------------------------------------------------------------------
        void Start()
        {      
            m_door_Animator = Door.GetComponent<Animator>();
            m_door_Audio = Door.GetComponent<GvrAudioSource>();
        }
        //---------------------------------------------------------------------
        public void PointerHover()
        {
            Debug.Log("Hover");
            TutorialBlob_Animator.SetTrigger("JUMP");
        }
        //---------------------------------------------------------------------
        public void CardClicked(string _cardType)
        {
           // if (GameFSM.Instance.Gamestate != GameFSM.GAMESTATES.INTRO||GameFSM.Instance.Gamestate != GameFSM.GAMESTATES.FINISH) { return; }
            /// if card type tutorial, play switch animation and replace card with the next one, increment tutorial steps by one
       
                if (m_currentCardStep < m_currentStepsGameObjects.Length-1)
                {
                                     
                    TutorialBlob_Animator.SetTrigger("SWITCH");

                    // Shortly disable input
                    
                    InputController.Instance.TutorialBlobInput(false);
                    DOVirtual.DelayedCall(1f, () => InputController.Instance.TutorialBlobInput(true));

                    // turn off old step, increment steps, turn on new
                    
                }
                else
                {
                    TutorialBlob_Animator.SetTrigger("HIDE");
           
                    InputController.Instance.TutorialBlobInput(false);
                    DOVirtual.DelayedCall(2.5f,()=> EventManager.Instance.InvokeEvent("TUTORIAL_COMPLETED"));
                if (GameFSM.Instance.Gamestate == GameFSM.GAMESTATES.FINISH)
                {
                    GameFSM.Instance.Gamestate = GameFSM.GAMESTATES.GAME;
                }                
                }
                
      
        }
        //---------------------------------------------------------------------
        public void ShowContinueCard()
        {
            TutorialBlob_Animator.speed = 2;
            m_door_Animator.speed = 2;
            TutorialBlob_Animator.gameObject.SetActive(true);
            OpenDoor();
            TutorialBlob_Animator.SetTrigger("SHOW");
            ContinueCard.SetActive(true);
            InputController.Instance.TutorialBlobInput(false);
        }
        //---------------------------------------------------------------------
        public void AnimationEnd(string _clip)
        {
            if(GameFSM.Instance.Gamestate == GameFSM.GAMESTATES.GAME)
            {
                if (_clip == "SHOW")
                {
                    TutorialBlob_Animator.SetTrigger("HIDE");
                }
                else if (_clip == "HIDE")
                {
                    CloseDoor();
                    ContinueCard.SetActive(false);
                   
                }
            }
        }
        //---------------------------------------------------------------------
        public void CardContentVisibility(int _alpha)
        {
            if(_alpha == 0)
            {
                m_currentStepsGameObjects[m_currentCardStep].SetActive(false);
                m_currentCardStep++;
                m_currentStepsGameObjects[m_currentCardStep].SetActive(true);
            }
           
            TutorialCanvas.GetComponent<CanvasGroup>().alpha = _alpha;
            if (m_currentCardStep < m_currentStepsGameObjects.Length)
            {
                CardFlipAudioSource.clip = CardFlipAudioClip;
                CardFlipAudioSource.Play();
            }
        }
        //---------------------------------------------------------------------
        public void PlaySwoshAudio()
        {
            CardFlipAudioSource.clip = Swosh;
            CardFlipAudioSource.Play();
        }
        //---------------------------------------------------------------------
        private void StateChanged()
        {
            switch (GameFSM.Instance.Gamestate)
            {
                case (GameFSM.GAMESTATES.INTRO):
                    m_currentStepsGameObjects = TutorialStep;
                    StartShowingCards();
                    break;
                case (GameFSM.GAMESTATES.GAME):
                    CleanUpTutorial();
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    m_currentStepsGameObjects = FinishStep;
                    StartShowingCards();
                    break;
            }
        }
        //---------------------------------------------------------------------
        private void StartShowingCards()
        {
                      
            m_currentCardStep = 0;
            TutorialBlob_Animator.speed = 1;
            m_door_Animator.speed = 1;
            OpenDoor();

            m_currentStepsGameObjects[m_currentCardStep].SetActive(true);
            Debug.Log(m_currentStepsGameObjects.Length);
            DOVirtual.DelayedCall(1f, () => TutorialBlob_Animator.gameObject.SetActive(true));
            DOVirtual.DelayedCall(1f, () => TutorialBlob_Animator.SetTrigger("SHOW"));
            InputController.Instance.TutorialBlobInput(true);
        }
        private void OpenDoor()
        {
            m_door_Audio.clip = DoorOpen;
            m_door_Audio.Play();
            m_door_Animator.SetTrigger("OPEN");
        }
        //---------------------------------------------------------------------
        private void CloseDoor()
        {
            m_door_Audio.clip = DoorClose;
            m_door_Audio.Play();
            m_door_Animator.SetTrigger("CLOSE");
        }
        //---------------------------------------------------------------------
        private void CleanUpTutorial()
        {
            CloseDoor();
            HideAllCards();
            TutorialBlob_Animator.gameObject.SetActive(false);
        }
        //---------------------------------------------------------------------
        private void HideAllCards()
        {
            foreach (GameObject _go in TutorialStep)
            {
                _go.SetActive(false);
            }
            foreach (GameObject _go in FinishStep)
            {
                _go.SetActive(false);
            }

            ContinueCard.SetActive(false);
        }
        //---------------------------------------------------------------------
    }
}
