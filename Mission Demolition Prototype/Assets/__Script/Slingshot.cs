using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour{
    static private Slingshot S;
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    void Awake(){
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void Update(){
        if (!aimingMode) return;

        //Toma las coordenadas 2D del mouse 
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //Encuntra el Delta de launchPos a mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limite de mouseDelta al radio de la esfera
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {//Si mouse Delta pasa de maxMagnitude el vector se convierte en 1 y obtiene los valores de maxMagnitude
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = mouseDelta + launchPos;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {
            //El mouse lanza el projectile
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
        
    }

    static public Vector3 LAUNCH_POS {
        get {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void OnMouseEnter(){//Reconoce la entrada del mouse en el area de accion
        //print("Slingshot:OnMouseEnter");
        launchPoint.SetActive(true);
    }

    void OnMouseExit(){//Reconoce la salida del mouse en el area de accion
        //print("Slingshot:OnMouseExit");
        launchPoint.SetActive(false);
    }

    void OnMouseDown(){//Al presionar el mouse activa
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;//Crea una instancia del projectil y la pone en posicion
        projectile.transform.position = launchPos;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
}
