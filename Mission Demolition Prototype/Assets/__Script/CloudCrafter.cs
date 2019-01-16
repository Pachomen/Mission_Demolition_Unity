using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour{

    [Header("Set in Inspector")]
    public int numClouds = 40; //Numero de nubes
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; //Escala minima por cada nube
    public float cloudScaleMax = 3; //Escala maxima por cada nube
    public float cloudSpeedMult = 0.5f; //Ajuste de la velocidad de cada nube

    private GameObject[] cloudInstance;

    void Awake(){
        //Hacer un arregle tan grande para meter el numero de nubes para crear
        cloudInstance = new GameObject[numClouds];
        //Buscar el cloudAnchor parent
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Iterar para hacer el numero de nubes
        GameObject cloud;
        for (int i = 0; i < numClouds; i++) {
            //Crear una intancia de cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            //Poscion de la nube
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //Scala de la nube
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Nubes mas pequeñas (con un ScaleU pequeño) deben estar mas cerca al piso
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;
            //Aplicar tranforma la nube
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Hacer la nube pariente de Anchor
            cloud.transform.SetParent(anchor.transform);
            //Añadir la nube a la lista
            cloudInstance[i] = cloud;
        }
    }

    void Update(){
        foreach (GameObject cloud in cloudInstance){
            //Tomar la posicion y la escala de la nube
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //Mover las nubes mas grandes mas rapido
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //Si una nube se muebe mucho a la izquierda
            if (cPos.x <= cloudPosMin.x){
                cPos.x = cloudPosMax.x;
            }
            //Aplicar la nueva posicion de la nube
            cloud.transform.position = cPos;
        }    
    }

}
