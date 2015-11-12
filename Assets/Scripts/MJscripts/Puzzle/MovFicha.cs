using UnityEngine;
using System.Collections;
//Para usar canvas
using UnityEngine.UI;



public class MovFicha : MonoBehaviour {

	//Variables para fichas

	public bool mover;
	//Referencia sobre la cual nos vamos a mover
	public Transform hueco;
	//Guarda la referencia central(hueco=
	private float xMove;
	private float yMove;


	//Variables para verificar tag del objeto(trigger&foto)

	public string tagObj;

    public bool Acerto;
	//Variables para modificar el scrip Puntos

	public GameObject script;
	//Para modificar puntos a traves de la variable
	//private Puntos puntosMod;
	private int puntosMod ;

	//Variables para elementos de Canvas

	//public GameObject canvas;
	//private Text texto;






	// Use this for initialization
	void Start () {
	

		//Se puede mover al inicio
		mover = true;

		//Obtenemos el tag del objeto
		tagObj = gameObject.tag;

		//LOLLLLLLLLLLLLLLLLLLLLLLL ¿SE PUEDE IMPLEMENTAR DE OTRA FORMA, QUE SIG???????????
		//puntosMod = script.GetComponent<Puntos>();
		puntosMod = 0;

		//Obtengo el componente texto del canvas(para modificar)
		//texto = canvas.GetComponent<Text> ();

		//Vacio por defecto
	//	texto.text = "Es una foto de mi padre ";
	}
	
	// Update is called once per frame
	void Update () {
	
		//Comprobaos
		/*Comprobamos que en cada fotograma si hemos alcanzado
		 los 8 puntos (puzzle terminado)*/
		if(puntosMod == 8){

			//texto.text = "Es mi padre, Pedro Crespo, en la Plaza de Zalamea";

			Debug.Log("Acerto");
		    Acerto = true;
			//Ya se acaba el juego, no movemos fichas
			mover = false;

		}



	}


	/*

	Cuando el tag del objeto other es igual al tag
	del objeto propio(script)

	 */
	void OnTriggerEnter(Collider other){

		/*
			Si el tag de otro objeto es igual
			al tag del objeto obtenido(cada uno tiene este script)
		 */
		if (other.tag == tagObj) {

			//Aumentamos los puntos
			puntosMod+=1;
			Debug.Log(puntosMod);
		}





	}

	/*void OnGUI(){

		GUI.Label(new Rect (10, 10, 100, 20), "Este es mi padre");

	}
*/
	/*

	Cuando el tag del objeto other es igual al tag
	del objeto propio(script)

	 */
	void OnTriggerExit(Collider other){
		
		/*
			Si el tag de otro objeto es igual
			al tag del objeto obtenido(cada uno tiene este script)
		 */
		if (other.tag == tagObj) {
			
			//Decremento los puntos
			puntosMod -=1;
			Debug.Log(puntosMod);
		}
	}


	//Funcion cuando se levanta el raton despues de pulsar
	void OnMouseUp(){


		//Si quedan por colocar fichas
		if (mover == true) {
		
			/*
				Si la distancia entre la posicion de la ficha(que tiene el script)
				y la posicion del "hueco" -> es 1
			 */
			if(Vector3.Distance (transform.position, hueco.position) == 1){

				//Guardamos la x,y de la ficha a mover -> la usara el hueco
				xMove = transform.position.x;
				yMove = transform.position.y;

				//Movemos la ficha a la posicion del hueco
				transform.position = new Vector3 (hueco.position.x,hueco.position.y,0);

				//Movemos el hueco a la posicion de la ficha movida
				hueco.position = new Vector3 (xMove,yMove,0);

			}
		
		}







	}
}
