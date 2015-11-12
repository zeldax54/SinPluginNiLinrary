using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.UI;


public class MecanicaController : MonoBehaviour {


	//Objeto controlador de poemas
	private ControladorPoemas _controladorPoema;
	//Variables intentos por nivel

	//Variables Personaje
	private int orgullo = 5;
	private int diamantes = 5;

	//Contador de intentos
	private int _intentos = 3;
	private int numAyuda = 1;
	//Ayuda por nivel


	//TextMesh
	public TextMesh intentosT;
	public TextMesh orgulloT;
	public TextMesh diamantesT;
	public TextMesh ayudaT;
	public TextMesh nivelT;
	//public TextMesh numAyudaT;

	//Objeto de tipo del script ModalPanel
	private ModalPanel _modalPanel;
	//Objeto del panel para mostrar el poema cuando lo resuelva
	public Text textoPoema;

	//Botones para el panel
	public Button Aceptar;
	public Button Cancelar;

	private bool _incdiamantel = true;
	private bool _incorgullo=true;

	void Awake()
	{
		// Le asigno una instancia del script Modal Pane
		_modalPanel = ModalPanel.Instance();
		//Obtenemos el objeto controladorPoema de los componentes Ui
		_controladorPoema = GetComponent<ControladorPoemas>();

		UpdateTextMesh(intentosT, _intentos.ToString());
		UpdateTextMesh(ayudaT, numAyuda.ToString());
		UpdateTextMesh(diamantesT, diamantes.ToString());
		UpdateTextMesh(orgulloT, orgullo.ToString());
		UpdateTextMesh(nivelT, numAyuda.ToString());
		//Actualizo el nivel en la pantalla
		UpdateTextMesh(nivelT,( _controladorPoema.nivelPoema+1) + " / " + _controladorPoema._poemas.Count());

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

	/*
	 * Metodo que se asignara al boton siguiente!!!
	 * Supone que el jugador se rinde en este nivel
	 * y se activada el panel de notificaciones
	 * correspondiente a pasar de nivel
	 * 
	 * */
	public void RendirseNivel()
	{
		//Comprobamos el 
		if (_controladorPoema.nivelPoema < 2)
		{

			_modalPanel.Rendirse("Si te rindes Rebolledo gana 1.Diamante." + "\n" + "¿Quieres pasar al siguiente nivel?", textoPoema, Aceptar, Cancelar, AccionRendirse);
		}
		else
		{
			_modalPanel.ShowPoema("Es el ultimo Poema (nivel)."+ "\n" +  "Desea ir al <color=green>Menu Principal</color> del Juego?", textoPoema, Aceptar, Cancelar, FinJuego);

		}
		
	}

	/*
	 * 
	 * Metodo que reacciona al dar al boton
	 * aceptar del panel sobre la ayuda
	 * 
	 * */
	private void AccionRendirse()
	{
		IncNivel();
		_controladorPoema.SetUI ();


	}

	private void FinJuego()
	{
		Application.LoadLevel("MenuMain");
	}


	/*
	 * Metodo para manejar las posiciones de los objetos
	 * que estan siendo arrastrados en la escena
	 * */
	public void CheckAndSet()
	{

		/*
		 * 
		 * Obtengo una lista TextMesh de los elementos vacio
		 * "_" donde se deben de poner las palabras
		 * en el poema con una consulta Linq para obtener las lineas y ordenarlas
		 * segun el orden en pantalla
		 * */
		List<TextMesh> lineasEnInterfaz =
			FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a => a.name).ToList();


		if (lineasEnInterfaz.Select(l => l.GetComponent<ManejadorLinea>()).Any(m => m.GetActualPuesta() == ""))
		{

			//Los sitios vacios "_" no estan completos
			_modalPanel.ShowPoema("Hay espacios sin completar."+ "\n" +"Rellena todos los huecos", textoPoema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
			return;

		}


		// Si todas estan completas Obtengo el poema actual
		Poema poemaActual = _controladorPoema._poemas[_controladorPoema.nivelPoema];

		//Obtengo las palabras correctas del poema las ordeno por su posicion para asegurar el orden
		List<Palabra> lineasOkPoema = poemaActual.palabrasP.OrderBy(a=>a.posicion).ToList();
		//Bandera para controlar si se resolvieron todos los poemas
		bool bandera = true;
		//Guardar texto del poema para mostrarlo
		string poemaTexto = "";

		/*
		 * Recorro todos los sitios vacios "_"
		 * 
		 * */
		for (int i = 0; i < lineasEnInterfaz.Count; i++)
		{
			//Obtengo el script manejador para cada linea
			ManejadorLinea manejador = lineasEnInterfaz[i].GetComponent<ManejadorLinea>();

			//Voy guardando las lineas del poema
			poemaTexto += manejador.PintarPalabra(lineasEnInterfaz[i].text, "#008000ff") + "\n";

			//Comprobamos si hay una palabra puesta en el TextMesh
			if (manejador.ContienePalabraDeResp())
			{
				//Si la palabra que tiene puesta coincide con la que le toca en el poema
				if (manejador.GetActualPuesta() == poemaActual.GetPalabradeLinea(lineasOkPoema, i).palabra)
				{
					//Palabra correcta, se asigna color acierto, verde en este caso
					manejador.SetCorrectWord("#008000ff"); 
				}
				else
				{
					//Actualizamos variables
					bandera = false;
					_intentos--;
					_incorgullo = false;

					//Actualizamos TextMesh de la escena
					SetIntentos(_intentos);

					//Si ya ha agotado sus intentos significa que fallo el poema hay que mostrar el otro opema y resetear los intentos
					/*
					 * Si hay 0 intentos
					 * significa que ha fallado en el poema (nivel)
					 * Mostramos el siguiente poema (nivel)
					 * y reiniciamos variable intentos 
					 * */
					if (_intentos == 0)
					{
						
						if (_controladorPoema.nivelPoema <= 2)
						{
						
							_intentos = 3;
							SetIntentos(_intentos);

							//Aumentamos el nivel (poema)
							//_controladorPoema.nivelPoema++;
							IncNivel();

							//Si utilizo la ayuda la reseteo para el siguiente poema
							CheckAyuda();
							//Le quito un diamante fallo el poema
							DecDiamantes();

							_modalPanel.ShowPoema("Tienes Tres errores. Poema Fallido."+ "\n" +"Has perdido este nivel", textoPoema, Aceptar, Cancelar,_controladorPoema.SetUI);
							
						}
					}
				}
			}
			
		}


		/*
		 * Si se resolvieron todos
		 * los poemas, tratamos los casos
		 * 
		 * 
		 * */
		if (bandera)
		{
			if (_incdiamantel)
			{
				if (_controladorPoema.nivelPoema == 2)
					_incdiamantel = false;
				else
				{
					diamantes++;
					UpdateTextMesh(diamantesT, diamantes.ToString());
				}
				
			}

			if (_controladorPoema.nivelPoema < 2)
			{
				_intentos = 3;
				SetIntentos(_intentos);

				//Preparo el siguiente nivel (poema)
				//_controladorPoema.nivelPoema++;
				IncNivel();
				CheckAyuda();
				_modalPanel.ShowPoema(poemaTexto, textoPoema, Aceptar, Cancelar, _controladorPoema.SetUI);
			}
			else
			{
				if (_incorgullo)
				{
					IncOrgullo();
					_incorgullo = false;
				}

				_intentos = 0;
				SetIntentos(_intentos);
				_modalPanel.ShowPoema(poemaTexto, textoPoema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
			}
			
		}
	}

	//OJO ESTE TIENE QUE ACTUALIAR TODAS VARIABLES PILAS!
	private void SetIntentos(int intentos)
	{
		_intentos = intentos;
		UpdateTextMesh(intentosT, _intentos.ToString());
	}

	/*
	 * Metodo que actualiza las variables
		* correspondientes al numAyudas
	 * */
	private void CheckAyuda()
	{
		if (numAyuda == 0)
		{
			numAyuda++;
			UpdateTextMesh(ayudaT, numAyuda.ToString());
		}
		
	}



	/*
	 * Metodo que actualiza las variables
		* correspondientes al cambiar de nivel
	 * */
	private void IncNivel()
	{
		_controladorPoema.nivelPoema++;
		UpdateTextMesh(nivelT,( _controladorPoema.nivelPoema+1) + " / " + _controladorPoema._poemas.Count());

		//Decrementamos los diamantes
		DecDiamantes ();
	}

	/*
	 * Metodo que actualiza las variables
		* correspondientes al aumentar Diamantes
	 * */
	private void DecDiamantes()
	{
		diamantes--;
		UpdateTextMesh(diamantesT,diamantes.ToString());
	}

	/*
	 * Metodo que actualiza las variables
		* correspondientes al aumentar Orgullo
	 * */
	private void IncOrgullo()
	{
		orgullo++;
		UpdateTextMesh(orgulloT,orgullo.ToString());
	}

	/*
	 * Metodo que informa sobre el panel
	 * de ayuda
	 * */
	public void RequestHelp()
	{

		Debug.Log(" Help");

		if(numAyuda>0)
			_modalPanel.Rendirse("Una Ayuda cuesta -1.Diamante" + "\n" + "Deseas continuar?", textoPoema, Aceptar, Cancelar, Help);
		else
		{
			_modalPanel.ShowPoema("No quedan Ayudas para este Poema (nivel)" + "\n" + "Intenta resolverlo!", textoPoema, Aceptar, Cancelar, _modalPanel.CerrarPanel);
		}
	}


	
	private void Help()
	{
		Poema poemaActual = _controladorPoema._poemas[_controladorPoema.nivelPoema];

		Debug.Log("Entre a Help");


		/*
		 * 
		 * Obtengo una lista TextMesh de los elementos vacio
		 * "_" donde se deben de poner las palabras
		 * en el poema con una consulta Linq para obtener las lineas y ordenarlas
		 * segun el orden en pantalla
		 * */
		List<TextMesh> lineas = FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a=>a.name).ToList();

		int cont = 0;//Para saber en que linea estoy

		foreach (var l in lineas)
		{
			ManejadorLinea m = l.GetComponent<ManejadorLinea>();

			Debug.Log("var l in lineas");

			if (m.LineaDisponible() ||m.GetPalabraInsegura())
			{
				Debug.Log("Entro IF () ()");

				//Recorro la lista de palabras del poema donde estoy
				foreach (var p in poemaActual.palabrasP)
				{
					Debug.Log("var p in poemaActual.palabrasP)");

					//Si la popsicion de la palabra es de la linea donde estoy
					if (p.posicion == cont) 
					{
						Debug.Log("Entro IF");

						ControladorPalabra cp ;
						GameObject o;

						//Si hay una palabra puesta la cambio o la marco como correcta
						if (m.GetActualPuesta() != "")
						{
							//Aguardar la que esta puesta antes de cambiarla
							o = FindObjectsOfType<TextMesh>().First(a => a.text ==m.GetActualPuesta()).gameObject;
							Debug.Log(o.GetComponent<TextMesh>().text);

							//Cambia la palabra y marco la correcta
							m.FindandReplaceRedWord(p.palabra);
							m.SetCorrectWord("#008000ff");

							//Quitar la palara correcta de la lista y bajar la que habia
							GameObject correcta = FindObjectsOfType<TextMesh>().First(a => a.text == p.palabra).gameObject;
							 cp = o.GetComponent<ControladorPalabra>();

							//Colocamos las palabra a sus posiciones 
							cp.SubirPalabra(correcta);
						

							cp.BajarPalabra(o.GetComponent<TextMesh>().text);//Bajando la que estaba puesta

						}
						else//Si no hay la pongo
						{

							Debug.Log("Entro ELSE");

							//m.PutText(p.palabra,m.FindFirst_());
							m.Remove_();
							m.SetCorrectWord("#008000ff");
							//Desactivando la palabra para que no la pueda seleccionar mas
							o = FindObjectsOfType<TextMesh>().First(a => a.text == p.palabra).gameObject;

							cp = o.GetComponent<ControladorPalabra>();
							cp.SubirPalabra(o);
						}

						Debug.Log("Decremento Ayuda");

						numAyuda--;
						UpdateTextMesh(ayudaT, numAyuda.ToString());
						DecDiamantes();

						break;
					}
				}
				break;
			}
			cont++;
		}
	}

}
