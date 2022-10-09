using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Animations.ScriptsBehavior
{
    public class AttackBlueBehavior : StateMachineBehaviour
    {
        private Transform _player;
        private Transform _bot;
        private NavMeshAgent _agent;
        private float _timer;
        [SerializeField] private GameObject projectile;
        private NavMeshHit _hit;
        private float walkRadius = 20;

        [Inject]
        private void Construct([Inject(Id = "Player")] Transform player)
        {
            _player = player;
        }
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bot = animator.GetComponent<Transform>();
            _agent = animator.GetComponent<NavMeshAgent>();
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer += Time.deltaTime;
            if (_timer > 3)
            {
                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                NavMesh.SamplePosition(randomDirection, out _hit, walkRadius, 1);
                _agent.SetDestination(_hit.position);
                if (true)
                {
                    _bot.LookAt(new Vector3(_player.position.x, _player.position.y + 1f, _player.position.z));
                    Instantiate(projectile, new Vector3(_bot.position.x, _bot.position.y, _bot.position.z + 1), 
                        Quaternion.identity);
                }
                _timer = 0;
            }
        }
    }
}
