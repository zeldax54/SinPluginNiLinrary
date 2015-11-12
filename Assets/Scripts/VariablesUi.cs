using UnityEngine;
using System.Collections;
using System.Linq;

public class VariablesUi : MonoBehaviour {




	private int orgulloUI;
	private int diamantesUI;

	/*
     Text para actualizar las variables en la pantalla, aqui
     * asigo el valor no el indicador!
     */
	private TextMesh OrgulloTextMesh;
	private TextMesh DiamantesTextMesh;

	//Para poder acceder variables generales
	private VariablesPersonaje variablesPersonaje;

	// Use this for initialization
	void Start () {
	

		OrgulloTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "OrgulloValor");
		DiamantesTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "DiamantesValor");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
