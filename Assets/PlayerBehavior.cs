using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
   
	public float health;
	public float maxHealth;

	public HealthBar healthBar;

	private void TakeDamage(float damagePoints)
	{
		if (!(health > 0)) return;
		health -=damagePoints;
		healthBar.Damage(damagePoints);
	}

	// Update is called once per frame
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StaminaBar._instance.UseStamina(15); 
		} 
	}
}
