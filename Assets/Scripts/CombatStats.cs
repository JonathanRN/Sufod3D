using UnityEngine;
using WeSoLit;

public class CombatStats : MonoBehaviour
{
	[SerializeField] private int movementPoints = 5;
	[SerializeField] private int actionPoints = 6;
	[SerializeField] private int attackRange = 2;
	
	// todo maybe make an object which contains all stats?
	[SerializeField] private int speed = 2;

	public int MovementPoints
	{
		get { return movementPoints; }
		private set { movementPoints = value; }
	}

	public int ActionPoints
	{
		get { return actionPoints; }
		private set { actionPoints = value; }
	}

	public int AttackRange
	{
		get { return attackRange; }
		private set { attackRange = value; }
	}

	public int Speed
	{
		get { return speed; }
		private set { speed = value; }
	}
	
	public bool IsOutOfMovementPoints
	{
		get { return movementPoints <= 0; }
	}

	public bool IsOutOfActionPoints
	{
		get { return actionPoints <= 0; }
	}

	public void AddMovementPoints(int amount)
	{
		MovementPoints += amount;
	}

	public void RemoveMovementPoints(int amount)
	{
		MovementPoints -= amount;
	}
	
	public void AddActionPoints(int amount)
	{
		ActionPoints += amount;
	}

	public void RemoveActionPoints(int amount)
	{
		ActionPoints -= amount;
	}

	public bool CanUseAbility(Ability ability)
	{
		return ActionPoints >= ability.ApCost;
	}

	public void Reset()
	{
		MovementPoints = 5; //todo make this constant
		ActionPoints = 6;
	}
}