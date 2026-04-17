using System.Collections.Generic;
using UnityEngine;

public class RefillGrid : MonoBehaviour
{
    [SerializeField] private GridData gridData;   
    void Start()
    {
        IconSwitcher.onMatchMade.AddListener(_ => CheckGridForEmptySpots());
    }

    //check the grid for spaces that have been left empty on a match and marks them a rewritable then rebuilds the grid
    private void CheckGridForEmptySpots()
    {
        for (int x = 0; x < gridData.grid.GetLength(1); x++)
        {
            var populated = new Queue<Icon>();
            for (int y = 0; y < gridData.grid.GetLength(0); y++)
            {
                var toCheck = gridData.grid[x, y];
                if (toCheck.GO == null) continue;
                populated.Enqueue(toCheck);
                gridData.grid[x, y] = new Icon();
            }

            var itteration = 0;
            while (populated.Count != 0)
            {
                var icon = populated.Dequeue();
                var offsetx = (gridData.width - 1) / 2f;
                var offsety = gridData.height - 1;

                var worldX = (x - offsetx) * gridData.tileSize;
                var worldY = (itteration - offsety) * gridData.tileSize;
                
                icon.pos = new Vector2(x, itteration);
                gridData.grid[x, itteration] = icon;
                icon.GO.transform.position = new Vector2(worldX, worldY);
                itteration++;
            }
        }
        
        gridData.BuildGrid();
    }
}
