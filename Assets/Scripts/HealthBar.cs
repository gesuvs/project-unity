using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public PlayerBehavior playerBehavior;

	private void HealthBarFilled()
	{
		slider.value = Mathf.Clamp(playerBehavior.health / playerBehavior.maxHealth, 0, 1f);
	}


	public void Damage(float damagePoints)
	{
		if (playerBehavior.health > 0)
		{
			slider.value -= damagePoints;
		}
	}

	public void Heal(float healingPoints)
	{
		if (playerBehavior.health < playerBehavior.maxHealth)
		{
			playerBehavior.health += healingPoints;
		}
	}
	
}