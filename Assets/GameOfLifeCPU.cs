using System.Collections;
using UnityEngine;

public class GameOfLifeCPU : MonoBehaviour
{
    public int width;
    public int height;

    [Range(0.01f, 1f)] public float timeIntervalBetweenRenders;
    [Range(1, 100)] public int density;
    private WaitForSeconds waitForSeconds;
    public Color deadCellColor;
    public Color aliveCellColor;
    private int[,] currentGeneration;
    private int[,] nextGeneration;
    private Texture2D texture;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(timeIntervalBetweenRenders);
        currentGeneration = new int[width, height];
        nextGeneration = new int[width, height];

        texture = new Texture2D(width, height);
    }

    private void Start()
    {
        GenerateBoard();
        StartCoroutine(Simulate());
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(texture, destination);
    }
    private void GenerateBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (Random.Range(0, 100) < density)
                {
                    currentGeneration[i, j] = 1;
                }
                else
                {
                    currentGeneration[i, j] = 0;
                }
            }
        }
    }
    private void TransferNextGenerations()
    {
        int[,] temp = currentGeneration;
        currentGeneration = nextGeneration;
        nextGeneration = temp;
    }

    private int CalculateLiveNeighbours(int x, int y)
    {
        int liveNeighbours = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x + i < 0 || x + i >= width)
                    continue;
                if (y + j < 0 || y + j >= height)
                    continue;
                if (x + i == x && y + j == y)
                    continue;

                liveNeighbours += currentGeneration[x + i, y + j];
            }
        }

        return liveNeighbours;
    }
    public void SetNextGeneration()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int liveNeighbours = CalculateLiveNeighbours(x, y);

                if (currentGeneration[x, y] == 1 && liveNeighbours < 2)
                {
                    nextGeneration[x, y] = 0;
                    texture.SetPixel(x, y, deadCellColor);
                }

                else if (currentGeneration[x, y] == 1 && liveNeighbours > 3)
                {
                    nextGeneration[x, y] = 0;
                    texture.SetPixel(x, y, deadCellColor);
                }

                else if (currentGeneration[x, y] == 0 && liveNeighbours == 3)
                {
                    nextGeneration[x, y] = 1;
                    texture.SetPixel(x, y, aliveCellColor);
                }

                else
                {
                    nextGeneration[x, y] = currentGeneration[x, y];
                    texture.SetPixel(x, y, nextGeneration[x, y] >= 1 ? aliveCellColor : deadCellColor);
                }
            }
        }

        texture.Apply();
        TransferNextGenerations();
    }
    private IEnumerator Simulate()
    {
        while (true)
        {
            yield return waitForSeconds;
            SetNextGeneration();
        }
    }
}


