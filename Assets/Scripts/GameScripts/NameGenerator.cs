using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public static string GenerateName(BlockStats.blockType blocktype)
    {
        //Get value from nameValues, return one from the array.
        //Generate name here.
        return "name";
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    readonly Dictionary<int, string[]> nameValues = new Dictionary<int, string[]>()
    {
        {1, new string[] {"Hello", "World"} },
        {2, new string[] {"Hello", "World"} },
        {3, new string[] {"Hello", "World"} },
        {4, new string[] {"Hello", "World"} }
    };
}
