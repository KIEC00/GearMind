using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public class GravityModifierComponent : MonoBehaviour
    {
        [SerializeField, OnValueChanged(nameof(OnEditorValueChanged))]
        private Vector2 _gravity;

        private Vector2 _stagedGravity;

        private void OnEnable() =>
            (_stagedGravity, Physics2D.gravity) = (Physics2D.gravity, _gravity);

        private void OnDisable() => Physics2D.gravity = _stagedGravity;

        private void OnEditorValueChanged()
        {
            if (Application.isPlaying)
                Physics2D.gravity = _gravity;
        }
    }
}
