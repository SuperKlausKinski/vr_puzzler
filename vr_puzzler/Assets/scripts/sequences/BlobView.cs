using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace VRPuzzler
{


    public class BlobView : MonoBehaviour
    {
        public enum BLOBSTATES { IDLE, JUMP, MISS, WIN, HOVER, SING }

        // Listeners--------------------------------------------------------------------------------------------------
        private UnityAction listenForChange;
        private UnityAction listenForWrongAnswer;
        private UnityAction listenForRightAnswer;
        private UnityAction listenForSequenceComplete;
        //------------------------------------------------------------------------------------------------------------
        public BLOBSTATES Blobstate { get { return m_blobState; } set { changeState(value); } }
        public int BlobID;
        public Animator Animator;
        public AudioClip BlobSound;
        //------------------------------------------------------------------------------------------------------------
        private BLOBSTATES m_blobState;
        private Dictionary<BLOBSTATES, string[]> m_blobAnimationTriggers = new Dictionary<BLOBSTATES, string[]>();
        private SequenceController Controller;
        //------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            AddEventTriggers();
            InitTriggerDictionary();
            Controller = gameObject.GetComponentInParent<SequenceController>();
            listenForChange = new UnityAction(OnGameStateChanged);
        }
        void Start()
        {
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", listenForChange);
        }
        //------------------------------------------------------------------------------------------------------------
        public void TriggerByController(SequenceParameters _params)
        {
            if (_params.ReceiverID == BlobID)
            {
                Sing();
            }
        }
        //------------------------------------------------------------------------------------------------------------
        public void Sing()
        {
            if (Blobstate != BLOBSTATES.SING)
            {
                Blobstate = BLOBSTATES.SING;
                SendMessageUpwards("ValidateStep", BlobID);
            }
        }
        //------------------------------------------------------------------------------------------------------------
        private void AddEventTriggers()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry hoverEvent = new EventTrigger.Entry();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry();
            hoverEvent.eventID = EventTriggerType.PointerEnter;
            clickEvent.eventID = EventTriggerType.PointerClick;
            hoverEvent.callback.AddListener((data) => OnHover());
            clickEvent.callback.AddListener((data) => Sing());
            trigger.triggers.Add(hoverEvent);
            trigger.triggers.Add(clickEvent);
        }
        //------------------------------------------------------------------------------------------------------------
        private void InitTriggerDictionary()
        {
            m_blobAnimationTriggers.Add(BLOBSTATES.HOVER, new string[] { "FOCUS", "RAISE_HANDS", "JUMP" });
            m_blobAnimationTriggers.Add(BLOBSTATES.IDLE, new string[] { "IDLE_00", "IDLE_01" });
            m_blobAnimationTriggers.Add(BLOBSTATES.JUMP, new string[] { "JUMP" });
            m_blobAnimationTriggers.Add(BLOBSTATES.MISS, new string[] { "MISS_00", "MISS_01" });
            m_blobAnimationTriggers.Add(BLOBSTATES.WIN, new string[] { "RAISE_HANDS", "JUMP" });
            m_blobAnimationTriggers.Add(BLOBSTATES.SING, new string[] { "SING_00", "SING_01", "SING_02","SING_03" });
        }
        //------------------------------------------------------------------------------------------------------------
        private void TriggerAnimation(BLOBSTATES _state)
        {
            
            string[] _triggers = m_blobAnimationTriggers[_state];
            string _trigger = (_triggers.Length > 0) ? _triggers[UnityEngine.Random.Range(0, _triggers.Length)] : _triggers[0];
            Debug.Log("Trigger:" + _trigger);
            Animator.SetTrigger(_trigger);
        }
        public void AnimationComplete(string _stateAsString)
        {
            if (_stateAsString == BLOBSTATES.SING.ToString())
            {
                SendMessageUpwards("SequenceStepCompleted", BlobID);             
            }
            if (_stateAsString != BLOBSTATES.IDLE.ToString())
            {
                Blobstate = BLOBSTATES.IDLE;
            }
           
        }
        public void OnHover()
        {
            TriggerAnimation(BLOBSTATES.HOVER);
        }
        //------------------------------------------------------------------------------------------------------------
        private void OnGameStateChanged()
        {
            switch (GameFSM.Instance.Gamestate)
            {
                case (GameFSM.GAMESTATES.INTRO):
                   
                    break;
                case (GameFSM.GAMESTATES.GAME):
                    
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    break;
            }
        }
        //------------------------------------------------------------------------------------------------------------
        private void changeState(BLOBSTATES _state)
        {
            m_blobState = _state;
            TriggerAnimation(m_blobState);
            switch (m_blobState)
            {
                case (BLOBSTATES.IDLE):                   
                    break;
                case (BLOBSTATES.SING):
                    break;
            }
        }
        //------------------------------------------------------------------------------------------------------------
        private void OnSingComplete()
        {
            SendMessageUpwards("SequenceStepCompleted", BlobID);
            Blobstate = BLOBSTATES.IDLE;
        }
    }
}