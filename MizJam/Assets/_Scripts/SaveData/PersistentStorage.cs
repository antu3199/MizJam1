using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {

	string savePath;

    public void Initialize() {
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


        #if UNITY_WEBGL
            o.Save(new GameDataWriter(null));
        #else
		using (
			var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
		) {
			o.Save(new GameDataWriter(writer));
		}
        #endif
	}

	public void Load (PersistableObject o) {
        Debug.Log("Loading file: " + this.savePath);

        #if UNITY_WEBGL
            if (!PlayerPrefs.HasKey("0")) {
                Debug.Log("First time playing. Loading canceled!");
                return;
            }

            o.Load(new GameDataReader(null));
        #else
        if (!File.Exists(this.savePath)) {
            Debug.Log("First time playing. Loading canceled!");
            return;
        }

		using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
		) {
			o.Load(new GameDataReader(reader));
		}
        #endif
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
        Debug.Log("Deleting save: " + this.savePath);
        #if UNITY_WEBGL
        PlayerPrefs.DeleteAll();
        #else
        File.Delete(this.savePath);

        #endif
    }
}
