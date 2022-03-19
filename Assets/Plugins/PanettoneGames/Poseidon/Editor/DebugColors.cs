using UnityEngine;

namespace PanettoneGames.Poseidon.Utils
{
    [CreateAssetMenu(fileName = "New Debug Colors", menuName = "Poseidon/Debug Colors")]
    public class DebugColors : ScriptableObject
    {
        public Color[] FormatColors;
    }
}