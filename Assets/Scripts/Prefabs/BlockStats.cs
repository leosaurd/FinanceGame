using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStats : MonoBehaviour
{
    //Values to be configured upon block creation
    public float stability;
    public float profit;
    public float cost;
    public string newname;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = newname;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
