using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/*
Script que utilizara un panel para generar avisos segun las acciones
se se hagan mediante botones ayuda,pista,pasar nivel

 */
public class ModalPanel : MonoBehaviour {

	
	//Texto del panel (aviso) que aparecera segun accion
	public Text TextUI;
	//Objeto estatico para tener una instancia del panel 
    private static ModalPanel mPanel;
	//Para saber cuando el panel(aviso) esta activo
	private bool activo;
	//Sprites(imagenes) que aparecen en el panel
	public Sprite ok;
	public Sprite si;

    //Objeto panel de UI(aviso para una accion)
    public GameObject modalPanelO ;

	/*
	 * Constructor de instancias, para que otros
	 * scripts puedan acceder a este.
	 */
	public static ModalPanel Instance() 
	{
		//Si no es creado el objeto estatico
        if (!mPanel)
		{
            mPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
			//Si no ha sido creado, aviso.
            if (!mPanel)
				Debug.LogError("No hay paneles en tu escena.");
		}
		
		return mPanel;
	}



	/*
	Metodo para activar el panel(aviso)
	pregunta = texto que saldra en el panel
	yesEvent= metodo (evento a ejecutar ) si se acepta
	aceptar= Boton para accion aceptar
	cancelar= Boton para accion cancelar
	bandera = booleano para modificar paneles informativos

	 */
	public void Elejir(string pregunta, UnityAction yesEvent, Button  aceptar,Button cancelar, bool bandera, Button help = null)
	{
		
		//Activo el panel (por defecto desactivado)
		modalPanelO.SetActive(true);

		//Genero el texto que aparecera en el aviso
		TextUI.text = pregunta;


        if (help != null && help.IsActive())
            help.gameObject.SetActive(false);


		/*
		 * Remuevo y agrego los oyentes del boton cancelar.
		 *LLama a un evento que cierra el panel
		 */
		cancelar.onClick.RemoveAllListeners();
		cancelar.onClick.AddListener(CerrarPanel);

		/*
		 * Remuevo y agrego los oyentes del boton cancelar.
		 * LLama al evento de tipo(UnityAction) que sera
		 * asignador a otro metodo en el script que le haya pasado
		 * dicho evento.Es decir yesEvent=Metodo en el otro script
		
		 */
		aceptar.onClick.RemoveAllListeners();


		if (yesEvent != null) {

			aceptar.onClick.RemoveAllListeners();
			aceptar.onClick.AddListener(yesEvent);
		}

		//Cambio la imagen del boton aceptar
		aceptar.gameObject.SetActive(true);
		aceptar.GetComponentInChildren<Image>().sprite = si;
	

		if (bandera) //Esto es para los paneles informativos dejarle un solo boton
		{
		

			cancelar.gameObject.SetActive(false);
			aceptar.GetComponentInChildren<Image>().sprite = ok;

			
		}
		else
		{
			
			cancelar.gameObject.SetActive(true);
		}
		
		activo = true;
	}


	/*
	 * Metodo encargado de enseñar un mensaje
	 * en el panel.
	 * 
	 * pregunta = texto que saldra en el panel
	 * aceptar= Boton para accion aceptar(normal)
	 * cancelar= Boton para accion cancelar
	 * help = Boton para la accion aceptar( para ayuda)
	 * 
	 * */
	public void MostrarMsg(string texto, Button aceptar, Button cancelar, Button help)
	{
		//Activo el panel porque inicialmente tiene que estar desactivado para que no se muestre en la escena
		modalPanelO.SetActive(true);
		//Le asigno al texto correspondiente
		TextUI.text = texto;

		//Desactivo el boton cancelar/aceptar(normal)
		cancelar.gameObject.SetActive(false);
		aceptar.gameObject.SetActive(false);

		//Actualizo help y cambio imagen ok(cierra panel)
		help.gameObject.SetActive(true);
		help.onClick.RemoveAllListeners();
		help.onClick.AddListener(CerrarPanel);

		help.GetComponentInChildren<Image>().sprite = ok;
		
	}

	/*
	 * 
	 * Metodo encargado de tratar el panel cuando
	 * se pida una ayuda
	 * texto: texto que aparecera en el panel
	 * yesEvent= metodo (evento a ejecutar ) si se acepta
	 * aceptar= Boton para accion aceptar(normal) 
	 * cancelar= Boton para accion cancelar
	 * help = Boton para la accion aceptar( para ayuda)
	 * */
	public void ChoiseHelp(string texto, UnityAction yesEvent, Button  aceptar,Button cancelar,Button help)
	{
		//Activo el panel porque inicialmente tiene que estar desactivado para que no se muestre en la escena
		modalPanelO.SetActive(true);
		TextUI.text = texto;



		//Desactivo boton aceptar(el normal), por si se quedo activo
		aceptar.gameObject.SetActive(false);

		//Actualizo cancelar
		cancelar.gameObject.SetActive(true);
		cancelar.onClick.RemoveAllListeners();
		cancelar.onClick.AddListener(CerrarPanel);

		//Actualizo el aceptar( de ayuda)
		help.gameObject.SetActive(true);
		help.onClick.RemoveAllListeners();
		help.onClick.AddListener(yesEvent);
		activo = true;
	}

	/*
	Metodo que cierra el panel
	 */
	public void CerrarPanel()
	{
		//Esta desactivado
		activo = false;
		//Cerramos panel
        modalPanelO.SetActive(false);
	}


	public bool IsActivo()
	{
		return activo;
	}


	/*
	Metodo para activar el panel(aviso)
	cuando se haya acertado en algun nivel
	pregunta = texto que saldra en el panel
	yesEvent= metodo (evento a ejecutar ) si se acepta
	aceptar= Boton para accion aceptar
	cancelar= Boton para accion cancelar
	bandera = booleano para modificar paneles informativos

	 */
	public void AvisoAcierto(string pregunta, UnityAction yesEvent, Button  aceptar,Button cancelar, Button help)
	{
		
		//Activo el panel (por defecto desactivado)
		modalPanelO.SetActive(true);
		
		//Genero el texto que aparecera en el aviso
		TextUI.text = pregunta;

		//Desactivo cancelar
		cancelar.gameObject.SetActive(false);
		aceptar.gameObject.SetActive(false);

		//Activo botn help ( que hara de ok para este panel)

		help.gameObject.SetActive(true);
		/*
		 * Remuevo y agrego los oyentes del boton cancelar.
		 * LLama al evento de tipo(UnityAction) que sera
		 * asignador a otro metodo en el script que le haya pasado
		 * dicho evento.Es decir yesEvent=Metodo en el otro script
		
		 */
		aceptar.onClick.RemoveAllListeners();
		if (yesEvent != null) {
			
			help.onClick.RemoveAllListeners();
			help.onClick.AddListener(yesEvent);
		}
		
		//Cambio la imagen del boton aceptar
		help.GetComponentInChildren<Image>().sprite = ok;

		

		activo = true;
	}


	//Metodo del panel para mostrar un poema
	//Sirve para mostrar una informacion tambien sin que sea un poema
	public void ShowPoema(string poema,Text poemaUi, Button aceptar, Button cancelar,UnityAction accion)
	{

		modalPanelO.gameObject.SetActive(true);
		cancelar.gameObject.SetActive(false);
		aceptar.onClick.RemoveAllListeners();
		aceptar.onClick.AddListener(accion);

		//Cambio la imagen del boton aceptar
		aceptar.GetComponentInChildren<Image>().sprite = ok;

		aceptar.onClick.AddListener(CerrarPanel);
		poemaUi.text = poema;
		
	}


	public void Rendirse(string poema, Text poemaUi, Button aceptar, Button cancelar, UnityAction accion)
	{
		modalPanelO.gameObject.SetActive(true);
		cancelar.gameObject.SetActive(true);
		aceptar.onClick.RemoveAllListeners();
		cancelar.onClick.RemoveAllListeners();
		aceptar.onClick.AddListener(accion);
		//Cambio la imagen del boton aceptar
		aceptar.GetComponentInChildren<Image>().sprite = si;
		aceptar.onClick.AddListener(CerrarPanel);
		cancelar.onClick.AddListener(CerrarPanel);
		poemaUi.text = poema;
		
	}





}
