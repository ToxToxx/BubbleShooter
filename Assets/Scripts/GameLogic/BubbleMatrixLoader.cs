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

    public delegate void BubblesCreated(List<GameObject> bubbles);
    public static event BubblesCreated OnBubblesCreated;

    void Start()
    {
        LoadMatrixFromJson();
        CreateMatrixFromData();
    }

    private void LoadMatrixFromJson()
    {
        string path = Path.Combine(Application.dataPath, "Scripts/GameLogic/bubbleMatrix.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _loadedMatrix = JsonConvert.DeserializeObject<MatrixData>(json);
            Debug.Log("Matrix loaded from file");
        }
        else
        {
            Debug.LogError("Matrix file not found!");
        }
    }

    private void CreateMatrixFromData()
    {
        if (_loadedMatrix == null || _loadedMatrix.Matrix == null)
        {
            Debug.LogError("No data found to create matrix.");
            return;
        }

        List<GameObject> createdBubbles = new();

        for (int row = 0; row < _loadedMatrix.Matrix.Count; row++)
        {
            string rowData = _loadedMatrix.Matrix[row];

            for (int col = 0; col < rowData.Length; col++)
            {
                char colorId = rowData[col];
                Color bubbleColor = GetColorById(colorId);

                if (bubbleColor != default)
                {
                    Vector2 position = new(_startPosition.x + col * _spacing, _startPosition.y - row * _spacing);

                    GameObject bubbleInstance = Instantiate(_bubblePrefab, position, Quaternion.identity);

                    Bubble bubble = bubbleInstance.GetComponent<Bubble>();
                    bubble.Initialize(bubbleColor, colorId);

                    createdBubbles.Add(bubbleInstance); 
                }
                else
                {
                    Debug.LogError($"Unknown colorId {colorId} at row {row}, col {col}");
                }
            }
        }

        OnBubblesCreated?.Invoke(createdBubbles);
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
