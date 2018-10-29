using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public static LevelGenerator instance;
    public Transform levelStartPoint;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
    public LevelPieceBasic startPlatformPiece;
    bool end = false;
    // Use this for initialization
    void Start () {
        instance = this;
        ShowPiece((LevelPieceBasic)Instantiate(startPlatformPiece));
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
    }
    public void ShowPiece(LevelPieceBasic piece)
    {
        piece.transform.SetParent(this.transform, false);
        if (pieces.Count < 1)
            piece.transform.position = new Vector2(
            levelStartPoint.position.x - piece.startPoint.localPosition.x,
            levelStartPoint.position.y - piece.startPoint.localPosition.y);
        else
            piece.transform.position = new Vector2(
            pieces[pieces.Count - 1].exitPoint.position.x - pieces[pieces.Count - 1].
            startPoint.localPosition.x,
            pieces[pieces.Count - 1].exitPoint.position.y - pieces[pieces.Count - 1].
            startPoint.localPosition.y);
        pieces.Add(piece);
    }

    public void AddPiece()
    {
        if (end)
            return;
        int randomIndex = Random.Range(0, levelPrefabs.Count-1);
        int endLevel = Random.Range(0, 11);
        LevelPieceBasic piece;
        if (endLevel == 10)
        {
            piece = (LevelPieceBasic)Instantiate(levelPrefabs[levelPrefabs.Count - 1]);
            end = true;
        }
        else
        {
            piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
        }
        ShowPiece(piece);
    }
    public void RemoveOldestPiece()
    {
        if(pieces.Count > 10)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }
}
