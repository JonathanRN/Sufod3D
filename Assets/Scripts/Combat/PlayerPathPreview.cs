using UnityEngine;

public class PlayerPathPreview : MonoBehaviour
{
	private Tile[] pathPreview;

	public void ShowPathPreview(Tile targetTile)
	{
		if (targetTile != null)
		{
			ClearPathPreview();
			if (targetTile.Type == TileType.Walkable || targetTile.Type == TileType.WalkablePreview)
			{
				Tile[] path = GridManager.Instance.GetPathTo(targetTile).ToArray();
				for (int i = 0; i < path.Length; i++)
				{
					path[i].SetTileType(TileType.WalkablePreview);
					if (i != 0)
					{
						// Don't show the 0
						path[i].SetPreviewNumber(i);
					}
				}
				pathPreview = path;
			}
		}
	}

	private void ClearPathPreview()
	{
		if (pathPreview != null)
		{
			foreach (Tile tile in pathPreview)
			{
				tile.SetTileType(TileType.Walkable);
				tile.DisablePreviewText();
			}
		}
	}

	public void SetToNull()
	{
		pathPreview = null;
	}
}
