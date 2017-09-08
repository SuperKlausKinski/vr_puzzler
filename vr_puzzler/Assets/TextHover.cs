using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace VRPuzzler
{


    public class TextHover : MonoBehaviour
    {
        void Start()
        {
            Vector3 _currentPos = gameObject.GetComponent<RectTransform>().localPosition;
            gameObject.GetComponent<RectTransform>().DOLocalMoveZ(_currentPos.y-0.25f, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}