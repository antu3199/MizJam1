using System.IO;
using UnityEngine;

public class GameDataWriter {


    #if UNITY_WEBGL



	private int prefsIndex = 0; // Last minute workaround

	public GameDataWriter (BinaryWriter writer) {
	}

	public void Write (bool value) {
        //Debug.Log("Save value: " + value);
        GameDataWriter.SetBool(prefsIndex++.ToString(), value);
	}


	public void Write (float value) {
        //Debug.Log("Save value: " + value);
        PlayerPrefs.SetFloat(prefsIndex++.ToString(), value);
	}

	public void Write (int value) {
        //Debug.Log("Save value: " + value);
		PlayerPrefs.SetInt(prefsIndex++.ToString(), value);
	}

    public void Write(double value) {
        //Debug.Log("Save value: " + value);
        GameDataWriter.SetDouble(prefsIndex++.ToString(), value);
    }

	public void Write (Quaternion value) {
        /*
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
        */
	}

	public void Write (Vector3 value) {
        /*
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
        */
	}

     public static void SetBool(string key, bool state) {
         PlayerPrefs.SetInt(key, state ? 1 : 0);
     }
 
     public static bool GetBool(string key) {
         int value = PlayerPrefs.GetInt(key);
 
         if (value == 1) {
             return true;
         } else {
             return false;
         }
     }


    public static void SetDouble(string key, double value) {
        PlayerPrefs.SetString(key, DoubleToString(value));
    }
    public static double GetDouble(string key, double defaultValue) {
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }
    public static double GetDouble(string key) {
        return GetDouble(key, 0d);
    }
    
    private static string DoubleToString(double target) {
        return target.ToString("R");
    }
    private static double StringToDouble(string target) {
        if (string.IsNullOrEmpty(target))
            return 0d;
    
        return double.Parse(target);
    }

    #else

	BinaryWriter writer;

    private int writerIndex = 0; // Workaround since WebGL only supports playerprefs

	public GameDataWriter (BinaryWriter writer) {
		this.writer = writer;
	}

	public void Write (bool value) {
        //Debug.Log("Save value: " + value);
		writer.Write(value);
	}


	public void Write (float value) {
        //Debug.Log("Save value: " + value);
		writer.Write(value);
	}

	public void Write (int value) {
        //Debug.Log("Save value: " + value);
		writer.Write(value);
	}

    public void Write(double value) {
        //Debug.Log("Save value: " + value);
        writer.Write(value);
    }

	public void Write (Quaternion value) {
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}

	public void Write (Vector3 value) {
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
	}

    #endif

}