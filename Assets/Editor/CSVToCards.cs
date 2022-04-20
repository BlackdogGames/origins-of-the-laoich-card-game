using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

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
            string[] variables = SplitCSV(s);

            //split the string s into substrings with a comma seperator, ignoring commas if they're prefixed with a backslash
            //string[] variables = s.Split(new string[] { "," }, StringSplitOptions.None);

            Card newCard = ScriptableObject.CreateInstance<Card>();
            newCard.ID = int.Parse(variables[0]);
            newCard.CardName = variables[1];
            newCard.ManaCost = int.Parse(variables[2]);
            newCard.Attack = int.Parse(variables[3]);
            newCard.Health = int.Parse(variables[4]);
            newCard.CardImage = Resources.Load<Sprite>("Sprites/CardSprites/" + variables[5]);
            newCard.Description = variables[6];
            
            //if function field isn't empty, give the event an ability
            if (variables[7] != "")
            {
                newCard.Ability.AddPersistentCall(Delegate.CreateDelegate(typeof(CardDelegateAbility),
                    typeof(CardAbilities), variables[7]));
            }
            
            if (variables[8] != "")
                newCard.AbilityTrigger =
                    (Card.CardAbilityTrigger)Enum.Parse(typeof(Card.CardAbilityTrigger), variables[8]);
            else
                newCard.AbilityTrigger = Card.CardAbilityTrigger.NoAbility;

            AssetDatabase.CreateAsset(newCard, $"Assets/Resources/Cards/{newCard.CardName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    //function that splits a string into substrings with a comma seperator
    public static string[] SplitCSV(string line)
    {
        List<string> values = new List<string>();
        bool inQuotes = false;
        string value = "";

        foreach (char c in line)
        {
            if (c == ',' && !inQuotes)
            {
                values.Add(value);
                value = "";
            }
            else
            {
                if (c == '"')
                    inQuotes = !inQuotes;
                else
                    value += c;
            }
        }
        values.Add(value);

        return values.ToArray();
    }
}

