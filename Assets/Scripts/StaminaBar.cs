using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
	public Slider staminaBar;
	public PlayerBehavior playerBehavior;

	public static StaminaBar Instance;

	private readonly WaitForSeconds regenTick = new(0.1f);
	private Coroutine regen;

	private void Awake()
	{
		Instance = this;
	}

	public void UseStamina(float amount)
	{
		if (playerBehavior.stamina > 0)
		{
			TakeStamina(amount);
		}
		else if (playerBehavior.stamina - amount >= 0)
		{
			TakeStamina(amount);
		}
		else
		{
			Debug.Log("Not enough stamina");
		}

		if (regen != null)
		{
			StopCoroutine(regen);
		}

		regen = StartCoroutine(RegenStamina());
	}

	// Start is called before the first frame update
	void Start()
	{
		playerBehavior.stamina = playerBehavior.maxStamina;
		staminaBar.maxValue = playerBehavior.maxStamina;
		staminaBar.value = playerBehavior.maxStamina;
	}

	private IEnumerator RegenStamina()
	{
		yield return new WaitForSeconds(2f);

		while (playerBehavior.stamina < playerBehavior.maxStamina)
		{
			playerBehavior.stamina += playerBehavior.maxStamina / 100;
			staminaBar.value = playerBehavior.stamina;
			yield return regenTick;
		}

		regen = null;
	}

	private void TakeStamina(float amount)
	{
		
		playerBehavior.stamina -= amount;
		staminaBar.value = playerBehavior.stamina;
	}
}