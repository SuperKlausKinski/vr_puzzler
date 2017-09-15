using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;

namespace VRPuzzler
{
    public class AudioController : MonoBehaviour
    {

        public GvrAudioSource AmbientAudio;
        public AudioClip AmbientMurmur;
        public AudioClip AmbientMurmurSilentClip;
        public AudioClip AmbientApplauseClip;
        public AudioClip AmbientLooseClip;
        public GvrAudioSource Orchestra;
        public AudioClip OrchestraStartClip;
        public AudioClip OrchestraWinClip;
        public AudioClip OrchestraLooseClip;
        public GvrAudioSource Counter;
        public AudioClip CounterBingClip;
        public AudioClip CounterLooseClip;
        public GvrAudioSource ApplauseAudio;

        private UnityAction m_listenForGameStateChange;
        private UnityAction m_listenForRightAnswer;
        private UnityAction m_listenForWrongAnswer;
        private UnityAction m_listenForPresentationMode;
        private UnityAction m_listenForSequenceComplete;
        //-------------------------------------------------------------------------------------------------
        void Awake()
        {
            m_listenForGameStateChange = new UnityAction(OnGameStateChanged);
            m_listenForRightAnswer = new UnityAction(()=>PlayAnswerFX(true));
            m_listenForWrongAnswer = new UnityAction(() => PlayAnswerFX(false));
            m_listenForPresentationMode = new UnityAction(() => StartAudio(AmbientAudio,AmbientMurmurSilentClip,true,true));
            m_listenForSequenceComplete = new UnityAction(() => StartAudio(ApplauseAudio, AmbientApplauseClip));
        }
        //-------------------------------------------------------------------------------------------------
        void Start()
        {
            EventManager.Instance.StartListening("GAMESTATE_CHANGED", m_listenForGameStateChange);
            EventManager.Instance.StartListening("RIGHT", m_listenForRightAnswer);
            EventManager.Instance.StartListening("WRONG", m_listenForWrongAnswer);
            EventManager.Instance.StartListening("PRESENTING", m_listenForPresentationMode);
            EventManager.Instance.StartListening("INPUTSEQUENCE_COMPLETED", m_listenForSequenceComplete);
        }
        //-------------------------------------------------------------------------------------------------
        private void PlayAnswerFX(bool _valid)
        {
           
            if (_valid)
            {

                StartAudio(Counter, CounterBingClip);
            }
            else
            {
                DOVirtual.DelayedCall(0.5f, () => StartAudio(ApplauseAudio, AmbientLooseClip));
                
            }
        }
        private void WinSequence()
        {
            StartAudio(ApplauseAudio, AmbientApplauseClip);
            StartAudio(Counter, CounterBingClip);
        }
        //-------------------------------------------------------------------------------------------------
        private void OnGameStateChanged()
        {
            switch (GameFSM.Instance.Gamestate)
            {
                case (GameFSM.GAMESTATES.INTRO):
                    StartAudio(AmbientAudio, AmbientMurmur,true,true);
                    StartAudio(Orchestra, OrchestraStartClip,false);
                    break;
                case (GameFSM.GAMESTATES.GAME):
                    StopAudio(AmbientAudio,true, () => StartAudio(AmbientAudio, AmbientMurmurSilentClip, true,true));
                    break;
                case (GameFSM.GAMESTATES.FINISH):
                    StartAudio(ApplauseAudio, AmbientApplauseClip);
                    break;
                default:
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------
        private void StartAudio(GvrAudioSource _source, AudioClip _clip, bool _fadeIn = false, bool _loop = false)
        {
            _source.clip = _clip;
            _source.loop = _loop;
            _source.Play();           
            if (_fadeIn)
            {
                StartCoroutine(FadeIn(_source));
            }
            else
            {
                _source.volume = 1;
            }
        }
        //-------------------------------------------------------------------------------------------------
        private void StopAudio(GvrAudioSource _source,bool _fadeOut = false, Action _crossFadeCallback = null)
        {
            
            if (_fadeOut)
            {
                StartCoroutine(FadeOut(_source,_crossFadeCallback));
            }
            else
            {
                _source.Stop();
            }
        }
        //-------------------------------------------------------------------------------------------------
        IEnumerator FadeIn(GvrAudioSource _source)
        {
            _source.volume = 0;
            while (_source.volume < 1)
            {
                _source.volume += 0.5f*Time.deltaTime;
                yield return null;
            }
            _source.volume = 1;
            yield return null;
        }
        //-------------------------------------------------------------------------------------------------
        IEnumerator FadeOut(GvrAudioSource _source, Action _crossFadeCallback = null)
        {

            while (_source.volume > 0)
            {
                _source.volume -= 0.5f * Time.deltaTime;
                yield return null;
            }
            _source.volume = 0;
            _crossFadeCallback();
            yield return null;
        }
    }
}