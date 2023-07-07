public class ChaseState : StateMachine
{

	public AttackState attackState;
	public bool isInAttackRange;
	
	public override StateMachine RunCurrentState()
	{
		if (isInAttackRange)
		{
			return attackState;
		}

		return this;
	}
}