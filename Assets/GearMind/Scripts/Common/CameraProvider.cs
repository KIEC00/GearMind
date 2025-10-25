using UnityEngine;

namespace Assets.GearMind.Common
{
    public class CameraProvider : ICameraProvider
    {
        public Camera Current => GetCamera();

        private Camera _camera;

        private Camera GetCamera()
        {
            if (_camera == null)
                _camera = Camera.main;
            if (_camera == null)
                Debug.LogError("No main camera found.");
            return _camera;
        }
    }
}
