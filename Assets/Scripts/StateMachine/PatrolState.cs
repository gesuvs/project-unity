using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
	private float timer;

	private List<Transform> wayPoints = new();

	private NavMeshAgent agent;
	private Transform player;
	private const float ChaseRange = 8;

	private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");
	private static readonly int IsChasing = Animator.StringToHash("isChasing");

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;

		agent = animator.GetComponent<NavMeshAgent>();
		agent.speed = 1.5f;
		timer = 0;
		var gameObject = GameObject.FindGameObjectWithTag("WayPoints");

		foreach (Transform t in gameObject.transform)
		{
			wayPoints.Add(t);
		}

		agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
		}

		timer += Time.deltaTime;

		if (timer > 10)
		{
			animator.SetBool(IsPatrolling, false);
		}

		var distance = Vector3.Distance(player.position, animator.transform.position);
		if (distance < ChaseRange)
		{
			animator.SetBool(IsChasing, true);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}