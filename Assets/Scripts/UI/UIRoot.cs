using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIRoot", menuName = "Tetris/UI Root")]
public class UIRoot : ScriptableObject
{
    public MainMenuUI MainMenuUI;
    public LevelSelectionUI LevelSelectUI;
    public InGameUI InGameUI;
    public SavingScoreUI SavingScoreUI;
}
