using System;
using UnityEngine;

namespace UI
{
    public class UIBillboarding : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            transform.forward = _camera.transform.forward;
        }
    }
}