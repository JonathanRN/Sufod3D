using UnityEngine;

namespace Ui
{
	public class AbilityPointsProgressBar : RadialProgressBar
	{
		private void Update()
		{
			progressBar.fillAmount = (float)playerStats.Ap / playerStats.MaxAp;
			amountText.text = playerStats.Ap.ToString();
		}
	}
}