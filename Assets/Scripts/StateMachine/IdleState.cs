public class IdleState : StateMachine
{

	public ChaseState chaseState;
	public bool isCanSeePlayer;
	
	public override StateMachine RunCurrentState()
	{
		if (isCanSeePlayer)
		{
			return chaseState;
		}
		return this;
	}
}