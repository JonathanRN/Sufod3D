using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public abstract class RadialProgressBar : MonoBehaviour
	{
		[SerializeField] protected Text amountText;
		[SerializeField] protected Image progressBar;

		[SerializeField]
		protected UnitStats playerStats;
	}
}