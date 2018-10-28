using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public static LevelGenerator instance;
    public Transform levelStartPoint;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
    // Use this for initialization
    void Start () {
        instance = this;
        AddPiece();
	}
	
	public void AddPiece()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count - 1);
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
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
    public void RemoveOldestPiece()
    {
        if(pieces.Count > 1)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }
}
