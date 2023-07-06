using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
	public Slider staminaBar;
	public float maxStamina = 100f;
	public float stamina;

	public static StaminaBar _instance;

	private WaitForSeconds regenTick = new(0.1f);
	private Coroutine regen;

	private void Awake()
	{
		_instance = this;
	}

	public void UseStamina(float amount)
	{

		if (stamina > 0)
		{
			TakeStamina(amount);
		} else if (stamina - amount >= 0)
		{
			TakeStamina(amount);
		}else
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
		stamina = maxStamina;
		staminaBar.maxValue = maxStamina;
		staminaBar.value = maxStamina;
	}

	private IEnumerator RegenStamina()
	{
		yield return new WaitForSeconds(2f);

		while (stamina < maxStamina)
		{
			stamina += maxStamina / 100;
			staminaBar.value = stamina;
			yield return regenTick;
		}

		regen = null;
	}

	private void TakeStamina(float amount)
	{
		stamina -= amount;
		staminaBar.value = stamina;
	}
}