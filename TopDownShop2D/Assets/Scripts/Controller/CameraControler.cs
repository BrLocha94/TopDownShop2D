namespace Project.Core
{
    using System;
    using UnityEngine;
    using Project.Utils;

    public class CameraControler : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraObject;

        [Header("Pop animation config")]
        [SerializeField]
        private AnimationCurve turnOnCurve;
        [SerializeField]
        private float turnOnTime;
        [SerializeField]
        private Vector3 onPosition;

        [Space]

        [SerializeField]
        private AnimationCurve turnOffCurve;
        [SerializeField]
        private float turnOffTime;
        [SerializeField]
        private Vector3 offPosition;

        Coroutine currentPopRoutine = null;

        public void ExecuteCameraMoveOn(Action callback)
        {
            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            cameraObject.transform.localPosition = offPosition;

            currentPopRoutine = this.MoveRoutine(cameraObject, cameraObject.transform.localPosition, onPosition,
                                                        turnOnCurve, turnOnTime, callback);
        }

        public void ExecuteCameraMoveOff(Action callback)
        {
            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            cameraObject.transform.localPosition = onPosition;

            currentPopRoutine = this.MoveRoutine(cameraObject, cameraObject.transform.localPosition, offPosition,
                                                        turnOnCurve, turnOnTime, callback);
        }
    }
}