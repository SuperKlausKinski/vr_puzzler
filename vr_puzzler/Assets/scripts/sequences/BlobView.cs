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
        public GameObject BlobObject;
        public GameObject BlobFlare;
        //------------------------------------------------------------------------------------------------------------
        private BLOBSTATES m_blobState;
        private Dictionary<BLOBSTATES, string[]> m_blobAnimationTriggers = new Dictionary<BLOBSTATES, string[]>();
        private SequenceController Controller;
        private Color32 m_blobColor;
        private GvrAudioSource m_gvrAudioSource;
        //------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            m_gvrAudioSource = GetComponent<GvrAudioSource>();
            m_gvrAudioSource.clip = BlobSound;
            AddEventTriggers();
            InitTriggerDictionary();
            Controller = gameObject.GetComponentInParent<SequenceController>();
            listenForChange = new UnityAction(OnGameStateChanged);
            BlobFlare.SetActive(false);
            m_blobColor = BlobObject.GetComponent<Renderer>().material.color;
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
            Debug.Log("I sing now!!!");
                Blobstate = BLOBSTATES.SING;
                SendMessageUpwards("ValidateStep", BlobID);
            m_gvrAudioSource.Play();
      
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
            string _trigger = (_triggers.Length > 0) ? _triggers[UnityEngine.Random.Range(0, _triggers.Length-1)] : _triggers[0];
            Debug.LogFormat("Trigger {0} on {1}, choosen from {2} triggers",_state,gameObject.name,_triggers.Length);
       
            Animator.SetTrigger(_trigger);
            
          //  Debug.Break();
        }
        public void AnimationComplete(string _finishedAnimation)
        {
            //Debug.Break();
            Debug.Log(_finishedAnimation + " is complete for" + gameObject.name + "in state " + Blobstate);
            if (_finishedAnimation == BLOBSTATES.SING.ToString())
            {
                Debug.Log("Sing completed");
                SendMessageUpwards("SequenceStepCompleted", BlobID);
                Blobstate = BLOBSTATES.IDLE;
                return;
            }
            if (Blobstate == BLOBSTATES.SING) return; // we're entering from another animation state here, which could interrupt the singing
            if (_finishedAnimation != BLOBSTATES.IDLE.ToString())  // we do not want to be idling like crazy, don't we?
            {
                Debug.Log("I am not blobstate sing? I'm " + Blobstate);
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
            Debug.Log("changing animation state to" + _state);
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
        public void CheckForSingState()  // hack to prevent the idle overwriting the sing trigger if the blob sings to times in a row
        {
            if(Blobstate == BLOBSTATES.SING)
            {
                TriggerAnimation(m_blobState);
            }
        }
        //------------------------------------------------------------------------------------------------------------
        private void ShowblobFlare()
        {
            BlobFlare.SetActive(true);
            Material _material = BlobFlare.GetComponent<MeshRenderer>().material;     
            _material.SetColor("_TintColor",Color.black);
           
            _material.DOColor(m_blobColor,"_TintColor", 0.75f).SetEase(Ease.InOutBack).OnComplete(() => BlobFlare.SetActive(false));
        }
        //------------------------------------------------------------------------------------------------------------
        private void OnSingComplete(string _stateAsString)
        {
            Debug.Log(_stateAsString + " is complete for" + gameObject.name + "in state " + Blobstate);
            if (_stateAsString == BLOBSTATES.SING.ToString())
            {
                Debug.Log("Sing completed");
                SendMessageUpwards("SequenceStepCompleted", BlobID);
                Blobstate = BLOBSTATES.IDLE;
                return;
            }
        }
    }
}