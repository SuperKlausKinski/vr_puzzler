using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;
namespace VRPuzzler
{


    public class BlobView : MonoBehaviour
    {
        public enum BLOBSTATES
        {
            IDLE = 0,
            SINGING = 1
        }

        public BLOBSTATES Blobstate { get { return m_blobState; } set { changeState(value); } }
        public int BlobID;
        public SequenceController Controller;
        public AudioClip BlobSound;
        //------------------------------------------------------------------------------------------------------------
        private BLOBSTATES m_blobState;
        private UnityAction listenForChange;
        //------------------------------------------------------------------------------------------------------------
        void Awake()
        {
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
            if (Blobstate != BLOBSTATES.SINGING)
            {
                Blobstate = BLOBSTATES.SINGING;
                SendMessageUpwards("ValidateStep", BlobID);
                transform.DOPunchScale(Vector3.one * 1.05f, 2f).OnComplete(() => OnSingComplete());
               
            }
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
            switch (m_blobState)
            {
                case (BLOBSTATES.IDLE):
                    break;
                case (BLOBSTATES.SINGING):
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