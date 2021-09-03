using System;
using System.Collections;
using System.Collections.Generic;
using devRHS.ClassicTetris.TetrominoCreator;
using TMPro;
using UnityEngine;

public class PieceCounter : MonoBehaviour
{
    public TetrominoDatabase TetrominoDatabase;
    public List<TextMeshProUGUI> PieceCounterList;
    public CellBlock CellBlockObj;
    private Camera _camera;
    public void InstantiatePieceCounters()
    {
        for (int i = 0; i < PieceCounterList.Count; i++)
        {
            var piece = TetrominoDatabase.Tetrominoes[i];
            var parent = new GameObject {name = piece.TetrominoName};
            for (int j = 0; j < piece.CellPositions.Length; j++)
            {
                var block = Instantiate(CellBlockObj, piece.CellPositions[j], Quaternion.identity, parent.transform);
                block.SpriteRenderer.sprite = piece.CellSprite;
            }
            parent.transform.localScale *= 3;
            parent.transform.position += PieceCounterList[i].transform.TransformPoint(PieceCounterList[i].transform.position) + new Vector3(-14,0,0);
            parent.transform.SetParent(transform);
        }
    }

    public void UpdatePieceCounters(Dictionary<string,object> message)
    {
        var player = (Player) message["player"];
        for (int i = 0; i < PieceCounterList.Count; i++)
        {
            PieceCounterList[i].text = player.Spawner.TetrominoCounter[i].ToString("000");
        }
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        InstantiatePieceCounters();
    }

    private void OnEnable()
    {
        EventManager.StartListening("UpdatingPieceCounters", UpdatePieceCounters);
    }

    private void OnDisable()
    {
        EventManager.StopListening("UpdatingPieceCounters", UpdatePieceCounters);
    }
}
