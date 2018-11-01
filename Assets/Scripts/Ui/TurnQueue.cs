using UnityEngine;

public class TurnQueue : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("IsPlaying", true);
	}

	public void OnSliderButtonClick()
	{
		animator.SetBool("IsPlaying", !animator.GetBool("IsPlaying"));
	}
}