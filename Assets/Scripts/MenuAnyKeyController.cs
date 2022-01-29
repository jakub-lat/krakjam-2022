using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuAnyKeyController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent t = null;

    public void OnAny()
    {
        t.Invoke();
    }
}
