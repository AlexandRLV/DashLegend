using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSunsetStudio
{
    public class SimpleSwipeDetector : MonoBehaviour
    {
        private Vector3 _clickPos;

        [Tooltip("Threshold of detection in pixels.")]
        public int DeadZone;

        public static event Action OnSwipeUp;
        public static event Action OnSwipeDown;
        public static event Action OnSwipeLeft;
        public static event Action OnSwipeRight;

        void Update()
        {
            //PC
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))// || Input.GetTouch(0).phase == TouchPhase.Began)
                _clickPos = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))// || Input.GetTouch(0).phase == TouchPhase.Ended)
                CheckSwipeDirection(Input.mousePosition);
#endif

            //MOBILE
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Began)
                    _clickPos = touch.position;

            if (touch.phase == TouchPhase.Ended)
                CheckSwipeDirection(touch.position);
        }
#endif
        }

        private void CheckSwipeDirection(Vector3 releasePos)
        {
            Vector3 dir = releasePos - _clickPos;

            // UP / DOWN
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                // UP
                if (dir.y > DeadZone)
                {
                    OnSwipeUp?.Invoke();
                }
                // DOWN
                if (dir.y < -DeadZone)
                {
                    OnSwipeDown?.Invoke();
                }
            }
            // LEFT / RIGHT
            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                // RIGHT
                if (dir.x > DeadZone)
                {
                    OnSwipeRight?.Invoke();
                }
                // LEFT
                if (dir.x < -DeadZone)
                {
                    OnSwipeLeft?.Invoke();
                }
            }
        } 
    }
}
