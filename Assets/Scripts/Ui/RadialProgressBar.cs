using System;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public abstract class RadialProgressBar : MonoBehaviour
	{
		[SerializeField] protected Text amountText;
		[SerializeField] protected Image progressBar;
		
		protected CombatStats playerCombatStats;

		private void Awake()
		{
			playerCombatStats = GameObject.FindWithTag("Player").GetComponentInChildren<CombatStats>();
		}

		private void OnEnable()
		{
			playerCombatStats.OnAbilityPointsChanged += OnAbilityPointsChange;
			playerCombatStats.OnMovementPointsChanged += OnMovementPointsChange;
		}

		private void OnDisable()
		{
			playerCombatStats.OnAbilityPointsChanged -= OnAbilityPointsChange;
			playerCombatStats.OnMovementPointsChanged -= OnMovementPointsChange;
		}

		protected virtual void OnAbilityPointsChange()
		{
			//Not all implement this
		}

		protected virtual void OnMovementPointsChange()
		{
			//Not all implement this			
		}
	}
}