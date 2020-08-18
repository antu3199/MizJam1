using System.IO;
using UnityEngine;

public class GameDataReader {

	BinaryReader reader;

	public GameDataReader (BinaryReader reader) {
		this.reader = reader;
	}

	public float ReadFloat () {
        float val = reader.ReadSingle();
        //Debug.Log("Load value: " + val);
		return val;
	}

    public double ReadDouble() {
        double val = reader.ReadDouble();
        //Debug.Log("Load value: " + val);
        return val;
    }

	public int ReadInt () {
        int val = reader.ReadInt32();
        //Debug.Log("Load value: " + val);
		return val;
	}

	public Quaternion ReadQuaternion () {
		Quaternion value;
		value.x = reader.ReadSingle();
		value.y = reader.ReadSingle();
		value.z = reader.ReadSingle();
		value.w = reader.ReadSingle();
		return value;
	}

	public Vector3 ReadVector3 () {
		Vector3 value;
		value.x = reader.ReadSingle();
		value.y = reader.ReadSingle();
		value.z = reader.ReadSingle();
		return value;
	}
}