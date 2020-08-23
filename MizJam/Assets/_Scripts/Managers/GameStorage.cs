using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameStorage : PersistableObject {
    
    public PersistentStorage storage;

    public float autoSaveTime = 10f;
    public float timeSinceLastSave = 0f;

    public void Initialize() {
        this.storage.Initialize();
        this.LoadGame();
        GameManager.Instance.AfterLoad();
    }

    public void SaveGame(bool force = true) {
        if (!force && this.timeSinceLastSave <= autoSaveTime / 2 ) {
            return;
        }

        this.timeSinceLastSave = 0;
        storage.Save(this);
    }

    public void DeleteSaveFile() {
        storage.DeleteSaveFile();
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



