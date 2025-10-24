using Assets.Utils.Runtime;
using EditorAttributes;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    [ExecuteAlways]
    [RequireComponent(typeof(Renderer))]
    public class GridVisual : MonoBehaviour
    {
        const float planeScale = 0.1f;

        [SerializeField, Required]
        [OnValueChanged(nameof(HandleGridChangedOrInit))]
        private GridComponent _grid;

        [SerializeField]
        [OnValueChanged(nameof(HandleGridChangedOrInit))]
        private Color _color = Color.cyan;

        [SerializeField, Clamp(0, 1)]
        [OnValueChanged(nameof(HandleGridChangedOrInit))]
        private float _thickness = 0.1f;

        [SerializeField, Required]
        [OnValueChanged(nameof(HandleGridChangedOrInit))]
        private Material _gridMaterial;

        public void Init(GridComponent grid)
        {
            if (_grid != null)
                _grid.OnGridChangedOrInit -= HandleGridChangedOrInit;
            _grid = grid;
            _grid.OnGridChangedOrInit += HandleGridChangedOrInit;
            HandleGridChangedOrInit();
        }

        public void HandleGridChangedOrInit()
        {
            transform.position = _grid.WorldCenter;
            var worldSize = _grid.WorldSize * planeScale;
            var scale = new Vector3(worldSize.x, 1f, worldSize.y);
            transform.localScale = scale;
            transform.rotation = Quaternion.Euler(90f, 0f, 180f);
            var material = new Material(_gridMaterial);
            material.SetColor("_Color", _color);
            material.SetVector("_Size", (Vector2)_grid.Size);
            material.SetFloat("_Thickness", _thickness);
            GetComponent<Renderer>().material = material;
        }

        [Button("Reinitialize")]
        public void Awake() => Init(_grid);

        private void OnDestroy() => _grid.OnGridChangedOrInit -= HandleGridChangedOrInit;
    }
}
