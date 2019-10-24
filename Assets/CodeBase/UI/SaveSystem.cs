using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour {

    //to be used with the buttons
    public void NewGame() {
        PlayerPrefs.SetInt("aristotle_save", 0); //a save value of 0 will mean new game
        PlayerPrefs.Save();
    }

    public void SimulateLevelClear(int whichLevel) {
        PlayerPrefs.SetInt("aristotle_save", whichLevel);
        PlayerPrefs.Save();
    }

    //load the according level/scene
    public void LoadLastSave() {
        int targetScene = PlayerPrefs.GetInt("aristotle_save") + 1;

        print("We would begin level " + targetScene);
    }
}
