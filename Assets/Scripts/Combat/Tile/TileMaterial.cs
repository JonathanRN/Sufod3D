using UnityEngine;

public class TileMaterial : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer meshRenderer;

	[SerializeField]
	private TileMat[] tileMats;

	public void SetTile(TileType type)
	{
		for (int i = 0; i < tileMats.Length; i++)
		{
			if (type == tileMats[i].TileType)
			{
				meshRenderer.material = tileMats[i].Material;
				return;
			}
		}
	}
}

[System.Serializable]
public struct TileMat
{
	public Material Material;
	public TileType TileType;
}
