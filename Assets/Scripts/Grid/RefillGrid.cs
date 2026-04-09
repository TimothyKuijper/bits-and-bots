using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillGrid : MonoBehaviour
{
    [SerializeField] private GridData gridData;   
    void Start()
    {
        IconSwitcher.onMatchMade.AddListener(_ => CheckGridForEmptySpots());
    }

    void Update()
    {
        
    }

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
            }


            var itteration = 0;
            while (populated.Count != 0)
            {
                var icon = populated.Dequeue();
                var offsetx = (gridData.width - 1) / 2f;
                var offsety = gridData.height - 1;

                var worldX = (x - offsetx) * gridData.tileSize;
                var worldY = (itteration - offsety) * gridData.tileSize;
                
                icon.pos = new Vector2(worldX, worldY);
                gridData.grid[x, itteration] = icon;
                icon.GO.transform.position = icon.pos;
                itteration++;
            }
        }
    }
}
