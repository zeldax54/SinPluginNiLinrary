using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
 * Prefab que aparecera en todas las escenas
 * para poder acceder a los componentes del
 * script(Para poder depurar escena a escena )
 * */
public class VariablesPersonaje : MonoBehaviour {

	//Instancia al objeto
	public static VariablesPersonaje  variablesPersonaje;

	//Variables del personaje PREGUNTAR SI HACER PRIVADO (GET Y SET)
	private  static int diamantesP ;
	private  static int orgulloP ;


	private String rutaArchivo;

	void Awake(){

		//obtenemos ruta
		rutaArchivo = Application.persistentDataPath + "/datos.dat";

		//Si es la primera vez
		if (variablesPersonaje == null) {

			//Referencia al componente que ejecuta el codigo
			variablesPersonaje = this;

			/*
		 * Objeto que no se destruira cuando cambiamos
		 * de escenas
		 * */
			DontDestroyOnLoad (gameObject);
		} else if (variablesPersonaje != this) {

			//Destruyo componente que tiene el script
			Destroy(gameObject);

		}



	}



	public int getDiamantes(){

		return diamantesP;
	}

	public int getOrgullo()
	{
		return orgulloP;
	}


	public void UpdateDiamantes(int d)
	{
		diamantesP = d;
	}


	public void UpdateOrgullo(int o)
	{
		orgulloP = o;
	}



	// Use this for initialization
	void Start () {
	
		Cargar ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void Guardar()
	{
		//Serializamos la clase DatosAGuardar
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(rutaArchivo);
		
		DatosAGuardar datos = new DatosAGuardar();
		datos.setOrgulloMax (orgulloP);
		datos.setDiamantesMax(diamantesP);
		
		bf.Serialize(file, datos);
		
		file.Close();
	}


	void Cargar()
	{
		if(File.Exists(rutaArchivo)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(rutaArchivo, FileMode.Open);

			///Casting para que el objeto sea tratado
			DatosAGuardar datos = (DatosAGuardar) bf.Deserialize(file);


			diamantesP = datos.getDiamantesMax();
			orgulloP = datos.getOrgulloMax();

			
			file.Close();
		}else{
			diamantesP = 5;
			orgulloP = 5;
		}

	}
}


[Serializable]
class DatosAGuardar
{
	private int diamantesMax;
	private int orgulloMax;

	/* public DatosAGuardar(int diamantes, int orgullo)
	{
		diamantesMax = diamantes;
		orgulloMax = orgullo;
	}*/


	public int getDiamantesMax()
	{
		return diamantesMax;
	}

	public int getOrgulloMax()
	{
		return orgulloMax;
	}


	public void setDiamantesMax(int diamantes) {
		diamantesMax = diamantes;
	}

	public void setOrgulloMax(int orgullo)
	{
		orgulloMax = orgullo;
	}


}