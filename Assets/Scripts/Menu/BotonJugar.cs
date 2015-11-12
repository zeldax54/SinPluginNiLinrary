using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;


public class BotonJugar : MonoBehaviour {


	private GameObject MainCamera;
    //Este es el boton que cree en la pantalla principal
    private Button botonMj1;
	private Button botonMj2;
	private Button botonMj3;
	private Button botonMj5;


	// Use this for initialization
	void Start () {

        botonMj1 = FindObjectsOfType<Button>().First(a => a.name == "Bm1");//Busco el boton que cree
        botonMj1.onClick.RemoveAllListeners();//Quito los eventos onclick por si los tubiera
        botonMj1.onClick.AddListener(AccionBmj1);//Le agrego en el evento onclick el metodo accion



		botonMj2 = FindObjectsOfType<Button>().First(a => a.name == "Bm2");//Busco el boton que cree
		botonMj2.onClick.RemoveAllListeners();//Quito los eventos onclick por si los tubiera
		botonMj2.onClick.AddListener(AccionBmj2);//Le agrego en el evento onclick el metodo accion

		botonMj3 = FindObjectsOfType<Button>().First(a => a.name == "Bm3");//Busco el boton que cree
		botonMj3.onClick.RemoveAllListeners();//Quito los eventos onclick por si los tubiera
		botonMj3.onClick.AddListener(AccionBmj3);//Le agrego en el evento onclick el metodo accion

		botonMj5 = FindObjectsOfType<Button>().First(a => a.name == "Bm5");//Busco el boton que cree
		botonMj5.onClick.RemoveAllListeners();//Quito los eventos onclick por si los tubiera
		botonMj5.onClick.AddListener(AccionBmj5);//Le agrego en el evento onclick el metodo accion
	}
	

	private void AccionBmj5()
	{
		//Cargo la escena del minigame esto es para probar se que despues se puede cargar desde otrea parte
		Application.LoadLevel("MJfotoPuzzle");
	}

	private void AccionBmj3()
	{
		//Cargo la escena del minigame esto es para probar se que despues se puede cargar desde otrea parte
		Application.LoadLevel("MJarrastra");
	}

	private void AccionBmj2()
	{
		//Cargo la escena del minigame esto es para probar se que despues se puede cargar desde otrea parte
		Application.LoadLevel("MJretoInicial");
	}

    private void AccionBmj1()
    {
        //Cargo la escena del minigame esto es para probar se que despues se puede cargar desde otrea parte
        Application.LoadLevel("MJdeberes");
    }

	/*Reacciona al click del objeto que tiene este scrip*/
	void OnMouseDown(){

		//Desactivamos sonido actualmente
//		MainCamera.GetComponent<AudioSource>().Stop();
		//Se activa el sonido del boton
		GetComponent<AudioSource> ().Play ();


		//Cargamos la siguiente escena
        Invoke("AccionBmj1", GetComponent<AudioSource>().clip.length);


	}

	void CargarEscena(){
		Application.LoadLevel ("IntroBosque");
	}
}
