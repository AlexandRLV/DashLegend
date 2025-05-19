using UnityEngine;

namespace GameCore.Level
{
    [ExecuteAlways]
    public class LevelCurveStrengthController : MonoBehaviour
    {
        private static readonly int _curveStrengthId = Shader.PropertyToID("_CurveStrength");
        private static readonly int _distanceOffsetId = Shader.PropertyToID("_DistanceOffset");

        [SerializeField] private bool _enableCurve;
        [SerializeField] [Range(-0.01f, 0.01f)] private float _curveStrength;
        [SerializeField] private float _distanceOffset;

        private void Update()
        {
            float strength = _enableCurve ? _curveStrength : 0f;
            Shader.SetGlobalFloat(_curveStrengthId, strength);

            _distanceOffset = Mathf.Max(0f, _distanceOffset);
            Shader.SetGlobalFloat(_distanceOffsetId, _distanceOffset);
        }
    }
}