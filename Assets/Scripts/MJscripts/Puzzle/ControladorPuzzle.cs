using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.UI;

public class ControladorPuzzle : MonoBehaviour {


	//Enfoca elementos principal
	public Camera camP;
	//Enfoca elemento mapa, secundario
	public Camera camS;

	//TextMesh de los avisos
	public TextMesh diamantesT;
	public TextMesh orgulloT;
	public TextMesh nivelT;
	public TextMesh numAyudaT;


	//Estas son las variables del jugador(defecto)
	private int orgullo = 5 ;
	private int diamantes  = 1;

	
	//Variables internas(las que se va pasar)
	private int fallos = 0;
	private int aciertos = 0;
	private int movimientos = 0;
	private int ayudas = 0;
	
	//Saber en que nivel estoy
	private int nivel = 1;
	
	
	//Objeto de tipo del script ModalPanel
	private ModalPanel _modalPanel;
	public Button aceptarButton;
	//Boton cancelar del panel
	public Button cancelarButton;
	
	private VariablesPersonaje variablesPersonaje;


	private bool primeraVez = true;

	//Objeto que contiene el script
    public GameObject objMov;
    private MovFicha mov;

	//Iniciaiza atributos al principio del todo
	private void Awake()
	{
        //Para saber cuando acerto
        mov = objMov.GetComponentInChildren<MovFicha>();
		
		//diamantes = VariablesPersonaje.variablesPersonaje.getDiamantes ();
		//orgullo = VariablesPersonaje.variablesPersonaje.getOrgullo ();
		
		/*
        * Actualizo las variables TextMesh que tienen que modificarse
        * y verse en pantalla a lo largo del minijuego.
        */
		
		UpdateTextMesh(diamantesT, diamantes.ToString());
		UpdateTextMesh(orgulloT, orgullo.ToString());
		UpdateTextMesh(nivelT, (nivel + " / " + nivel));
		UpdateTextMesh(numAyudaT, diamantes.ToString());


	
		
		

	
		// Le asigno una instancia del script Modal Panel
		_modalPanel = ModalPanel.Instance();
	}

	void Start () {
		
		//Por defecto la camara principal y secundaria
		camP.GetComponent<Camera> ().enabled = true;
		camS.GetComponent<Camera> ().enabled = false;
	}
	
	/*
 * Metodo para actualizar TextMesh
 * t = TextMesh a modificar su texto
 * valor = nuevo valor para el TextMesh
 */
	private void UpdateTextMesh(TextMesh t, string valor)
	{
		t.text = valor;
	}


	public void ChoiceHelp()
	{
		//Si me quedo sin 
		if (diamantes == 0) {

			_modalPanel.Elejir ("No te quedan diamantes para  poder pedira ayuda! " + "\n"+ "Intenta resolver el puzzle",
			                   _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
		} else {

			
			if(primeraVez)
			{

				/*Avisamos lo que cuesta pedir una pista 
                 * Botones aceptar (accion ejecutar la pista) y cancelar.
                 */
				_modalPanel.Elejir("Una pista cuesta -1.ORGULLO."+ "\n"+ "Deseas continuar?",
				                   EjecutaAyuda, aceptarButton, cancelarButton, false);


			}
			else{
				EjecutaAyuda();
			}



		}
	}
	
	private void EjecutaAyuda()
	{


		//Cuando se pide ayuda
		if (primeraVez) {

			//Solo aparece una vez el aviso
			primeraVez = false;
		} else {
			//Una vez pedido ayuda se quiere volver al puzzle
			primeraVez = true;

			
			//Actualizo variables
			diamantes--;
			ayudas++;
			
			UpdateTextMesh (diamantesT, diamantes.ToString ());
			UpdateTextMesh (numAyudaT, diamantes.ToString ());

		}

	




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


	
	
		//Cerramos panel
		_modalPanel.CerrarPanel();
	}

	public void PasarNivel()
	{

		
		/*
         * LLamamos al metodo elejir del scrip ModalPanel
         * para que ejecute la accion a convenir
         */
		_modalPanel.Elejir("Este es el unico nivel." + "\n"+ " Deseas salir?",
		                   Rendirse, aceptarButton, cancelarButton, false);
	}
	
	
	private void Rendirse()
	{
		
		_modalPanel.Elejir ("Es una fotografia familiar."+ "\n"+ "Es mi padre Crespo y mi hermana Isabel!",
		                   FinJuego, aceptarButton, cancelarButton, true);
	}


	

		
	private void FinJuego()
	{
		Application.LoadLevel("MenuMain");
	}

	// Update is called once per frame
	void Update () {

	    if (mov.Acerto)
	    {
            _modalPanel.AvisoAcierto("Puzle acertado", FinJuego,aceptarButton,cancelarButton,null);
	    }
	}
}
