using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVToCards
{
    // CSV file location, must be here to work
    private static string _csvFilePath = "/Editor/CardCSV/CardCSV.csv";

    [MenuItem("OOTL/Generate Cards From CSV")]
    public static void GenerateCards()
    {
        // Delete existing cards
        string[] cardFolder = { "Assets/Resources/Cards" };
        foreach (string assetGUID in AssetDatabase.FindAssets("", cardFolder))
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            AssetDatabase.DeleteAsset(path);
        }

        // Get all lines from file
        string[] textLines = File.ReadAllLines(Application.dataPath + _csvFilePath);

        // Variable and check to ignore first line of CSV file that contains the column headers
        bool firstLine = true;

        // Create each card from lines
        foreach(string s in textLines)
        {
            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            // Checks will go here if code fails
            string[] variables = s.Split(',');

            Card newCard = ScriptableObject.CreateInstance<Card>();
            newCard.CardName = variables[0];
            newCard.ManaCost = int.Parse(variables[1]);
            newCard.Attack = int.Parse(variables[2]);
            newCard.Health = int.Parse(variables[3]);
            newCard.CardImage = Resources.Load<Sprite>("Sprites/CardSprites/" + variables[4]);
            
            AssetDatabase.CreateAsset(newCard, $"Assets/Resources/Cards/{newCard.CardName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
