using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct InspectorCharInfo {
    public char character;
    public int referenceNumber;
};

public class FontHelper : ButtonScript
{

    public Font font;

    public List<InspectorCharInfo> inspectorCharValues;

    const int TILE_SIZE = 16;
    const int NUM_ROWS = 22;
    const int NUM_COLS = 48;

    const float UV_W = 1f/NUM_COLS;
    const float UV_H = 1f/NUM_ROWS;
    
    public override void BuildObject() {
        Debug.Log("Built character Info!");

        List<CharacterInfo> cInfo = new List<CharacterInfo>();

        foreach (InspectorCharInfo val in inspectorCharValues) {
            cInfo.Add(CalculateCharacterInfo(val.character, val.referenceNumber));

            if (val.character >= 'A' && val.character <= 'Z') {
                cInfo.Add(CalculateCharacterInfo((char)(val.character + ('a' - 'A')), val.referenceNumber));
            }
        }

        // Manual additions (Useful for special characters)
        //cInfo.Add(CalculateCharacterInfo('A', 899));

        font.characterInfo = cInfo.ToArray();
    }


    private CharacterInfo CalculateCharacterInfoHelper(char c, int columnInSheet, int rowInSheet) {
        CharacterInfo info = new CharacterInfo();
        info.index = (int)c;
        info.uvBottomLeft = new Vector2( (float)columnInSheet * UV_W, (float)rowInSheet * UV_H );
        info.uvBottomRight = info.uvBottomLeft + new Vector2(UV_W, 0);
        info.uvTopLeft = info.uvBottomLeft + new Vector2(0, UV_H);
        info.uvTopRight = info.uvBottomLeft + new Vector2(UV_W, UV_H);
        info.size = 0; // Default size
        info.advance = TILE_SIZE; // Depends on pixel size
        info.minX = 0;
        info.minY = -TILE_SIZE;
        info.maxX = TILE_SIZE;
        info.maxY = 0;

        return info;
    }

    public CharacterInfo CalculateCharacterInfo(char c, int referenceNumber) {
        
        int colInSheet = referenceNumber % NUM_COLS;
        int rowInSheet = NUM_ROWS - (int)(Mathf.Floor(referenceNumber / NUM_COLS)) - 1;

        Debug.Log("Generating Character info for: " + c);
        Debug.Log("ColInSheet: " + colInSheet);
        Debug.Log("RowInSheet: " + rowInSheet);

        return this.CalculateCharacterInfoHelper(c, colInSheet, rowInSheet);
    }
}
