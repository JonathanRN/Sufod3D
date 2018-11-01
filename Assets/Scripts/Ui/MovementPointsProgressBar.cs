using UnityEngine;

namespace Ui
{
	public class MovementPointsProgressBar : RadialProgressBar
	{
		protected override void OnMovementPointsChange()
		{
			progressBar.fillAmount = (float)playerCombatStats.MovementPoints / playerCombatStats.MaxMovementPoints;
			amountText.text = playerCombatStats.MovementPoints.ToString();
		}
	}
}