using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour {

    public Texture2D cursor;
	public Vector2 Offset = Vector2.zero;
	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursor, Offset, CursorMode.Auto);
	}

}
