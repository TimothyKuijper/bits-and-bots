using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    public int height;
    public int width;

    public float tileSize;

    [SerializeField] private Sprite[] icons;

    [SerializeField] private GameObject iconPrefab;

    private List<GameObject> gridObjects = new();
    
    public enum IconTypes
    {
        square,
        circle,
        triangle,
        hexagon,
        diamond
    }

    public Icon[,] grid;

    void Start()
    {
        BuildGrid();
    }

    public void BuildGrid()
    {
        if (gridObjects.Count != 0)
        {
            foreach (var objects in gridObjects)
            {
                Destroy(objects);
            }
        }

        gridObjects = new();
        
        if (grid == null)
        {
            grid = new Icon[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var randomType = (IconTypes)Random.Range(0, 5);
                    var Icon = new Icon();
                    grid[x, y] = Icon;
                    grid[x, y].type = randomType;
                    grid[x, y].pos = new Vector2(x, y);
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var offsetx = (width - 1) / 2f;
                var offsety = (height - 1);

                var worldX = (x - offsetx) * tileSize;
                var worldY = (y - offsety) * tileSize;

                var pos = new Vector2(worldX, worldY);
                var icon = Instantiate(iconPrefab, pos, Quaternion.identity);
                grid[x, y].gameObject = icon;

                var index = (int)grid[x,y].type;
                icon.GetComponent<SpriteRenderer>().sprite = icons[index];
                gridObjects.Add(icon);
            }
        }
    }
}
