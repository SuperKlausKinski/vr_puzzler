using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPuzzler.Templates;
namespace VRPuzzler
{
    public class GameFSM : Singleton<GameFSM>
    {
        public enum GAMESTATES
        {
            INIT   = 0,
            LOADING = 1,
            START = 2,
            INTRO = 3,
            GAME = 4,
            FINISH = 5
        }

 

        public GAMESTATES Gamestate
        {
            get { return m_gameState; }
            set { changeState(value); }
        }

        public GAMESTATES PrevState { get { return m_prevState; } }

        private GAMESTATES m_gameState;
        private GAMESTATES m_prevState;

        //---------------------------------------------------------------------
        private bool changeState(GAMESTATES _state)
        {

            // validate if state change is possible, if not exit and return false
            if (m_gameState == _state) return false;
            Debug.Log("Entering " + _state);
            // set the new state, start coroutine to keep the state and return true
            m_gameState = _state;
            StartCoroutine(KeepState(m_gameState));
            EventManager.Instance.InvokeEvent("GAMESTATE_CHANGED");
            return true;
        }


        IEnumerator KeepState(GAMESTATES _state)
        {
            // send event that state has entered
            EventManager.Instance.InvokeEvent(_state.ToString()+"_ENTERED");
            while (m_gameState == _state)
            {
                yield return null;
            }
            // send event that state has exited
            m_prevState = _state;
            EventManager.Instance.InvokeEvent(m_prevState.ToString()+"_EXITED");
        }

    }
}
