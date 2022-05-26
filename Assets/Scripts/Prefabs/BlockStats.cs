using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStats : MonoBehaviour
{
    //Values to be configured upon block creation
    public float stability;
    public float profit;
    public float cost;
    blockType blkType;
    // Start is called before the first frame update
    void Start()
    {
        //Assign Block Type
        blkType = blockType.GroupA;
        //Generate a name of the object based upon the group. 
        gameObject.name = NameGenerator.GenerateName(blkType);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public enum blockType
    {
        GroupA,
        GroupB,
        GroupC,
        GroupD
    };
}
