using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameStorage : PersistableObject {
    
    public PersistentStorage storage;


    public void Initialize() {
        this.LoadGame();
        GameManager.Instance.AfterLoad();
    }

    public void SaveGame() {
        storage.Save(this);
    }

    public void LoadGame() {
        storage.Load(this);
    }

	public override void Save (GameDataWriter writer) {
        GameManager.Instance.gameState.Save(writer);
	}

	public override void Load (GameDataReader reader) {
        GameManager.Instance.gameState.Load(reader);
	}

}



