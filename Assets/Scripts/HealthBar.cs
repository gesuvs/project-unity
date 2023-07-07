using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Image healthBar;
	public PlayerBehavior playerBehavior;

	private void HealthBarFilled()
	{
		// healthBar.fillAmount = Mathf.Clamp(player.health / player.maxHealth, 0, 1f);
	}


	public void Damage(float damagePoints)
	{
		if (playerBehavior.health > 0)
		{
			var healthBarRect = healthBar.rectTransform.rect;
			var healthBarWidth = healthBarRect.width;
			var healthBarHeight = healthBarRect.height;
			healthBar.rectTransform.sizeDelta = new Vector2((healthBarWidth - damagePoints), healthBarHeight);
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