using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	[SerializeField]
	private float readyTimeInSeconds = 60f;

	[SerializeField]
	private float turnTimeInSeconds = 45f;

	private bool shouldCountdown;
	private Unit currentTurnUnit;

	public float TimeLeft { get; private set; }
	public float ReadyTimeInSeconds => readyTimeInSeconds;
	public float TurnTimeInSeconds => turnTimeInSeconds;

	public static event System.Action OnReadyTimerExpired;
	public static event System.Action OnTurnTimerExpired;

	private void OnEnable()
	{
		CombatManager.OnCombatInitialized += CombatManager_OnCombatInitialized;
		CombatManager.OnCombatStarted += CombatManager_OnCombatStarted;
		TurnManager.OnTurnStarted += TurnManager_OnTurnStarted;
		TurnManager.OnTurnEnded += TurnManager_OnTurnEnded;
	}

	private void OnDisable()
	{
		CombatManager.OnCombatInitialized -= CombatManager_OnCombatInitialized;
		CombatManager.OnCombatStarted -= CombatManager_OnCombatStarted;
		TurnManager.OnTurnStarted -= TurnManager_OnTurnStarted;
		TurnManager.OnTurnEnded -= TurnManager_OnTurnEnded;
	}

	private void Update()
	{
		if (shouldCountdown)
		{
			if (CombatManager.Instance.IsCombatStarted && currentTurnUnit.CurrentState != UnitState.Idle)
			{
				return;
			}

			if (TimeLeft < 0)
			{
				if (CombatManager.Instance.IsCombatStarted)
				{
					OnTurnTimerExpired?.Invoke();
				}
				else
				{
					OnReadyTimerExpired?.Invoke();
				}
			}
			TimeLeft -= Time.deltaTime;
		}
	}

	private void CombatManager_OnCombatStarted()
	{
		if (currentTurnUnit == null)
		{
			shouldCountdown = false;
		}
	}

	private void TurnManager_OnTurnEnded(Unit unit)
	{
		shouldCountdown = false;
	}

	private void TurnManager_OnTurnStarted(Unit unit)
	{
		TimeLeft = turnTimeInSeconds;
		currentTurnUnit = unit;
		shouldCountdown = true;
	}

	private void CombatManager_OnCombatInitialized()
	{
		TimeLeft = readyTimeInSeconds;
		shouldCountdown = true;
	}
}
