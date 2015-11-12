
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;


public class ControladorAdi : MonoBehaviour {

    // que contiene la lista de adivinanzas y el metodo buscar adivinanza

    /*
     * Objeto de la clase adivinanza que
     * contiene la lista de adivinanzas
     * y el metodo buscar adivinanza
     * 
     * 
     * */
    private readonly Adivinanza _adivinanza = new Adivinanza();

    //Texto de la Ui que mostrara las adivinanzas
    public Text adivinanzaPregunta;

    //Botones donde se encuentras las posibles respuestas
    public Button posible1;
    public Button posible2;
    public Button posible3;

    //Estas son las variables del jugador(defecto)
    private int orgullo ;
    private int diamantes;


    //Variables internas(las que se va pasar)
    private int fallos = 0;
    private int aciertos = 0;
    private int siguientes = 0;
    //Ayudas permitidas( sabremos cuantas pide)
    private int ayudas = 0;

    //Saber en que nivel estoy
    private int _adivinanzaNivel = 1;
	// Para saber si ya ha usado su ayuda la adivinanza actual CCCC 

	//Para saber las ayudas permitidas
	private int numAyuda = 1;



    //TextMesh variables jugador
    public TextMesh orgulloTextMesh;
    public TextMesh diamantesTextMesh;
	//Internas
	public TextMesh FallosTextMesh;
	public TextMesh AciertosTextMesh;
	public TextMesh AyudasTextMesh;
	public TextMesh SiguientesTextMesh;
	public TextMesh numAyudaT;


    //TextMesh variables minijuego
    public TextMesh gameOver;
    public TextMesh adivinanzaNivel;

    //Guardo la respuesta de la adivinaza actual
    private string _respuestactual = "";

    //Parte del panel para las notificaciones
    //Objeto de tipo del script ModalPanel
    private ModalPanel _modalPanel;
    //Boton aceptar del panel
    public Button aceptarButton;
    //Boton cancelar del panel
    public Button cancelarButton;
	//Boton aceptar ayuda para el panel ayuda
	public Button helpButton;



	private VariablesPersonaje variablesPersonaje;


     //Iniciaiza atributos al principio del todo
    private void Awake()
    {

		diamantes = VariablesPersonaje.variablesPersonaje.getDiamantes ();
		orgullo = VariablesPersonaje.variablesPersonaje.getOrgullo ();

        /*
        * Actualizo las variables TextMesh que tienen que modificarse
        * y verse en pantalla a lo largo del minijuego.
        */
        UpdateTextMesh(diamantesTextMesh, diamantes.ToString());
        UpdateTextMesh(orgulloTextMesh, orgullo.ToString());
		UpdateTextMesh(orgulloTextMesh, orgullo.ToString());
		UpdateTextMesh(numAyudaT, numAyuda.ToString());

        //Inicio la lista de adivinanzas
        _adivinanza.InicializarLista();
        // Le asigno una instancia del script Modal Panel
        _modalPanel = ModalPanel.Instance();
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
	// Use this for initialization
	void Start () {

		CargaNivel ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Esto es para q la respuesta salga en un boton al azar

    /*
     * 
     * Metodo para que las posibles respuestas salgan
     * en algun boton al alzar
     * botones : botones donde controlador
     * a: adivinanza correspondiente
     * 
     * */
    private void ColocarRespuesta(List<Button> botones, Adivinanza a)  {

 
		//Activo todos (por si alguno fue desactivado al pedir ayuda)
		foreach (var boton in botones) 
		{
			boton.gameObject.SetActive(true);
		}

		//Obtengo las respuestas no validas
		string[] noRespuestas = a.falsasrespuestas;
		//Obtengo posicion Random
		int pos = Random.Range(0, 2);
		/*
		 * Asigno un boton con posicion ramdon
		 * y le coloco la respuesta correcta
		 **/ 
		Button b = botones[pos];
		b.GetComponentInChildren<Text>().text = a.respuesta;

		/*
		 * Recorro los demas botones (los que 
		 * no tengan la respuesta valida)
		 * y les asigno una respuesta no valida
		 * */
		foreach (var boton in botones)
		{
			if (boton != b)
			{
				/*
				 * Obtengo la respuesta no valida de la pos 0 
				 * y desplazo y anulo la ultima posicion
				 * Siempre cogeremos la pos 0
				 * */
				boton.GetComponentInChildren<Text>().text=noRespuestas[0];
				//
				noRespuestas[0] = noRespuestas[1];
				noRespuestas[1] = null;
			}
		}

    }

	/*
	 * Metodo que comprueba si la respuesta seleccionada
	 * a traves del boton es la correcta correspontiene al nivel
	 * actual
	 * x: Boton que contiene la respuesta seleccionada
	 * 
	 * */
	public void Comprobar(Button x)
	{
		 
		//Si estamos en un nivel permitido y no esta activo el panel de avisos
		if (_adivinanzaNivel <= _adivinanza.ContAdivinanzas () && !_modalPanel.IsActivo ()) {

			//Compruebo si acierta(el contenido del boton es igual a la respusta del nivel actual
			if (x.GetComponentInChildren<Text>().text == _respuestactual)
			{
				//Actualizamos variables
				diamantes++;
				aciertos++;


				UpdateTextMesh(diamantesTextMesh, diamantes.ToString());
				UpdateTextMesh(AciertosTextMesh, aciertos.ToString());

				/*
                 * Aviso del acierto y pido
                 * ok para poder pasar al siguiente nivel
                 */
			/*	_modalPanel.Elejir("Has acertado!. +1.Diamante. Ganaste a la Chispa.Pasar siguiente nivel",
				                   SiguientePantalla, aceptarButton, cancelarButton, true);
*/
				/*
				 * 
				 * Aviso que se ha acertado
				 * */
				_modalPanel.AvisoAcierto("Has acertado!. +1.Diamante. Ganaste a la Chispa.Pasar siguiente nivel"
				                         ,SiguientePantalla,aceptarButton, cancelarButton,helpButton);


			}
			else
			{
				//Actualizamos variables 
				fallos++;
				diamantes--;

				UpdateTextMesh(FallosTextMesh, fallos.ToString());
				UpdateTextMesh(diamantesTextMesh, diamantes.ToString());

				/*
                 * Aviso del acierto y pido
                 * ok para poder pasar al siguiente nivel
                 */
				/*_modalPanel.Elejir("Has Fallado!. -1.Diamante.Gana la Chispa. Pasar siguiente nivel",
				                   SiguientePantalla, aceptarButton, cancelarButton, true);

*/
				/*
				 * 
				 * Aviso que se ha acertado
				 * */
				_modalPanel.AvisoAcierto("Has Fallado!. -1.Diamante.Gana la Chispa. Pasar siguiente nivel",
				                         SiguientePantalla,aceptarButton, cancelarButton,helpButton);

			}

		


		}





	}

	/*
     * Metodo que se lanza cuando se ha pulsado
     * ok del aviso correspondient al acierto
     * del nivel
     */
	private void SiguientePantalla()
	{
		//Cerramos panel
		_modalPanel.CerrarPanel ();
		//Aumentamos el nivel y pasamos al siguiente nivel
	//	_adivinanzaNivel++;
		NextNivel(false);
	}


	/*
	 * Metodo que permite pasar al siguiente nivel
	 * Comprueba si hemos llegado al final del minijuego
	 * En caso contrario comprueba si el jugador
	 * se ha rendido en el nivel(siguiente) y comprobamos
	 * que exista otro nivel para pasar.
	 * 
	 * Metodo sera asignado al panel que aparecece cuando
	 * queremos pasar de Nivel!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	 * 
	 * rindiendose: nos indica si el jugador se ha rendido
	 * en el nivel ( siguiente)
	 * */
	public void NextNivel(bool rindiendose)
	{
		//Al rendirse pasamos a otro nivel
		_adivinanzaNivel++;
		
		//Si he completado todos los niveles
		if (_adivinanzaNivel > _adivinanza.ContAdivinanzas())
		{
				/*
			 * Si no me rendido en ningun nivel(siguiente)
			 * y no he tenido ningun fallo (acierto todos los niveles)
			 * Recompenso con 1 orgullo por acertar todo!!
			 * 
			 * */
				if (siguientes == 0 && fallos == 0)
				{
					orgullo++;
					UpdateTextMesh(orgulloTextMesh, orgullo.ToString());
				/*
             * Avisamos que se ha terminado
             * el minijuego
             * */
				_modalPanel.AvisoAcierto("Terminaste la apuesta con la Chispa!. Acertaste todo. Recompensa +1.Orgullo.Continuar"
				                         ,FinJuego,aceptarButton, cancelarButton,helpButton);


				}
			else{


				/*
             * Avisamos que se ha terminado
             * el minijuego
             * */
				/*_modalPanel.Elejir("Terminaste la apuesta con la Chispa!. Continuar",
				                   FinJuego, aceptarButton, cancelarButton, true);
				*/
				_modalPanel.AvisoAcierto("Terminaste la apuesta con la Chispa!. Continuar"
				                         ,FinJuego,aceptarButton, cancelarButton,helpButton);


			}




			/*if (_modalPanel.IsActivo())
				_modalPanel.CerrarPanel();//Cierro el panel si esta abierto
			gameOver.text = "Todo Completado";
			*/
		}
		else
			//Si hay mas niveles
		{

			//La ayuda para el siguiente nivel se reinicia
			numAyuda= 1;
			UpdateTextMesh(numAyudaT, numAyuda.ToString());

			//Compruebo si se ha rendido en el nivel actual
			if (rindiendose)
			{
				//Cierro el panel si esta abierto
				if(_modalPanel.IsActivo())
					_modalPanel.CerrarPanel();

				//Actualizamos variables
				diamantes--;
				siguientes++;

				UpdateTextMesh(diamantesTextMesh, diamantes.ToString());
				UpdateTextMesh(SiguientesTextMesh, siguientes.ToString());

			

			}
			//Si hay proxima adivinanza
			if (_adivinanzaNivel <=_adivinanza.ContAdivinanzas())
				CargaNivel();

		}
	}

	/*
	 * Metodo que actualiza los datos
	 * para el siguiente nivel
	 * 
	 * */
	private void CargaNivel()
	{
        

		/*
		 * Obtengo la siguiente adivinanza(pregunta)
		 * y actualizo los datos 
		 * 
		 * */
		Adivinanza a = _adivinanza.Getadivinazas(_adivinanzaNivel.ToString());
		List<Button> lista = new List<Button>() { posible1, posible2, posible3 };
		adivinanzaPregunta.text = a.adivinanza;
		_respuestactual = a.respuesta;
		ColocarRespuesta(lista, a);

		//Actualizo el nivel en la pantalla
		UpdateTextMesh(adivinanzaNivel, a.numero + "/" + _adivinanza.ContAdivinanzas());



	}

	/*
     * 
     * Metodo que permite cambiar de escena cuando el minijuego
     * ha terminado
     * */
	private void FinJuego()
	{
		VariablesPersonaje.variablesPersonaje.UpdateOrgullo (orgullo);
		VariablesPersonaje.variablesPersonaje.UpdateDiamantes (diamantes);

		
		Application.LoadLevel("MenuMain");
	}



	//Metodo para llamar al Panel de notofocaciones
	//Estos eventos los asigno desde el inspector es otra forma y al final es mas facil
	//Este iria en el boton de rendirse

	/*
	 * Metodo que se asignara al boton siguiente!!!
	 * Supone que el jugador se rinde en este nivel
	 * y se activada el panel de notificaciones
	 * correspondiente a pasar de nivel
	 * 
	 * */
	public void RendirseNivel()
	{
        if (_adivinanzaNivel <= _adivinanza.ContAdivinanzas())
        
            _modalPanel.Elejir("Te rindes?. -1.Diamante. Gana la Chispa.Pasar siguiente nivel",
              null, aceptarButton, cancelarButton, false,helpButton);


        }


        /*
		if (_adivinanzaNivel <= _adivinanza.ContAdivinanzas ()) {

			_modalPanel.Elejir("Rendirse en esta adivinanza?",
			                   null, aceptarButton, cancelarButton, false);
		}
         * */
			
	

	/*
	 * 
	 * Metodo que se asignara al boton ayuda
	 * 
	 * Metodo encargado en mostrar una panel
	 * para el boton ayuda segun sus condiciones
	 * Mostrara si puede o no acceder a una ayuda, en caso
	 * de poder mostrar una ayuda accedera al metodo
	 * para la ayuda
	 * 
	 * */
	public void ChoiceHelp()
	{
		//Si la ayuda en este nivel ya ha sido usada
		if (numAyuda == 0)
		{

			_modalPanel.MostrarMsg("Ya ha usado la ayuda en este Nivel."
			                       ,aceptarButton, cancelarButton,helpButton);
		}
		else//Si aun tengo una ayuda para este nivel
		{
			if (diamantes <= 0)
			{
				_modalPanel.MostrarMsg("No hay Orgullo disponibles."
				                       , aceptarButton, cancelarButton,helpButton);
			}
			else
			{
				if (_adivinanzaNivel <= _adivinanza.ContAdivinanzas())
				{
					/*
					 * En caso de aceptar la ayuda 
					 * Accederemos al metodo que nos da la ayuda
					 * */
					_modalPanel.ChoiseHelp("Una ayuda cuesta -1.Orgullo.Continuar?",
					                       DoHelp, aceptarButton, cancelarButton,helpButton);
				}
			}
		}
	}

	/*
	 * Metodo que nos da una ayuda en el nivel respectivo.
	 * Elimina una de las 2 respuestas falsas
	 * 
	 * */
	private void DoHelp()
	{
		//Lista con botones que contiene las posiblres respuestas
		List<Button> botones = new List<Button>{posible1, posible2, posible3};
		//Obtengo la adivinanza(pregunta) del nivel actual
		Adivinanza a = _adivinanza.Getadivinazas(_adivinanzaNivel.ToString());
		//Creo boton que contiene respuesta correcta
		Button respuestaCorrecta=null;

		/*
		 * Recorro los botones(posibles respuestas)
		 * y guardo el boton que contiene
		 * la respuesta correcta
		 * 
		 * */
		foreach (var b in botones)
		{
			if (b.GetComponentInChildren<Text>().text == a.respuesta)
			{
				respuestaCorrecta = b;
			}
		}

		/*
		 * Recorro los botones(posibles respuestas)
		 * y elimino el primero que no sea
		 * la respuesta correcta
		 * 
		 * */
		foreach (var b in botones)
		{
			if (b != respuestaCorrecta)
			{
				b.gameObject.SetActive(false);
				break;
			}
		}

		//Actualizo variables
		orgullo--;
		ayudas++;
		UpdateTextMesh(orgulloTextMesh,orgullo.ToString());
		UpdateTextMesh(AyudasTextMesh,ayudas.ToString());

		//ayudaActual = true;
		numAyuda--;
		UpdateTextMesh(numAyudaT, numAyuda.ToString());
		//Cerramos panel
		_modalPanel.CerrarPanel();
	}
	
}
