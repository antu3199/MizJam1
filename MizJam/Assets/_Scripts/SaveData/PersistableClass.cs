using UnityEngine;

public class PersistableClass {

	public virtual void Save (GameDataWriter writer) {
		//writer.Write(transform.localPosition);
	}

	public virtual void Load (GameDataReader reader) {
		//transform.localPosition = reader.ReadVector3();
	}
}