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
        //------------------------------------------------------------------------------------------------------------
        private BLOBSTATES m_blobState;
        //------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            Controller = gameObject.GetComponentInParent<SequenceController>();
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