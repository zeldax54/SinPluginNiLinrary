using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class ControladorPoemas : MonoBehaviour {


	//Este prefab esta creado en la carpeta prefab
	public TextMesh prefabLinea;

	//Posibles palabras para rellenar "_"
	public TextMesh palabra1;
	public TextMesh palabra2;
	public TextMesh palabra3;
	public TextMesh palabra4; 
	public TextMesh palabra5;
	//Posicion donde voy a empesar a dibujar los TextMesh
	public GameObject lineStarterMarcador;
	//Decremento eje y (linea bajo linea del poema)
	private float decrementaY=1.5f;

	//Lista de poemas
	public List<Poema> _poemas;
	//Para traer la lista del scritp Poema
	private readonly Poema poema=new Poema();
	//En que nivel(poema) y acceder desde el script de la mecanicaController
	public int nivelPoema = 0;
	//Corresponde al hueco para completar con una posible palabra
	private const string raya = "_________";

	//Guardo en un objeto donde se hace click
	private GameObject _target;
	//Para saber si estoy arrastrando no 
	private bool _mouseState;
	//Posicion original desde donde estaba el Objeto que arrastro
	private Vector3 _originalPosition;
	private Vector3 _screenSpace;
	private Vector3 _offset;


	private void Awake()
	{
		//Obtenemos la lista de poemas
		_poemas = poema.InicializarListaPoemas ();
		//Inicializar la Ui por primera vez
		SetUI();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	private void Update()
	{
		//Si detectamos un click con el mouse
		if (Input.GetMouseButtonDown(0))
		{
			//Para detectar donde impacta un ray(para saber donde ir)
			RaycastHit hitInfo;
			//Obtener el objeto donde dio el rayo que se lanza en el metodo GetClickedObject
			_target = GetClickedObject(out hitInfo);

		
			/*
			 * Si obtenemos un objeto donde haya impactado
			 * el ray
			 * 
			 * */
			if (_target != null)
			{
				//Si el objeto fue en un TextMesh (el de las palabras)
				if (hitInfo.collider.name.Contains("Palabra")) 
					//EN QUE OBJETO DE LA ESCENA TIENE PUESTO "Palabra"
				{
					//Guardar la posicion Original del objeto
					_originalPosition = _target.transform.position;

					_mouseState = true;
					_screenSpace = Camera.main.WorldToScreenPoint(_target.transform.position);
					_offset = _target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenSpace.z));
				}
				
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			//Si me encuentro arrastrando algo en ese momento
			if (_target != null)
			{
				//Colocamos al objeto a su posicion inicial
				_target.transform.position = _originalPosition;
			}
			//Reiniciamos el estado del raton
			_mouseState = false;
		}

		//Si el raton esta en movimiento
		if (_mouseState)
		{
			//mantener chequeada la posicion del mouse
			var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenSpace.z);
			//Convertir de World posicion a Screen posicion
			var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + _offset;
			//Arastrar
			_target.transform.position = curPosition;
		}
	}
	

	/*
	 * //Metodo toma los 5 TextMesh y asigna las palabras
	 * 
	 * Metodo que inicializa las posibles palabras
	 * que se utilizaran para rellenar los huecos 
	 * en el texto del poema
	 * Toma los respectivos TextMesh y asigna las palabras
	 * 
	 * p = objeto poema que contiene el poema, las palabras validas
	 * y palabras falsas
	 * */
	private void InicializarPalabrasPosibles(Poema p)
	{
		//Lista con posibles palabras en la escena
		List<TextMesh> palabrasUi = new List<TextMesh>()
		{palabra1, palabra2, palabra3, palabra4, palabra5};
		//Lista con palabras verdadera y falsas segun el poema(nivel)
		List<Palabra> allPalabras = p.palabrasP.Concat(p.falsaspalabras).ToList();

		//Contador para iterar por la lista de palabras que forme
		int cont = 0;

		while (palabrasUi.Count!=0)//Cilco par asignar random las palabras del poema a los mesh botones
		{

			/*
			 * Posicion random en la lista de palabras de las escena
			 * y obtengo Textmesh correspondiente
			 * Asignamos al texto la palabra (segun cont )de mi lista 
			 * de palabras (
			 * */
			int pos = Random.Range(0, palabrasUi.Count);
			TextMesh t = palabrasUi[pos];
			t.text = allPalabras[cont].palabra;

			//Si el objeto t no tiene collider para interaccionar con el trigger
			if (t.gameObject.GetComponent<BoxCollider>()==null)
			{
				//Asignar un collider al objeto para poder arrastrarlo en la Interfaz
				t.gameObject.AddComponent<BoxCollider>();
				//Se dispara cuando choca con otro collider 
				t.GetComponent<Collider>().isTrigger = true;
			}

			//Lo pongo en la x original por si la palabra se ha movido
			ActivarMesh(t);

			//Elimino el textMesh de la lista para no repetirlo en la sig Iteracion
			palabrasUi.RemoveAt(pos);
			//Incremento cont para pasar el siguiente elemento en mi lista de palabras(allPalabras)
			cont++;
			
		}
	}

	/*
	 * Metodo que activa TextMesh de la escena,
	 * por si no esta en la UI
	 * 
	 * palabra = TextMesh que se activara
	 * */
	private void ActivarMesh(TextMesh palabra)
	{
		/*
		 * Obtenemos su transform
		 * y reubicamos
		 * */
		Transform p = palabra.transform;//Transform de la palabra
		palabra.transform.position = new Vector3(5.20f, p.position.y, p.position.z);
	}


	/*
	 * Metodo que convierte un * 
	 * en una linea _______
	 * Esto se debe a que el poema tiene 
	 * representado el elemento vacio (Donde hay que poner la palabra)
	 * con un *. Remasterizamos y ponemos la raya para
	 * que se vea en la Ui de la escena
	 * 
	 * text = linea de texto donde se hace la conversion "*" por "_____"
	 * */
	private string Remaster(string text) 
	{
		string lineaPoema = "";

		/*
		 * Recorremos cada caracter del texto de la 
		 * linea del poema.
		 * Si encontramos * hacemos conversion
		 * C.C = dejamos el caracter
		 * */
		foreach (char c in text)
		{
			if (c == '*')
			{
				lineaPoema += raya;
			}
			else
			{
				lineaPoema += c;
			}
		}
		return lineaPoema;
	}

	/*
	 * Obtiene las lineas del poema( objeto de otro script)
	 * y se lo asigna a las lineas que apareceran en Ui de la
	 * escena.
	 * Coje el contenido del scrip del poema para que aparezca en Ui
	 * Por cada linea del poema se remasteriza para que aparezca
	 * el contenido previsto en la Ui
	 * 
	 * lineasPoema = contiene las lineas del poema por defecto
	 * 
	 * return false si no hay linas de poema en caso contrario True
	 * */
	private bool RemasterLineasPoema(List<string>lineasPoema )
	{
	
		bool bandera = false;
		List<TextMesh> lineasUi = FindObjectsOfType<TextMesh>().Where(a => a.name.Contains("Linea")).OrderBy(a => a.name).ToList();

		//Si no hay lineas para mostrar en Ui, false no hay nada que limpiar
		if (!lineasUi.Any())
			return false;//Si no hay lineas las creo en el otro metodInicializarPalabrasPosibleso

		/*
		 * Recorremos cada linea del poema que aparecera
		 * en Ui y asignamos el contenido correspondiente
		 * a lineasPoema(contiene la linea del poema NO
		 * EL DE LA UI!) y  remasterizo su contenido
		 * 
		 * */
		for (int i = 0; i < lineasUi.Count; i++)
		{
			lineasUi[i].text = Remaster(lineasPoema[i]);
			bandera = true;
		}
		
		return bandera;
	}

	/*
	 * 
	 * Metodo que inicializa los elementos
	 * necesario en la Ui en la escena
	 * 
	 * */
	public void SetUI()
	{
		
		//Inicializar las lineas del Poema
		InicializarPalabrasPosibles(_poemas[nivelPoema]);

		//Cuenta el numero de linea para asignar el nombre a las lineas
		int cont = 1;
		float decrementa = 0;

		if (!RemasterLineasPoema(_poemas[nivelPoema].textoPoemaLineas))
		{
			/*
			 * Por cada linea del poema se instancia el prefab  
			 * con el texto de la linea.
			 * Y luego se va bajando el eje Y para que la siguiente
			 * linea aparezca abajo de la anterior
			 * */
			foreach (var linea in _poemas[nivelPoema].textoPoemaLineas)
			{
				
				prefabLinea.name = "Linea" + cont;
				prefabLinea.text = Remaster(linea);
	
				/*
				 * Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
				 * */
				Instantiate(prefabLinea,
				            new Vector3(lineStarterMarcador.transform.position.x, lineStarterMarcador.transform.position.y - decrementa,
				            0f), Quaternion.identity);
				//Incremento contador y Y
				cont++;
				decrementa += decrementaY;
				
			}
		}
		
		
		
	}

	/*
	 * Metodo encargado de obtener un objeto
	 * sobre el cual el hit haya colisionado
	 * 
	 * */
	GameObject GetClickedObject(out RaycastHit hit)//Obtener el objeto donde el rayo dio 
	{
		//Inicializo el objeto
		GameObject target = null;

		/*
             * Un Ray es un rayo imaginario que lanzo desde la camara hacia la
             * profundidad y si impacta en un cuadro entonces actuo
             */
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Si detactamos que el rayo impacta 
		if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
		{
			//Obtenemos el objeto donde colisiona el ray
			target = hit.collider.gameObject;
		}
		
		return target;
	}

}
