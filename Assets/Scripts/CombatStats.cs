using UnityEngine;

public class CombatStats : MonoBehaviour
{
	[SerializeField] private int movementPoints = 5;
	[SerializeField] private int attackRange = 2;

	public int MovementPoints
	{
		get { return movementPoints; }
		private set { movementPoints = value; }
	}
	 
	public int AttackRange
	{
		get { return attackRange; }
		private set { attackRange = value; }
	}
	
	public bool IsOutOfMovementPoints
	{
		get { return movementPoints <= 0; }
	}

	public void AddMovementPoints(int amount)
	{
		MovementPoints += amount;
	}

	public void RemoveMovementPoints(int amount)
	{
		MovementPoints -= amount;
	}

	public void Reset()
	{
		MovementPoints = 5; //todo make this constant
	}
}