using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
	public Slider staminaBar;
	private float maxStamina = 100f;
	private float stamina;

	public static StaminaBar _instance;


	private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
	private Coroutine regen;

	private void Awake()
	{
		_instance = this;
	}


	public void UseStamina(float amount)
	{
		if (stamina - amount >= 0)
		{
			stamina -= amount;
			staminaBar.value = stamina;

			if (regen != null)
			{
				StopCoroutine(regen);
			}

			regen = StartCoroutine(RegenStamina());
		}
		else
		{
			Debug.Log("Not enough stamina");
		}
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
}