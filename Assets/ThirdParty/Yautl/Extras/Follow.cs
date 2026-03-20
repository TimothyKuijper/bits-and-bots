using UnityEngine;

namespace Yakanashe.Yautl
{
    public class Follow : MonoBehaviour
    {
        private Transform _target;
        private float _smoothTime;
        private Vector3 _offset;

        private Vector3 _velocity = Vector3.zero;

        public void Initialize(Transform target, float? smoothTime, Vector3? offset)
        {
            _target = target;
            _offset = offset ?? Vector3.zero;
            _smoothTime = smoothTime ?? 0.3f;
        }

        private void LateUpdate()
        {
            if (!_target) return;

            var desiredPosition = _target.position + _offset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref _velocity,
                _smoothTime
            );
        }
    }
}