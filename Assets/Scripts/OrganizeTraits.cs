using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrganizeTraits : MonoBehaviour
{
    bool organized = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(!organized)
        {
            string[] traits = Enum.GetNames(typeof(Trait));
            Array.Sort(traits);
            string formatted = "";
            for(int i = 0; i < traits.Length; i++)
            {
                formatted += traits[i] + ", \n";
            }
            print(traits.Length);
            organized = true;
        }
    }

}
