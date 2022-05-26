using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    public static string GenerateName(BlockType blocktype)
    {
        //Get value from nameValues, return one from the array.
        //Generate name here.
        string[] names = nameValues[blocktype];
        return names[Random.Range(0, names.Length)];
    }
    
    private static readonly Dictionary<BlockType, string[]> nameValues = new Dictionary<BlockType, string[]>()
    {
        {BlockType.Insurance, new string[] {"Hello", "World"} },
        {BlockType.Risky, new string[] {"Hello", "World"} },
        {BlockType.Neutral, new string[] {"Hello", "World"} },
        {BlockType.Valuable, new string[] {"Hello", "World"} }
    };
}
