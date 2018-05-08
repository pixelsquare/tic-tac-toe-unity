using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public const int GRID_WIDTH = 3;
	public const int GRID_HEIGHT = 3;

	[SerializeField] private GameObject m_tile = null;
	[SerializeField] private float m_tileOffset = 0.0f;

	[SerializeField] private Color m_tileActiveColor = new Color();
	[SerializeField] private Color m_tileHoverColor = new Color();

	private int m_gridWidth = 0;
	private int m_gridHeight = 0;

	private Tile[,] m_tiles = null;
	private int m_turnCount = 0;

	public void Start()
	{
		initializeGrid();
	}

	private void initializeGrid(int p_width = GRID_WIDTH, int p_height = GRID_HEIGHT)
	{
		m_gridWidth = p_width;
		m_gridHeight = p_height;

		m_tiles = new Tile[m_gridWidth, m_gridHeight];

		if(m_gridWidth % 2 == 1)
		{
			m_gridWidth = m_gridWidth - 1;	
		}

		if(m_gridHeight % 2 == 1)
		{
			m_gridHeight = m_gridHeight - 1;
		}

		m_gridWidth /= 2;
		m_gridHeight /= 2;

		int tileIdx = 0;
		int tilesY = 0;

		for(int y = m_gridHeight; y >= -m_gridHeight; y--)
		{
			int tilesX = 0;
			for(int x = -m_gridWidth; x <= m_gridWidth; x++)
			{
				GameObject tile = (GameObject)Instantiate(m_tile, Vector3.zero, Quaternion.identity);
				tile.name = string.Format("Tile_{0}", tileIdx);

				Tile tileComponent = tile.GetComponent<Tile>();
				if(null == tileComponent)
				{
					tileComponent = tile.AddComponent<Tile>();
				}

				tileComponent.setHoverColor(m_tileHoverColor);
				tileComponent.setActiveColor(m_tileActiveColor);

				tileComponent.setGridPosition(tilesX, tilesY);
				tileComponent.addTileClickedEvent(onTileClickedEvent);

				float tileWidth = tileComponent.getTextureWidth();
				float tileHeight = tileComponent.getTextureHeight();

				Transform tileT = tile.transform;
				tileT.SetParent(transform, false);
				tileT.position = new Vector3(tileWidth * x, tileHeight * y, 0.0f) * m_tileOffset;

				m_tiles[tilesX, tilesY] = tileComponent;

				tileIdx++;
				tilesX++;
			}

			tilesY++;
		}
	}

	private void onTileClickedEvent(Tile p_tile)
	{
		int turnCount = m_turnCount;

		if(turnCount % 2 == 0)
		{
			p_tile.setTextureX();
		}
		else
		{
			p_tile.setTextureO();
		}

		Debug.Log(isGameOver(p_tile));

		m_turnCount++;
	}

	private bool isGameOver(Tile p_tile)
	{
		bool retVal = false;

		int gridXPos = p_tile.getGridX();
		int gridYPos = p_tile.getGridY();

		for(int i = 0; i < m_gridWidth; i++)
		{
			if(m_tiles[i, gridYPos].getTileValue() != p_tile.getTileValue())
			{
				break;
			}

			if(i == m_gridWidth)
			{
				retVal = true;
			}
		}

		return retVal;
	}

	private List<Tile> getNeighborTiles(Tile p_tile)
	{
		List<Tile> retVal = new List<Tile>();

		int gridXPos = p_tile.getGridX();
		int gridYPos = p_tile.getGridY();

		for(int x = gridXPos - 1; x <= gridXPos + 1; x++)
		{
			for(int y = gridYPos - 1; y <= gridYPos + 1; y++)
			{
				if(x < 0 || x >= m_gridWidth + 1 || y < 0 || y > m_gridHeight + 1)
				{
					continue;
				}

				if(x == gridXPos && y == gridYPos)
				{
					continue;
				}

				retVal.Add(m_tiles[x, y]);
			}
		}

		return retVal;
	}
}
