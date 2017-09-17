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
        public GameObject BulbFlares;
        public Material BulbFlareMaterial;
        public Color32 RightAnswerColor;
        public Color32 WrongAnswerColor;
        public AnimationCurve FlashCurve;

        private UnityAction listenForRightAnswer;
        private UnityAction listenForWrongAnswer;
        private Color32 m_defaultColor = Color.white;
        private Color32 m_flareDefaultColor = Color.grey;
       

        void Awake()
        {
            listenForRightAnswer = new UnityAction(RightAnswer);
            listenForWrongAnswer = new UnityAction(WrongAnswer);

        }

        void Start()
        {
            BulbFlareMaterial.SetColor("_TintColor", m_flareDefaultColor);
            EventManager.Instance.StartListening("RIGHT", listenForRightAnswer);
            EventManager.Instance.StartListening("WRONG", listenForWrongAnswer);
        }

        private void RightAnswer()
        {

            BulbFlareMaterial.DOColor(RightAnswerColor,"_TintColor", VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbFlareMaterial.SetColor("_TintColor", m_flareDefaultColor));
            BulbMaterial.DOColor(RightAnswerColor, VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbMaterial.DOColor(m_defaultColor, 0.5f));
           
        }
        private void WrongAnswer()
        {

            BulbFlareMaterial.DOColor(WrongAnswerColor,"_TintColor", VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbFlareMaterial.SetColor("_TintColor", m_flareDefaultColor));
            BulbMaterial.DOColor(WrongAnswerColor, VisualFeedbackDuration).SetEase(FlashCurve).OnComplete(() => BulbMaterial.DOColor(m_defaultColor, 0.5f));
          
        }
        private void DefaultBulb()
        {
            BulbFlareMaterial.DOColor(m_flareDefaultColor, 0.5f);
            BulbMaterial.DOColor(m_defaultColor, 0.5f);
        }
    }
}