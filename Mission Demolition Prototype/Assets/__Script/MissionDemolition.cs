using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode modes = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start(){
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        //Destruye un Castillo si es que existe
        if (castle != null) {
            Destroy(castle);
        }
        //Destruye los proyectiles anteriores
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos){
            Destroy(pTemp);
        }
        //Instancia un nuevo castillo
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        //Reinicia la camara 
        SwitchView("wShow Both");
        ProjectileLine.S.Clear();
        //Reinicia el goal
        Goal.goalMet = false;

        UpdateGUI();

        modes = GameMode.playing;
    }

    void UpdateGUI(){
        //Muestra los datos en la Interfaz del usuario
        uitLevel.text = "Level: " + (level + 1) + "of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update(){
        UpdateGUI();
        //Revisar por final de un nivel
        if ((modes == GameMode.playing) && Goal.goalMet) {
            //Cambiar el modo para dejar de revidar por niveles
            modes = GameMode.levelEnd;
            //Alejar la camara
            SwitchView("Show Both");
            //Empezar el siguiente nivel en 2 segundos
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "") {
        if (eView == "") {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing) {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "showCastle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired() {
        S.shotsTaken++;
    }
}

