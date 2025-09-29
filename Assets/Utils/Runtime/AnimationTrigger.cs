using UnityEngine;

namespace Assets.Utils.Runtime
{
    public class AnimationTrigger : StateMachineBehaviour
    {
        [SerializeField]
        private string _name;

        public override void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            foreach (var listener in GetListeners<IAnimationEnterCallbackReceiver>(animator))
                listener.OnAnimationEnter(new(_name, animator, stateInfo, layerIndex));
        }

        public override void OnStateExit(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            foreach (var listener in GetListeners<IAnimationExitCallbackReceiver>(animator))
                listener.OnAnimationExit(new(_name, animator, stateInfo, layerIndex));
        }

        private T[] GetListeners<T>(Animator animator) => animator.gameObject.GetComponents<T>();
    }

    public interface IAnimationEnterCallbackReceiver
    {
        void OnAnimationEnter(AnimatorCallbackInfo info);
    }

    public interface IAnimationExitCallbackReceiver
    {
        void OnAnimationExit(AnimatorCallbackInfo info);
    }

    public class AnimatorCallbackInfo
    {
        public string Name { get; }
        public GameObject GameObject => Animator.gameObject;
        public Animator Animator { get; }
        public AnimatorStateInfo StateInfo { get; }
        public int LayerIndex { get; }

        public AnimatorCallbackInfo(
            string name,
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            Name = name;
            Animator = animator;
            StateInfo = stateInfo;
            LayerIndex = layerIndex;
        }
    }
}
