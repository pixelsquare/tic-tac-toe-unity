using UnityEngine;
using System.Collections;

public delegate void OnTileClicked(Tile p_tile);

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour 
{
	[SerializeField] private SpriteRenderer m_sprTileValue = null;
	[SerializeField] private Sprite m_spriteX = null;
	[SerializeField] private Sprite m_spriteO = null;

	private GameObject m_gameobject = null;
	private Transform m_transform = null;

	private SpriteRenderer m_spriteRenderer = null;

	private Texture2D m_texture = null;

	private float m_textureWidth = 0.0f;
	private float m_textureHeight = 0.0f;

	private Vector3 m_textureSize = new Vector3();
	private Vector3 m_textureExtents = new Vector3();

	private Color m_originalColor = new Color();
	private Color m_hoverColor = new Color();
	private Color m_activeColor = new Color();

	private bool m_isMouseOver = false;
	private bool m_hasValue = false;

	private int m_gridX = 0;
	private int m_gridY = 0;

	private int m_tileValue = -1;

	private event OnTileClicked m_tileClickedEvent;

	public void Awake()
	{
		m_gameobject = gameObject;
		m_transform = transform;

		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_originalColor = m_spriteRenderer.color;

		m_texture = m_spriteRenderer.sprite.texture;

		if(null != m_texture)
		{
			m_textureWidth = m_texture.width;
			m_textureHeight = m_texture.height;

			m_textureSize = new Vector3(m_textureWidth, m_textureHeight, 0.0f);
			m_textureExtents = m_textureSize / 2.0f;
		}
	}

	public void OnMouseOver()
	{
		if(m_hasValue)
		{
			return;
		}

		m_spriteRenderer.color = m_hoverColor;
		m_isMouseOver = true;
	}

	public void OnMouseExit()
	{
		if(m_hasValue)
		{
			return;
		}

		m_spriteRenderer.color = m_originalColor;
		m_isMouseOver = false;
	}

	public void OnMouseDown()
	{
		if(m_hasValue)
		{
			return;
		}
		m_spriteRenderer.color = m_activeColor;
	}

	public void OnMouseUp()
	{
		if(!m_isMouseOver || m_hasValue)
		{
			return;
		}

		if(null != m_tileClickedEvent)
		{
			m_tileClickedEvent(this);
		}
	}

	public float getTextureWidth()
	{
		return m_textureWidth;
	}

	public float getTextureHeight()
	{
		return m_textureHeight;
	}

	public Texture2D getTexture()
	{
		return m_texture;
	}

	public Vector3 getSize()
	{
		return m_textureSize;
	}

	public Vector3 getCenter()
	{
		return m_transform.position;
	}

	public Vector3 getExtents()
	{
		return m_textureExtents;
	}

	public Vector3 getMin()
	{
		return m_transform.position - m_textureExtents;
	}

	public Vector3 getMax()
	{
		return m_transform.position + m_textureExtents;
	}

	public int getTileValue()
	{
		return m_tileValue;
	}

	public void setHoverColor(Color p_hoverColor)
	{
		m_hoverColor = p_hoverColor;
	}

	public void setActiveColor(Color p_activeColor)
	{
		m_activeColor = p_activeColor;
	}

	public void setGridPosition(int p_gridX, int p_gridY)
	{
		m_gridX = p_gridX;
		m_gridY = p_gridY;
	}

	public int getGridX()
	{
		return m_gridX;
	}

	public int getGridY()
	{
		return m_gridY;
	}

	public void addTileClickedEvent(OnTileClicked p_tileClicked)
	{
		m_tileClickedEvent += p_tileClicked;
	}

	public void removeTileClickedEvent(OnTileClicked p_tileClicked)
	{
		m_tileClickedEvent -= p_tileClicked;
	}

	public void setTextureX()
	{
		m_spriteRenderer.color = m_originalColor;
		m_sprTileValue.sprite = m_spriteX;
		m_hasValue = true;
		m_tileValue = 1;
	}

	public void setTextureO()
	{
		m_spriteRenderer.color = m_originalColor;
		m_sprTileValue.sprite = m_spriteO;
		m_hasValue = true;
		m_tileValue = 0;
	}
}
