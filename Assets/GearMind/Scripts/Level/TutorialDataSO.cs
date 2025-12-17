using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "GearMind/Tutorial Data", order = 1)]
public class TutorialDataSO : ScriptableObject
{
    [System.Serializable]
    public struct Slide
    {
        public Sprite Image;
        public string Text;
    }

    public List<Slide> Slides = new();
}