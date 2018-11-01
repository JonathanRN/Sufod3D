using UnityEngine;

namespace Ui
{
	public class AbilityPointsProgressBar : RadialProgressBar
	{
		protected override void OnAbilityPointsChange()
		{
			progressBar.fillAmount = (float)playerCombatStats.AbilityPoints / playerCombatStats.MaxAbilityPoints;
			amountText.text = playerCombatStats.AbilityPoints.ToString();
		}
	}
}