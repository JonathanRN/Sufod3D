using UnityEngine;

public class DamagePreviewCreator : MonoBehaviour
{
	[SerializeField] private GameObject damagePreviewPrefab;

	private const int Y_OFFSET = 50;

	public GameObject InstantiateDamagePreview()
	{
		if (GameObject.FindGameObjectWithTag("DamagePreview"))
			return null;

		var canvasParent = GameObject.FindWithTag("Canvas").gameObject;

		return Instantiate(damagePreviewPrefab,
						   new Vector3(Input.mousePosition.x, Input.mousePosition.y + Y_OFFSET, Input.mousePosition.z),
						   Quaternion.identity,
						   canvasParent.transform);
	}

	public void DestroyIfExists()
	{
		var found = GameObject.FindGameObjectWithTag("DamagePreview");

		if (found)
			Destroy(found.gameObject);
	}
}