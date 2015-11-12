using UnityEngine;
using System.Collections;

public class CambioCam : MonoBehaviour {

	//Enfoca elementos principal
	public Camera camP;
	//Enfoca elemento mapa, secundario
	public Camera camS;

	// Use this for initialization
	void Start () {
	
		//Por defecto la camara principal y secundaria
		camP.GetComponent<Camera> ().enabled = true;
		camS.GetComponent<Camera> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Si selecciono la tecla
		if (Input.GetKeyDown ("c")) {

			//Y la camP esta activa desactivo y activo la camS
			if(camP.GetComponent<Camera>().enabled == true){

				camP.GetComponent<Camera> ().enabled = false;
				camS.GetComponent<Camera> ().enabled = true;
			}
			else if(camS.GetComponent<Camera>().enabled == true){

				//Si camS activada, desactivo y activo camP
				camS.GetComponent<Camera> ().enabled = false;
				camP.GetComponent<Camera> ().enabled = true;

			}
		}

	
	}
}
