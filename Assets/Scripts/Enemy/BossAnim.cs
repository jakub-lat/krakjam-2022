using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    public void PunchEnd()
    {
        Boss.Current.EndPunching();
    }

    public void ElectroEnd()
    {
        Boss.Current.EndPipe();
    }
}
