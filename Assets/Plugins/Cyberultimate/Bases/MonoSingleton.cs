#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Cyberultimate.Unity
{
    public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
    {
        public static T Current { get; private set; }
        protected virtual void Awake()
        {
            if (Current != null)
            {
                Destroy(this);
            }
            else
            {
                Current = (T)this;
            }
        }
    }
}
