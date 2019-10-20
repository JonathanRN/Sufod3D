using UnityEngine;

namespace Ui
{
	public class MovementPointsProgressBar : RadialProgressBar
	{
		private void Update()
		{
			progressBar.fillAmount = (float)playerStats.Mp / playerStats.MaxMp;
			amountText.text = playerStats.Mp.ToString();
		}
	}
}