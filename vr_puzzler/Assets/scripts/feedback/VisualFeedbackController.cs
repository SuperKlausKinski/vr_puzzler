using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;


namespace VRPuzzler
{
    public class VisualFeedbackController : MonoBehaviour
    {


        public Animator VisualFeedbackAnimatior;
        public uint VisualFeedbackDuration = 1;

        private UnityAction listenForRightAnswer;
        private UnityAction listenForWrongAnswer;

        void Awake()
        {
            listenForRightAnswer = new UnityAction(RightAnswer);
            listenForWrongAnswer = new UnityAction(WrongAnswer);
        }

        void Start()
        {
            
            EventManager.Instance.StartListening("RIGHT", listenForRightAnswer);
            EventManager.Instance.StartListening("WRONG", listenForWrongAnswer);
        }

        private void RightAnswer()
        {
            VisualFeedbackAnimatior.SetTrigger("RIGHT");
            DOVirtual.DelayedCall(VisualFeedbackDuration, () => VisualFeedbackAnimatior.SetTrigger("IDLE"));
        }
        private void WrongAnswer()
        {
            VisualFeedbackAnimatior.SetTrigger("WRONG");
            DOVirtual.DelayedCall(VisualFeedbackDuration, () => VisualFeedbackAnimatior.SetTrigger("IDLE"));
        }
    }
}