using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSunsetStudio
{
    public class CubeControls : MonoBehaviour
    {
        private Vector3 _target;

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime);
        }

        private void OnEnable()
        {
            SimpleSwipeDetector.OnSwipeUp += MoveUp;
            SimpleSwipeDetector.OnSwipeDown += MoveDown;
            SimpleSwipeDetector.OnSwipeLeft += MoveLeft;
            SimpleSwipeDetector.OnSwipeRight += MoveRight;
        }

        private void OnDisable()
        {
            SimpleSwipeDetector.OnSwipeUp -= MoveUp;
            SimpleSwipeDetector.OnSwipeDown -= MoveDown;
            SimpleSwipeDetector.OnSwipeLeft -= MoveLeft;
            SimpleSwipeDetector.OnSwipeRight -= MoveRight;
        }

        void MoveUp()
        {
            _target.z++;
            Debug.Log("Swipe Up Action");
        }

        void MoveDown()
        {
            _target.z--;
            Debug.Log("Swipe Down Action");
        }

        void MoveLeft()
        {
            _target.x--;
            Debug.Log("Swipe Left Action");
        }

        void MoveRight()
        {
            _target.x++;
            Debug.Log("Swipe Right Action");
        }
    } 
}
