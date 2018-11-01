using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int maxHealthPoints;
	
	private int healthPoints;

	public int HealthPoints
	{
		get { return healthPoints; }
		private set
		{
			if (value < 0)
				healthPoints = 0;
			else if (value > maxHealthPoints)
				healthPoints = maxHealthPoints;
			else
				healthPoints = value;
		}
	}

	public bool IsDead
	{
		get { return HealthPoints <= 0; }
	}

	private void Awake()
	{
		ResetHealthPoints();
	}

	public void Hit(int amount)
	{
		HealthPoints -= amount;
	}

	public void Heal(int amount)
	{
		HealthPoints += amount;
	}

	public void ResetHealthPoints()
	{
		HealthPoints = maxHealthPoints;
	}
}