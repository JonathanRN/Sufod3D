using System.Collections;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
	[SerializeField]
	private GameObject[] systemsPrefabs;

	public bool IsCombatStarted { get; private set; }

	public static event System.Action OnCombatInitialized;
	public static event System.Action OnCombatStarted;

	protected override void Awake()
	{
		base.Awake();
		InstantiateAllSystems();
	}

	private void Start()
	{
		OnCombatInitialized?.Invoke();
	}

	private void Update()
	{
		if (!IsCombatStarted)
		{
			if (TurnManager.Instance.AreAllPlayersReady())
			{
				StartCombat();
			}
		}
	}

	public void StartCombat()
	{
		IsCombatStarted = true;
		// Making sure to reset everything before starting combat
		GridManager.Instance.ResetTiles();
		OnCombatStarted?.Invoke();
	}

	private void InstantiateAllSystems()
	{
		foreach (GameObject system in systemsPrefabs)
		{
			Instantiate(system, transform);
		}
	}
}
