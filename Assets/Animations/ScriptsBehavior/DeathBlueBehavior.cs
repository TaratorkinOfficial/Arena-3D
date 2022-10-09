using UnityEngine;

namespace Animations.ScriptsBehavior
{
    public class DeathBlueBehavior : StateMachineBehaviour
    {
        private Transform _bot;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bot = animator.GetComponent<Transform>();
            Destroy(_bot.gameObject);
        }

    }
}
