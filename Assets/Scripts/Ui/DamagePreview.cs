using UnityEngine;
using UnityEngine.UI;

public class DamagePreview : MonoBehaviour
{
	private Text[] texts;

	private const int Y_OFFSET = 50;

	private void Awake()
	{
		texts = GetComponentsInChildren<Text>();
	}
	
	private void Update()
	{
		transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + Y_OFFSET, Input.mousePosition.z);
	}

	public void SetDamageText(string newText)
	{
		texts[0].text = newText;
	}

	public void SetNameAndHealthText(string newText)
	{
		texts[1].text = newText;
	}
}