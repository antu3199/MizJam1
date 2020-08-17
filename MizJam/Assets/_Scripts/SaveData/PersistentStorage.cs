using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {

	string savePath;

	void Awake () {
		savePath = Path.Combine(Application.persistentDataPath, "saveFile");
	}

	public void Save (PersistableObject o) {
        Debug.Log("Saving file: " + this.savePath);

		using (
			var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
		) {
			o.Save(new GameDataWriter(writer));
		}
	}

	public void Save (PersistableClass o) {
        Debug.Log("Saving file: " + this.savePath);

		using (
			var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
		) {
			o.Save(new GameDataWriter(writer));
		}
	}


	public void Load (PersistableObject o) {
        Debug.Log("Loading file: " + this.savePath);
        if (!File.Exists(this.savePath)) {
            Debug.Log("First time playing. Loading canceled!");
            return;
        }

		using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
		) {
			o.Load(new GameDataReader(reader));
		}
	}

	public void Load (PersistableClass o) {
        Debug.Log("Loading file: " + this.savePath);
        if (!File.Exists(this.savePath)) {
            Debug.Log("First time playing. Loading canceled!");
            return;
        }

		using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
		) {
			o.Load(new GameDataReader(reader));
		}
	}

    // :(
    public void DeleteSaveFile() {
        File.Delete(this.savePath);
    }
}
