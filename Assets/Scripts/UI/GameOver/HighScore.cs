using System;
using TMPro;

namespace ClassicTetris.UI
{
    [Serializable]
    public class HighScore
    {
        public TMP_InputField PlayerName;
        public TextMeshProUGUI TopScore;
        public TextMeshProUGUI Level;
    }
}
