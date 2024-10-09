using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class MatrixData
{
    public List<string> Matrix;
}

public class BubbleMatrixLoader : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private float _spacing = 1.5f;
    [SerializeField] private BubbleColors _bubbleColors;

    private MatrixData _loadedMatrix;
    private string _matrixFilePath;

    public delegate void BubblesCreated(List<GameObject> bubbles);
    public static event BubblesCreated OnBubblesCreated;

    public delegate void TopRowBubblesCreated(List<GameObject> topRowBubbles);  // Новый ивент для передачи верхней строки
    public static event TopRowBubblesCreated OnTopRowBubblesCreated;

    private void Start()
    {
        _matrixFilePath = Path.Combine(Application.dataPath, "Scripts/GameLogic/bubbleMatrix.json");
        LoadMatrixFromJson();
        CreateMatrixFromData();
    }

    private void LoadMatrixFromJson()
    {
        if (File.Exists(_matrixFilePath))
        {
            string json = File.ReadAllText(_matrixFilePath);
            _loadedMatrix = JsonConvert.DeserializeObject<MatrixData>(json);
            if (_loadedMatrix == null || _loadedMatrix.Matrix == null)
            {
                Debug.LogError("Matrix data is empty or invalid.");
            }
            else
            {
                Debug.Log("Matrix loaded from file.");
            }
        }
        else
        {
            Debug.LogError($"Matrix file not found at path: {_matrixFilePath}");
        }
    }

    private void CreateMatrixFromData()
    {
        if (_loadedMatrix?.Matrix == null)
        {
            Debug.LogError("No data found to create matrix.");
            return;
        }

        List<GameObject> createdBubbles = new();
        List<GameObject> topRowBubbles = new();  // Список для верхней строки пузырей

        Vector2 matrixOffset = CalculateMatrixOffset();

        for (int row = 0; row < _loadedMatrix.Matrix.Count; row++)
        {
            string rowData = _loadedMatrix.Matrix[row];

            for (int col = 0; col < rowData.Length; col++)
            {
                char colorId = rowData[col];
                Color bubbleColor = GetColorById(colorId);

                if (bubbleColor != default)
                {
                    Vector2 position = CalculateBubblePosition(row, col) + matrixOffset;
                    GameObject bubbleInstance = Instantiate(_bubblePrefab, position, Quaternion.identity);

                    Bubble bubble = bubbleInstance.GetComponent<Bubble>();
                    bubble.Initialize(bubbleColor, colorId);

                    createdBubbles.Add(bubbleInstance);

                    // Если это верхний ряд, добавляем пузырь в список верхней строки
                    if (row == 0)
                    {
                        topRowBubbles.Add(bubbleInstance);
                    }
                }
                else
                {
                    Debug.LogError($"Unknown colorId {colorId} at row {row}, col {col}");
                }
            }
        }

        // Отправляем данные о созданных пузырях и верхней строке
        OnBubblesCreated?.Invoke(createdBubbles);
        OnTopRowBubblesCreated?.Invoke(topRowBubbles);  // Отправляем верхний ряд
    }

    private Vector2 CalculateBubblePosition(int row, int col)
    {
        return new Vector2(_startPosition.x + col * _spacing, _startPosition.y - row * _spacing);
    }

    private Vector2 CalculateMatrixOffset()
    {
        int maxCols = 0;
        foreach (var rowData in _loadedMatrix.Matrix)
        {
            if (rowData.Length > maxCols)
            {
                maxCols = rowData.Length;
            }
        }

        float totalWidth = (maxCols - 1) * _spacing;

        float xOffset = -totalWidth / 2f;

        return new Vector2(xOffset, 0);
    }

    private Color GetColorById(char colorId)
    {
        for (int i = 0; i < _bubbleColors.ColorId.Length; i++)
        {
            if (_bubbleColors.ColorId[i] == colorId)
            {
                return _bubbleColors.Color[i];
            }
        }

        Debug.LogError($"ColorId {colorId} not found in BubbleColors ScriptableObject");
        return default;
    }
}
