using System.Collections;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int width;
    public int height;

    public int[,] currentGeneration;
    public int[,] nextGeneration;
    [Range(0.01f, 1f)] public float timeIntervalBetweenRenders;
    [Range(1, 100)] public int density;
    public ComputeShader shader;
    public Color deadCellColor;
    public Color aliveCellColor;
    private WaitForSeconds waitForSeconds;
    private RenderTexture render;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(timeIntervalBetweenRenders);
        currentGeneration = new int[width, height];
        nextGeneration = new int[width, height];

        render = new RenderTexture(width, height, 1);
        render.enableRandomWrite = true;
        render.Create();

        GenerateBoard();
        Dispacth();
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(render, destination);
    }
    private void Start()
    {
        StartCoroutine(Simulate());
    }
    private void Dispacth()
    {
        ComputeBuffer currentGenerationBuffer = new ComputeBuffer(currentGeneration.Length, sizeof(int));
        ComputeBuffer nextGenerationBuffer = new ComputeBuffer(nextGeneration.Length, sizeof(int));

        currentGenerationBuffer.SetData(currentGeneration);
        nextGenerationBuffer.SetData(nextGeneration);
        shader.SetTexture(0, "Result", render);
        shader.SetBuffer(0, "currentGeneration", currentGenerationBuffer);
        shader.SetBuffer(0, "nextGeneration", nextGenerationBuffer);
        shader.SetFloats("deadCellColor", deadCellColor.r, deadCellColor.g, deadCellColor.b, deadCellColor.a);
        shader.SetFloats("aliveCellColor", aliveCellColor.r, aliveCellColor.g, aliveCellColor.b, aliveCellColor.a);
        shader.SetInt("width", width);
        shader.SetInt("height", height);

        shader.Dispatch(0, (width * height) / 64, 1, 1);
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
    private IEnumerator Simulate()
    {
        while (true)
        {
            yield return waitForSeconds;
            shader.Dispatch(0, (width * height) / 64, 1, 1);
        }
    }
}