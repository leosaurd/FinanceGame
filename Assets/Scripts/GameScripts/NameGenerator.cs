using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    //Very basic list for names to be added to, for randomization
    private List<string> nameList;
    // Start is called before the first frame update
    void Start()
    {
        //Simple variant for adding in names at the moment(I assume we'd want to read from a data file).
        nameList.Add("Insurance");
        nameList.Add("Investment");
        nameList.Add("Stock");
        nameList.Add("Hospital");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
