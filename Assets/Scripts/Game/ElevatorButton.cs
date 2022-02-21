using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ElevatorButton : MonoBehaviour
    {
        [SerializeField] private Text floorText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Image outline;
        
        [SerializeField] private Color normalColor, selectedColor;

        private void Start()
        {
            outline.color = normalColor;
        }

        public void Init(int floor, string description)
        {
            floorText.text = floor.ToString();
            descriptionText.text = description;
        }

        public void Select()
        {
            outline.color = selectedColor;
        }

        public void Unselect()
        {
            outline.color = normalColor;
        }
    }
}
