using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;
using System;
using UnityEngine.Events;
using DG.Tweening;

namespace VRPuzzler
{
    public class SequenceParameters
    {
        public Action Callback = null;
        public int ReceiverID = 0;
        public SequenceController Controller = null;
    }

    public class SequenceController : MonoBehaviour
    {

        public enum SEQUENCESTATES
        {
            IDLE = 0,
            PRESENTING = 1,
            LISTENING = 2,
            FINISH = 3
        }

        public SEQUENCESTATES SequenceState { get { return m_sequenceState; } set { changeState(value); } }
        public SequenceTemplate[] Sequences;
        public int CurrentSequenceID { get { return m_currentSequenceID; } }
        //------------------------------------------------------------------------------------------------------------
        private SEQUENCESTATES m_sequenceState;
        private SEQUENCESTATES m_prevSequencestate;
        private uint m_remainingSequenceSteps;
        private uint m_totalSequenceCount;
        private int m_currentStep;
        private int m_sequenceLength;
        private bool m_sequenceStepInProgess;
        private bool m_currentStepValid;
        private int[] m_currentSequence;
        private int m_currentSequenceID;
        private UnityAction ListenForStateChange;
        //------------------------------------------------------------------------------------------------------------
        void Start()
        {

            InitSequenceController();
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", ListenForStateChange);
        }
        void Awake()
        {
            ListenForStateChange = new UnityAction(OnGameStateChanged);          
        }

        private void OnGameStateChanged()
        {
            switch (GameFSM.Instance.Gamestate)
            {
                case (GameFSM.GAMESTATES.GAME):
                    SequenceState = SEQUENCESTATES.IDLE;
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    SequenceState = SEQUENCESTATES.FINISH;
                    break;
            }

        }
        //------------------------------------------------------------------------------------------------------------
        private int[] GetSequence(int _id)
        {
            return Sequences[_id].Sequence;
        }
        //------------------------------------------------------------------------------------------------------------
        private void changeState(SEQUENCESTATES _state)
        {
            
            m_prevSequencestate = m_sequenceState;
            m_sequenceState = _state;

            switch (m_sequenceState)
            {
                case (SEQUENCESTATES.IDLE):
                    //InputController.Instance.DisableInput();
                    InputController.Instance.BlobInput(false);
                    DOVirtual.DelayedCall(2f,()=> SetUpSequence(m_currentSequenceID));                  
                    break;
                case (SEQUENCESTATES.LISTENING):
                    EventManager.Instance.InvokeEvent("LISTEN");
                    ListenForSender();
                    break;
                case (SEQUENCESTATES.PRESENTING):
                    EventManager.Instance.InvokeEvent("PRESENTING");
                    TriggerReceiver(m_currentSequence[m_currentStep]);
                    break;
                case (SEQUENCESTATES.FINISH):
                    break;
            }
        }
        //------------------------------------------------------------------------------------------------------------
        public void InitSequenceController()
        {
            m_totalSequenceCount = (uint)Sequences.Length;
            m_currentSequenceID = 0;
        }
        //------------------------------------------------------------------------------------------------------------
        public void SetUpSequence(int _sequenceID)
        {
           
            m_currentStep = 0;
            m_currentSequenceID = _sequenceID;
            m_currentSequence = GetSequence(m_currentSequenceID);
            m_sequenceLength = m_currentSequence.Length;


            switch (m_prevSequencestate)
            {
                case (SEQUENCESTATES.PRESENTING):
                    SequenceState = SEQUENCESTATES.LISTENING;
                    break;
                case (SEQUENCESTATES.LISTENING):
                    SequenceState = SEQUENCESTATES.PRESENTING;
                    break;
                case (SEQUENCESTATES.IDLE):
                    if (m_prevSequencestate == SEQUENCESTATES.IDLE)
                    {
                        SequenceState = SEQUENCESTATES.PRESENTING;
                    }
                    break;
            }

        }
        //------------------------------------------------------------------------------------------------------------
        public void ValidateStep(int _id)
        {
            //InputController.Instance.DisableInput();
            InputController.Instance.BlobInput(false);
            m_currentStepValid = (_id == m_currentSequence[m_currentStep]) ? true : false;

            if (SequenceState == SEQUENCESTATES.LISTENING)
            {
                EventManager.Instance.InvokeEvent((_id == m_currentSequence[m_currentStep]) ? "RIGHT" : "WRONG");
            }        
        }
        //------------------------------------------------------------------------------------------------------------
        private void ListenForSender()
        {
            m_sequenceStepInProgess = true;
            //InputController.Instance.EnableInput();
            InputController.Instance.BlobInput(true);
            StartCoroutine(WaitingForSequenceStepToFinish());
        }
        //------------------------------------------------------------------------------------------------------------
        private void TriggerReceiver(int _receiverID)
        {
            SequenceParameters _sequenceParams = new SequenceParameters();
            _sequenceParams.ReceiverID = _receiverID;
            _sequenceParams.Controller = this;
            m_sequenceStepInProgess = true;
            StartCoroutine(WaitingForSequenceStepToFinish());
            BroadcastMessage("TriggerByController", _sequenceParams);
        }
        //------------------------------------------------------------------------------------------------------------
        private void SequenceStepCompleted(int _id)
        {         
            m_sequenceStepInProgess = false;
        }
        //------------------------------------------------------------------------------------------------------------
        private void MoveToNextStep()
        {
            // listen mode
            if (SequenceState == SEQUENCESTATES.LISTENING)
            {
                if (!m_currentStepValid)
                {
                    SequenceState = SEQUENCESTATES.IDLE;
                    return;
                }
                else
                {
                    if (m_currentStep < m_sequenceLength - 1)
                    {                      
                        m_currentStep++;
                        ListenForSender();
                    }
                    else
                    {
                        EventManager.Instance.InvokeEvent("INPUTSEQUENCE_COMPLETED");
                        if (m_currentSequenceID < m_totalSequenceCount)
                        {
                            m_currentSequenceID++;
                            SequenceState = SEQUENCESTATES.IDLE;
                        }
                        else
                        {
                            SequenceState = SEQUENCESTATES.FINISH;
                        }
                    }
                }
            }
            // presentation mode
            else if (SequenceState == SEQUENCESTATES.PRESENTING)
            {
                if (m_currentStep < m_sequenceLength - 1)
                {
                    m_currentStep++;
                    TriggerReceiver(m_currentSequence[m_currentStep]);
                }
                else
                {                
                    EventManager.Instance.InvokeEvent("SEQUENCE_COMPLETED");
                    SequenceState = SEQUENCESTATES.IDLE;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------
        IEnumerator WaitingForSequenceStepToFinish()
        {
            while (m_sequenceStepInProgess)
            {
                yield return null;
            }
            MoveToNextStep();
        }
        //------------------------------------------------------------------------------------------------------------

    }
}