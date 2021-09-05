using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClassicTetris.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public List<ButtonUI> Options;
        public Transform Arrow;
        
        private int _index;

        private void SelectOption()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _index = Mathf.Clamp(_index + 1, 0, Options.Count - 1);
                
                var position = Arrow.position;
                position = new Vector3(position.x, Options[_index].transform.position.y, position.z);
                Arrow.position = position;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _index = Mathf.Clamp(_index - 1, 0, Options.Count - 1);
                
                var position = Arrow.position;
                position = new Vector3(position.x, Options[_index].transform.position.y, position.z);
                Arrow.position = position;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Options[_index].ExecuteOption();
            }
        }

        private void Update()
        {
            SelectOption();
        }
    }
}