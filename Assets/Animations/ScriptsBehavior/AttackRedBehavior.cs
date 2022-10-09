using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Animations.ScriptsBehavior
{
    public class AttackRedBehavior : StateMachineBehaviour
    {
        private Transform _player;
        private Transform _bot;
        private NavMeshAgent _agent;
        private float _timer;
        private bool _pick;
        private bool _isPaused;
        private Vector3 _posit;
        private MainCharacterController _characterController;
        [SerializeField] private CharacterData _redCharacterData;
        private bool _isHit;

        [Inject]
        private void Construct([Inject(Id = "Player")] Transform player,
            [Inject(Id = "characterController")] MainCharacterController characterController)
        {
            _player = player;
            _characterController = characterController;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bot = animator.GetComponent<Transform>();
            _agent = animator.GetComponent<NavMeshAgent>();
            _posit = new Vector3(0, 10, 0);
        
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!UiController.IsPaused)
            {
                if(_agent.baseOffset<10 && !_pick)
                {
                    _agent.baseOffset = Mathf.MoveTowards(_agent.baseOffset, 11, .04f);
                }
                if (_agent.baseOffset >= 10) _pick = true;
                if(_pick)
                {
                    _timer += Time.deltaTime;
                }
                if (_timer > 2)
                {
                    _agent.SetDestination(_player.position);
                    _posit = Vector3.MoveTowards(_posit, new Vector3(_player.position.x, _player.position.y + .5f, _player.position.z), .01f);
                    _agent.baseOffset = _posit.y;
                    _bot.LookAt(new Vector3(_player.position.x, _player.position.y + 1f, _player.position.z));
                    if (_agent.remainingDistance> 0 && _agent.remainingDistance <= _agent.stoppingDistance)
                    {
                        animator.SetBool("isDead", true);
                        if (!_isHit)
                        {
                            _characterController.TakeDamage(_redCharacterData.damage);
                            _isHit = true;
                        }
                        
                    }
                } 
            }
        }
    }
}
