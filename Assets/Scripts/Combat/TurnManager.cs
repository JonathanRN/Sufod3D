using System.Collections.Generic;
using System.Linq;

public class TurnManager : Singleton<TurnManager>
{
	private List<Unit> units;

	public Queue<Unit> TurnUnits { get; private set; }
	public static event System.Action OnQueueUpdated;

	public static event System.Action<Unit> OnTurnStarted;
	public static event System.Action<Unit> OnTurnEnded;

	protected override void Awake()
	{
		base.Awake();
		TurnUnits = new Queue<Unit>();
		units = new List<Unit>();
	}

	private void OnEnable()
	{
		CombatManager.OnCombatStarted += CombatManager_OnCombatStarted;
		TimeManager.OnReadyTimerExpired += TurnTimeManager_OnReadyTimerExpired;
		TimeManager.OnTurnTimerExpired += TurnTimeManager_OnTurnTimerExpired;
	}

	private void OnDisable()
	{
		CombatManager.OnCombatStarted -= CombatManager_OnCombatStarted;
		TimeManager.OnReadyTimerExpired -= TurnTimeManager_OnReadyTimerExpired;
		TimeManager.OnTurnTimerExpired -= TurnTimeManager_OnTurnTimerExpired;
	}

	private void TurnTimeManager_OnReadyTimerExpired()
	{
		// Make all the unready players ready
		foreach (Unit unit in units)
		{
			if (unit.CurrentState != UnitState.Ready)
			{
				unit.CurrentState = UnitState.Ready;
			}
		}
	}

	private void TurnTimeManager_OnTurnTimerExpired()
	{
		EndTurn();
	}

	private void CombatManager_OnCombatStarted()
	{
		StartTurn();
	}

	private void StartTurn()
	{
		if (TurnUnits.Count > 0)
		{
			Unit unit = TurnUnits.Peek();
			unit.BeginTurn();
			OnTurnStarted?.Invoke(unit);
		}
	}

	public void EndTurn()
	{
		Unit unit = TurnUnits.Dequeue();
		unit.EndTurn();
		OnTurnEnded?.Invoke(unit);

		TurnUnits.Enqueue(unit);

		OnQueueUpdated?.Invoke();
		StartTurn();
	}
	
	public void AddUnit(Unit unit)
	{
		units.Add(unit);
		SortList();
	}

	public void RemoveUnit(Unit unit)
	{
		if (units.Contains(unit))
		{
			units.Remove(unit);
			SortList();
		}
	}

	private void SortList()
	{
		//todo change this
		// Sort the units depending on the speed
		units.Sort((x, y) => y.UnitStats.Spd.CompareTo(x.UnitStats.Spd));
		TurnUnits = new Queue<Unit>(units);
		OnQueueUpdated?.Invoke();
	}

	public bool AreAllPlayersReady()
	{
		return units.Where(u => u.CurrentState == UnitState.Ready).Count() == units.Count;
	}
}