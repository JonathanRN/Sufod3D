using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CountdownSlider : MonoBehaviour
{
	private Slider slider;
	private TimeManager timeManager;

	private void Start()
	{
		timeManager = TimeManager.Instance;
		slider = GetComponent<Slider>();
	}

	private void Update()
	{
		if (CombatManager.Instance.IsCombatStarted)
		{
			slider.value = timeManager.TimeLeft / timeManager.TurnTimeInSeconds;
		}
		else
		{
			slider.value = timeManager.TimeLeft / timeManager.ReadyTimeInSeconds;
		}
	}
}
