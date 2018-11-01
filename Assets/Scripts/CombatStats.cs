using UnityEngine;
using WeSoLit;

public delegate void OnMovementPointsChangedEventHandler();
public delegate void OnAbilityPointsChangedEventHandler();
public class CombatStats : MonoBehaviour
{
	[SerializeField] private int maxMovementPoints = 5;
	[SerializeField] private int maxAbilityPoints = 6;
	
	private int movementPoints;
	private int abilityPoints;
	private int attackRange = 2;
	
	// todo maybe make an object which contains all stats?
	[SerializeField] private int speed = 2;

	public event OnAbilityPointsChangedEventHandler OnAbilityPointsChanged;
	public event OnMovementPointsChangedEventHandler OnMovementPointsChanged;

	public int MaxAbilityPoints => maxAbilityPoints;

	public int MaxMovementPoints => maxMovementPoints;
	
	public int MovementPoints
	{
		get { return movementPoints; }
		private set { movementPoints = value; }
	}

	public int AbilityPoints
	{
		get { return abilityPoints; }
		private set { abilityPoints = value; }
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

	public bool IsOutOfAbilityPoints
	{
		get { return abilityPoints <= 0; }
	}

	public void AddMovementPoints(int amount)
	{
		MovementPoints += amount;
		OnMovementPointsChanged?.Invoke();
	}

	public void RemoveMovementPoints(int amount)
	{
		MovementPoints -= amount;
		OnMovementPointsChanged?.Invoke();
	}
	
	public void AddAbilityPoints(int amount)
	{
		AbilityPoints += amount;
		OnAbilityPointsChanged?.Invoke();
	}

	public void RemoveAbilityPoints(int amount)
	{
		AbilityPoints -= amount;
		OnAbilityPointsChanged?.Invoke();
	}

	public bool CanUseAbility(Ability ability)
	{
		return AbilityPoints >= ability.ApCost;
	}

	private void Awake()
	{
		Reset();
	}

	public void Reset()
	{
		MovementPoints = maxMovementPoints;
		AbilityPoints = maxAbilityPoints;
		OnMovementPointsChanged?.Invoke();
		OnAbilityPointsChanged?.Invoke();
	}
}