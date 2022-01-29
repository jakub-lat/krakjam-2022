using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public int index=0;
    public bool Hit()
    {
        Debug.Log("phit");
        return BossPipe.Current.PipeHit(index);
    }
}
