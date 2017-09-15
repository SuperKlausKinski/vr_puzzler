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



        public uint VisualFeedbackDuration = 1;
        public Material BulbMaterial;
        public Color32 RightAnswerColor;
        public Color32 WrongAnswerColor;
        public AnimationCurve FlashCurve;

        private UnityAction listenForRightAnswer;
        private UnityAction listenForWrongAnswer;
        private Color32 m_defaultColor = Color.white;
       

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
         
            BulbMaterial.DOColor(RightAnswerColor, VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbMaterial.DOColor(m_defaultColor, 0.5f));
           
        }
        private void WrongAnswer()
        {
            BulbMaterial.DOColor(WrongAnswerColor, VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbMaterial.DOColor(m_defaultColor, 0.5f));
          
        }
    }
}