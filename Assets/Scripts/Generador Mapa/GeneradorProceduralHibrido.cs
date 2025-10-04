using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GeneradorProceduralHibrido : MonoBehaviour
{
    [SerializeField] GameObject pasilloTile;
    [SerializeField] GameObject aula;

    [SerializeField] int cantidadAulas = 5;
    [SerializeField] int fabricaWidth = 200;
    [SerializeField] int fabricaHeight = 200;
    [SerializeField] int tileSize;
    private List<Vector2Int> posicionesAulas = new List<Vector2Int>();

    private int[,] grid;

    void Start()
    {
        grid = new int[fabricaWidth, fabricaHeight];

        GenerarAulas();
        ConectarAulas();

        ImprimirGrid();
    }

    void GenerarAulas()
    {
        int aulaSize = 2; // tamaño de cada aula en tiles
        int gridWidth = fabricaWidth / aulaSize;
        int gridHeight = fabricaHeight / aulaSize;

        List<Vector2Int> posiblesPosiciones = new List<Vector2Int>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                posiblesPosiciones.Add(new Vector2Int(x, y));
            }
        }

        // Mezclar posiciones
        for (int i = 0; i < posiblesPosiciones.Count; i++)
        {
            Vector2Int tmp = posiblesPosiciones[i];
            int r = UnityEngine.Random.Range(i, posiblesPosiciones.Count);
            posiblesPosiciones[i] = posiblesPosiciones[r];
            posiblesPosiciones[r] = tmp;
        }

        // Colocar aulas
        for (int i = 0; i < cantidadAulas && i < posiblesPosiciones.Count; i++)
        {
            Vector2Int gridPos = posiblesPosiciones[i];

            // Marcar las celdas del aula en el grid
            for (int dx = 0; dx < aulaSize; dx++)
            {
                for (int dy = 0; dy < aulaSize; dy++)
                {
                    int x = gridPos.x * aulaSize + dx;
                    int y = gridPos.y * aulaSize + dy;

                    if (x < fabricaWidth && y < fabricaHeight)
                    {
                        grid[x, y] = 1; // 1 = aula
                    }
                }
            }

            // Centro del aula para instanciar el objeto en el mundo
            Vector3 worldPos = new Vector3(
                (gridPos.x * aulaSize + aulaSize / 2) * tileSize,
                0,
                (gridPos.y * aulaSize + aulaSize / 2) * tileSize
            );

            posicionesAulas.Add(new Vector2Int(gridPos.x * aulaSize + aulaSize / 2, gridPos.y * aulaSize + aulaSize / 2));
            Instantiate(aula, worldPos, Quaternion.identity);
        }
    }



    void ConectarAulas()
    {
        for (int i = 0; i < posicionesAulas.Count - 1; i++)
        {
            Vector2Int start = posicionesAulas[i];
            Vector2Int end = posicionesAulas[i + 1];

            List<Vector2Int> camino = BFSConCosto(start, end);
            foreach (Vector2Int pos in camino)
            {
                if (grid[pos.x, pos.y] == 0)
                {
                    grid[pos.x, pos.y] = 2; // pasillo
                    Vector3 worldPos = new Vector3(pos.x * tileSize, 0, pos.y * tileSize);
                    Instantiate(pasilloTile, worldPos, Quaternion.identity);
                }
            }
        }
    }

    List<Vector2Int> BFSConCosto(Vector2Int start, Vector2Int end)
    {
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();

        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;

        Vector2Int[] dirs = new Vector2Int[]
        {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1)
        };

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == end) break;

            foreach (var dir in dirs)
            {
                Vector2Int next = current + dir;
                if (next.x < 0 || next.y < 0 || next.x >= fabricaWidth || next.y >= fabricaHeight)
                    continue;

                // ⚠️ Solo se puede pasar si es vacío o pasillo, nunca sobre aulas
                if (grid[next.x, next.y] == 1) continue;

                float newCost = costSoFar[current] + ((grid[next.x, next.y] == 0) ? 10f : 1f);
                float priority = newCost + UnityEngine.Random.Range(0, 5f);

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        // Reconstruir camino
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int temp = end;

        // Si no hay camino válido, path se queda vacío
        if (!cameFrom.ContainsKey(end)) return path;

        while (temp != start)
        {
            path.Add(temp);
            temp = cameFrom[temp];
        }
        path.Reverse();
        return path;
    }

    // Simple priority queue
    public class PriorityQueue<T>
    {
        private List<(T item, float priority)> elements = new List<(T, float)>();
        public int Count => elements.Count;
        public void Enqueue(T item, float priority) => elements.Add((item, priority));
        public T Dequeue()
        {
            int bestIndex = 0;
            for (int i = 1; i < elements.Count; i++)
                if (elements[i].priority < elements[bestIndex].priority)
                    bestIndex = i;
            T bestItem = elements[bestIndex].item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    void ImprimirGrid()
    {
        for (int j = 0; j < fabricaHeight; j++) // recorrer filas primero
        {
            string linea = "";
            for (int i = 0; i < fabricaWidth; i++)
            {
                linea += grid[i, j].ToString();
            }
            Debug.Log(linea);
        }
    }

}
