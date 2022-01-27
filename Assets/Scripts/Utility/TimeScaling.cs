using System;
using UnityEngine;
namespace LetterBattle.Utility
{
    //down time scaling is prioritized
    public static class TimeScaling
    {

        
        public static QueueValue<float> Status = new QueueValue<float>(1, (QueueValue<float>.DeciderCallback)Math.Min);
         static TimeScaling()
         {
             Status.OnValueChanged += OnValueChanged;
         }
         private static  void OnValueChanged(object sender, QueueValue<float> queue)
         {
             Time.timeScale = queue.Value;
         }
    }
}