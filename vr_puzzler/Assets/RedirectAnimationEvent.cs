using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPuzzler
{
    public class RedirectAnimationEvent : MonoBehaviour
    {
        public TutorialBlob TutorialBlob;
        public void SwitchCardContent(int _alpha)
        {
            TutorialBlob.CardContentVisibility(_alpha);
        }
        public void PlaySwooshAudio()
        {
            TutorialBlob.PlaySwoshAudio();
        }
        public void AnimationEnd(string _clip)
        {
            TutorialBlob.AnimationEnd(_clip);
        }
    }
}
