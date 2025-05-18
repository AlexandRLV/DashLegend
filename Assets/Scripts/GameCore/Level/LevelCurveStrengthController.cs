using UnityEngine;

namespace GameCore.Level
{
    [ExecuteAlways]
    public class LevelCurveStrengthController : MonoBehaviour
    {
        private static readonly int _curveStrengthId = Shader.PropertyToID("_CurveStrength");

        [SerializeField] private bool _enableCurve;
        [SerializeField] [Range(-0.03f, 0.03f)] private float _curveStrength;

        private void Update()
        {
            float strength = _enableCurve ? _curveStrength : 0f;
            Shader.SetGlobalFloat(_curveStrengthId, strength);
        }
    }
}