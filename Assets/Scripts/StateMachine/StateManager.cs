using UnityEngine;

public class StateManager : MonoBehaviour
{
	public StateMachine currentState;

	// Update is called once per frame
	void Update()
	{
		RunStateMachine();
	}

	private void RunStateMachine()
	{
		var nextState = currentState.RunCurrentState();

		if (nextState != null)
		{
			SwitchNextState(nextState);
		}
	}

	private void SwitchNextState(StateMachine nextState)
	{
		currentState = nextState;
	}
    
}