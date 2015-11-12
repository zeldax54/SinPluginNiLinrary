using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;

/*
 * Metodo que permite controlar el estado de las palabras
 * de la lista de palabras disponibles para rellenar un "_"
 * Reacciona cuando es arrastrado de la lista a un hueco "_"
 * Reacciona cuando se quiere cambiar una palabra por otra
 * en la posicion de "_"
 * */

public class ControladorPalabra : MonoBehaviour {

	//Saber si se ha soltado la palabra
	private bool _soltando;


	/*
	 * Metodo que es llamado si 2 objetos
	 * con rigibody colisionan
	 * Permite comportamiento como respuesta
	 * a una colision entre 2 objetos.
	 * Obtiene elTextMesh del objeto que colisiono
	 * 
	 * other : colliderl del objeto que colisiona
	 * 
	 * */
	private void OnTriggerEnter(Collider other)
	{
		//Si el collider con el que choca es una linea
		if (other.name.Contains("Linea"))
			//EN QUE ELEMENTO DE ESCENA TIENES PUESTO "Linea"?¿?¿? PARA QUE COJA
		{
			/*
			 * Obtengo el TextMesh del objeto que colisiono
			 * a traves de una pequeña consulta Linq
			 * */
			TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);
			//Aumentamos el tamaño de linea
			linea.characterSize += 0.05f;
		}
	}

	/*
	 * Metodo que se llama una vez cada frame para
	 * cada collider que esta en contacto con el trigger
	 * 
	 * 
	 * 
	 * */
	private void OnTriggerStay(Collider other)
	{
		//Inicializamos 
		_soltando = false;

		//Si el collider con el que choca es una linea
		if (other.name.Contains("Linea"))
		{
			/*
			 * Obtengo el TextMesh del objeto que colisiono
			 * a traves de una pequeña consulta Linq
			 * */
			TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);

			//Obtengo el objeto que creara la linea 
			ManejadorLinea manejadorLinea = linea.GetComponent<ManejadorLinea>();

			/*
			 * Si suelta el clic
			 * */
			if (Input.GetMouseButtonUp(0))//Si suelta el clic 
			{


				//Caso 1:Si la linea(Ui en escena) contiene _ (significa que no hay palabra puesta = (vacio))
				if (manejadorLinea.LineaDisponible())
				{
					//Encontrar la posicion de la 1ra _ (vacio)
					int posicion = manejadorLinea.FindFirst_();

					//Agrego la palabra a la linea donde esten las __
					manejadorLinea.PutText(gameObject.GetComponent<TextMesh>(),posicion);
					//Agrego la palabra a la linea donde esten las __
					manejadorLinea.Remove_();

				}

				//Caso 2 Si la linea  tiene palabras que ya han sido puestas 
				else if (manejadorLinea.ContienePalabraDeResp() && manejadorLinea) 
				{
					//Si la linea tiene color rojo es q tiene palabras puestas

					/*
					 * Obtengo la palabra antigua(donde quiero poner la nueva) 
					 * y coloco la palabra nueva
					 * La palabra antigua la devuelvo junto a las otras palabras
					 * por colocar
					 * 
					 * */

					string oldword=manejadorLinea.FindandReplaceRedWord(gameObject.GetComponent<TextMesh>().text);
				
					//Devolvemos la palabra a su sitio
					BajarPalabra(oldword);
					
				}

				_soltando = true;
				
			}
		}
	}

	/*
	 * Metodo llamado cuando el collider de objeto
	 * dejar de interaccionar con el trigger
	 * 
	 * 
	 * other= collider del objeto que inicia al trigger
	 * 
	 * */
	private void OnTriggerExit(Collider other)
	{
		//Si no solto la palabra en linea volver a su color original
		if (other.name.Contains("Linea")) 
		{
			TextMesh linea = FindObjectsOfType<TextMesh>().First(a => a.name == other.name);
			linea.characterSize -= 0.05f;
			//Saco de la pantalla  la palabra para que no este disponible en la lista de palabras
			if (_soltando)
				SubirPalabra(gameObject);
			
		}
	}

	/*
	 * Metodo que oculta una palabra 
	 * que haya sido puesta en algun "_"
	 * 
	 * o = gameobject que contiene este script, es decir
	 * una palabra de las posibles para rellenar un "_"
	 * */
	public void SubirPalabra(GameObject o)
	{
		o.transform.position = new Vector3(o.transform.position.x+20f,o.transform.position.y,o.transform.position.z);
	}


	/*
	 * Metodo que coloca la palabra junto a las demas
	 * palabras que tendran que ser colocadas en _
	 * 
	 * palabra = palabra que sea colocada en su posicion inicial
	 * */
	public void BajarPalabra(string palabra)
	{
		TextMesh o =
			FindObjectsOfType<TextMesh>().First(a => a.text == palabra);

		//Le devuelvo a la posicion original
		o.transform.position = new Vector3(o.transform.position.x-20f,o.transform.position.y,o.transform.position.z);
		
	}




}
