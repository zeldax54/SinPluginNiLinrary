using UnityEngine;
using System.Collections;
//Espacio de nombres,  interfaces y clases que definen colecciones genéricas
using System.Collections.Generic;
//Libreria para hacer consulta 
using System.Linq;
using UnityEngine.UI;

public class Generador : MonoBehaviour
{

    //Cuadro que va a dibujar en la respuesta
    public GameObject CuadroResp;
    //Cuadro que va a dibujar en las letras
    public GameObject CuadroLetra;
    //Sprite para el grafico dento del cuadro de respuesta y en las letras
    public SpriteRenderer SpriteResp;
    public SpriteRenderer SpriteLetra;
    //TextMesh dentro de los cuadros y en las letras
    public TextMesh TextorResp;
    public TextMesh TextoLetras;
    //Generadores de cuadrados y letras
    public GameObject GeneradorIzq;
    public GameObject GeneradorDer;
    public GameObject GeneradorLetras;


    /*
     * Variables para ubicar las letras (parte inferior) en dos filas una abajo de otra 
     * 
     */
    private float _posinigen;
    private float _posinoy;

    //Para contar las letras generadas (izquierda y derecha)
    private int _contizq;
    private int _contder;
    //Letras totales
    private int _contletras = 0;
    // Atributo de la imagen que se va a mostrar
    private Image _spriteContainer;


    /*
     * Variables String para guardar las palabas
     * que quiero generar letras, contarlas ubicarlas y demas.
     */
    private string _palabra;
    private string _palabrarespizq;
    private string _palabrarespder;
    private string _palabraletras;

    // Sumatoria para incrementar los espacios entre letras
    private float _incrementaizq = 0f;
    // Sumatoria para incrementar los espacios entre letras
    private float _incrementader = 0f;
    private float __incrementaletras = 0f;

    //Creo las variables que me guardaran los datos 
    //Guardo los datos en arreglos , respuesta primero , letras despues y luego  la pista del segundo tipo.
    /*
     Array que contiene los datos del primer nivel
     *Respuesta,Letras aleatorias, y pista (no ayuda)
     */
    private object[] primera = { "CALDERON", "HENGPRCOPGALDW", "Escritor de la obra " };
    private object[] segunda = { "CERVANTES", "REDCEVXNTSILAC", "Escritor del Quijote" };
    private object[] tercera = { "QUEVEDO", "OGLIQCUYEWEVND", "Escritor literario del Siglo de Oro" };

    private List<object[]> ListaPalabras = new List<object[]>();
    //Para saber en que nivel(array) estoy
    private object[] arregloactual;
    //Para saber en que numero de nivel
    private int contpantallas = 0;



    //Estas son las variables del jugador(defecto)
    private int orgullo ;
    private int diamantesl ;

    //Variables internas(las que se va pasar)
    private int fallos = 0;
    private int aciertos = 0;
    private int siguientes = 0;
    private int ayudas = 0;

	//Para saber si se ha usado una ayuda en este nivel
	private int numAyuda = 1;

    /*
     Text para actualizar las variables en la pantalla, aqui
     * asigo el valor no el indicador!
     */
    private TextMesh OrgulloTextMesh;
    private TextMesh DiamantesTextMesh;
    private TextMesh FallosTextMesh;
    private TextMesh AciertosTextMesh;
    private TextMesh AyudasTextMesh;
    private TextMesh SiguientesTextMesh;
    private TextMesh cuadernilloTextMesh;//Este es para la 2da pista
	private TextMesh numAyudaT;
	private TextMesh nivelT;



    //Parte de las pistas
    //Atributos que se asignan desde el inspector

    private ModalPanel _modalPanel;
    //Boton para ayuda(da una letra)
    public Button ayudaButton;
    //Boton para pistas(pista sobre personaje)
    public Button cuadernillo;
    //Boton para pasar de nivel
    public Button SiguientButton;


    /*
     * Botones para aceptar o cancelar una accion
     * Como pedir ayuda,pista o pasar nivel
     */
    public Button aceptarButton;
    public Button cancelarButton;
    
    /*
     * Objeto para saber la posicion del eje y de una letra de la parte
     * inferior.Esta letra se podra subir o bajar cuando le doy click
     */
    public GameObject Marcador;
    /*
     * 
     * Riendose para cuando paso de nivel sin resolver 
     * vecesrendidas para saber cuantas veces me he rendido en el minijuego.
     */
    private bool rindiendose;

    //eSTA ES VARIABLE INTERNA SIGUIENTES!! VER USO
    private int vecesrendidas;


    //Lista de objetos para guardar objetos temporalmente 
    //CUANDO SELECCIONO UN CUADRO DE ABAJO Y PONGO ARRIBA (GUARDO EL OBJETO POR SI TENGO QUE VOLVER ATRAS)
    public List<GameObject> EstadopLetras;

   

	private VariablesPersonaje variablesPersonaje;


    //Iniciaiza atributos al principio del todo
    private void Awake()
    {
		//Obtengo las variables generales del personaje
		diamantesl = VariablesPersonaje.variablesPersonaje.getDiamantes ();
		orgullo = VariablesPersonaje.variablesPersonaje.getOrgullo ();


        /*
         Agrego a la lista las palabras que se va adivinar
         * (niveles a superar)
         */
        ListaPalabras.Add(primera);
        ListaPalabras.Add(segunda);
        ListaPalabras.Add(tercera);


        /*
         * 
         * 
         * 
         * Devuelve objeto del tipo TextMesh de las atributos hijos
         * del componente "Variables"
         * Para poder modificar el TextMesh cuando sea necesario
         * Expresiones de LinQ de .Net para hacer consultas mas rapidas
         * "a.name" hace referencia al parametro nombre del objeto TextMesh
         * Ya que los TextMesh son privados
         * 
         */
        OrgulloTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "OrgulloValor");
        DiamantesTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "DiamantesValor");
        FallosTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "FallosValor");
        AciertosTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "AciertosValor");
        AyudasTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "AyudaValor");
        SiguientesTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "PasarValor");
        cuadernilloTextMesh = FindObjectsOfType<TextMesh>().First(a => a.name == "Cuadernillo");
		numAyudaT= FindObjectsOfType<TextMesh>().First(a => a.name == "NumAyuda");
		nivelT= FindObjectsOfType<TextMesh>().First(a => a.name == "Niveles");

        /*
         * Actualizo las variables TextMesh que tienen que modificarse
         * y verse en pantalla a lo largo del minijuego.
         */
        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
        UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());
        UpdateTextMesh(FallosTextMesh, fallos.ToString());
        UpdateTextMesh(AciertosTextMesh, aciertos.ToString());
        UpdateTextMesh(AyudasTextMesh, ayudas.ToString());
        UpdateTextMesh(SiguientesTextMesh, siguientes.ToString());
		UpdateTextMesh(numAyudaT, numAyuda.ToString());
		//ACtualizo nivel actual
		UpdateTextMesh(nivelT,(contpantallas+1) +" / "+ (ListaPalabras.Count()));
		
        /*
         * Genero los elementos necesarios para el primer 
         * nivel (inicio minijuego)
         */
        PrepararGenerar(ListaPalabras[contpantallas], contpantallas);
        //Actualizo el array para saber en que nivel estoy
        arregloactual = ListaPalabras[contpantallas];

        //Remuevo/asigno oyente al boton Siguiente
        SiguientButton.onClick.RemoveAllListeners();
        SiguientButton.onClick.AddListener(PasarNivel);


        //Parte de las Pistas
        /*
         * Asigno una instancia del script ModalPanel
         * para poder acceder a sus metodos
         */
        _modalPanel = ModalPanel.Instance();


        //Remuevo/asigno oyente al boton Ayuda
        ayudaButton.onClick.RemoveAllListeners();
        ayudaButton.onClick.AddListener(LevantarPanel);

        //Remuevo/asigno oyente al boton cuadernillo
        cuadernillo.onClick.RemoveAllListeners();
        cuadernillo.onClick.AddListener(Pistas);


    }


    /*
     * Update para detectar lo que esta pasando
     * cada fotograma en el minijuego
     */
    void Update()
    {
        //Si hay click y el panel no esta activo
        if (Input.GetMouseButton(0) && !_modalPanel.IsActivo())
        {
            
            /*
             * Un Ray es un rayo imaginario que lanzo desde la camara hacia la
             * profundidad y si impacta en un cuadro entonces actuo
             */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Para detectar donde impacta el rayo(para saber donde ir)
            RaycastHit hit = new RaycastHit();

            //Si detactamos que el rayo impacta 
            if (Physics.Raycast(ray, out hit))
            {
                //Obtengo el objeto donde impacto el ray por el nombre del collider del hit
                GameObject o = GameObject.Find(hit.collider.name);

                /*
                 * Si el nombre contine ! sabemos que es un cuadro inferior
                 * (posibles respuestas)
                 */
                if (o.name.Contains("!"))
                {

                    //Textmesh de la letra (cuadros posible respuesta)
                    TextMesh tletras = o.GetComponentInChildren<TextMesh>();
                    //Obtengo los cuadros de la respuesta para saber cual no tiene letra puesta
                    var resp = GetRespObjs();

                    /*
                     * Recorro cada cuadro de la respuesta para comprobar
                     * si estan llenos. Si alguno esta en blanco asigno la letra
                     * correspondiente y espero al sigiente
                     * click para volver a comprobar
                     */
                    foreach (var cuadro in resp)//Por cada cuadro en la respuesta 
                    {
                        //Obtengo el TextMesh del cuadro de la respuesta
                        var tresp = cuadro.GetComponentInChildren<TextMesh>();

                        if (tresp.text == "")//Si esta en blanco
                        {
                            //Asigno la letra(posible respuesta)al cuadro que clickeo(respuesta)
                            tresp.text = tletras.text;


                            /*
                             * Cambio posicion del cuadro que clickeo (letra posible respuesta) aumentado
                             * el eje y para que desaparezca, ya que ha sido puesta en la solucion
                             */
                            o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 20f,
                                                               o.transform.position.z);

                            /*
                             * Para que no se vea en la escena, guardo el objeto(el que acabo de poner)
                             * para tener constancia que existe y saber que esta en la nuevo eje y
                             */
                            EstadopLetras.Add(o);

                            /*
                             * Obtengo la respuesta al nivel actual
                             * Compruebo el estado de la respuesta y hagos
                             * los respectivos cambios
                             * Y salgo hasta el siguiente click
                             */
                            var actual = GetActual(GetRespObjs());
                            CheckandSet(actual, arregloactual[0].ToString());
                            break;
                        }
                    }
                }
                //Si el rayo impacta en un cuadro de los de la respuesta

                /*
                 * Si el nombre contine * sabemos que es un cuadro superior
                 * (en la respuesta)
                 */
                if (o.name.Contains("*"))
                {

                    //Obtengo el texto actual que esta puesto en la respuesta
                    var rLetras = o.GetComponentInChildren<TextMesh>();
                    //Es6tado de las letras que estan arriba o sea las que estan asignadas
                    /*
                     * Recupero las letras de la posible respuesta
                     * que ha sido asignadas en la respuesta(cuadros superiores)
                     * y se han guardado su estado
                     */
                    var letrasAsig = GetLetras();

                    /*
                     * Recorro las letras que han sido asignadas
                     */
                    foreach (var letraA in letrasAsig)
                    {
                        /*
                         *Obtengo el TextMesh de la letra que toque
                         *segun el recorrido. Si hay una letra que no es igual
                         *a la letra que clickea (cuadro respuesta)
                         *Compruebo la siguiente. Hasta encontrar una igual
                         */
                        var tresp = letraA.GetComponentInChildren<TextMesh>();
                        if (tresp.text != rLetras.text)
                            continue;


                        /*
                         * Asigno la letra que esta en respuesta 
                         * al cuadrado inferior correspondiente
                         * Y bajo la letra de la respuesta 
                         * a su posicion original y actualizo 
                         * en los componentes que afecta
                         */
                        tresp.text = rLetras.text;
                        ArribaAabajo(tresp.text);
                        EstadopLetras.Remove(letraA);
                        //Borro la letra del cuadro de respuesta
                        rLetras.text = "";
                        break;
                    }
                }
            }

        }


    }

    /*
     * Recorro las letras que han sido asignadas en la respuesta
     * hasta encontrar la letra que tiene que volver a su posicion original
     * es decir a la posicion en eje y correspondiente a los cuadros inferiores
     * (letras para la posible respuesta )
     * letra: letra que vamos a bajar a su posicion original( eje y )
     */
    private void ArribaAabajo(string letra)
    {
        foreach (var o in EstadopLetras)
        {
            TextMesh tresp = o.GetComponentsInChildren<TextMesh>().First();
            if (tresp.text.Contains(letra))
            {
                o.transform.position = new Vector3(o.transform.position.x, (o.transform.position.y - 20f),
                                                   o.transform.position.z);
                break;
            }
        }
    }




    /*
     * Estado de las letras de la posible respuesta (cuadrados inferiores) que
     * han sido asignadas a la respuesta(cuadros superiores)
     * 
     */
    private IEnumerable<GameObject> GetLetras()
    {
        return EstadopLetras;
    }


    /*   
     * Metodo que permite pedir una pista
     *
     */
    private void Pistas()
    {
        //Si ya he pedido ayuda o pista (maximo 1 )
		if (numAyuda == 0)
        {
            //Avisamos que ya no puede 
            _modalPanel.Elejir("Ya ha usado su ayuda en este nivel",
                               _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
        }
        else
        {
            //Asignar los objetos al choise con el Poner Pista que es el evento 


            /*Avisamos lo que cuesta pedir una pista 
                 * Botones aceptar (accion ejecutar la pista) y cancelar.
                 */
            _modalPanel.Elejir("Una pista cuesta -1ORGULLO. Continuar?",
                               PistasEjecutar, aceptarButton, cancelarButton, false);
        }

    }

    /* 
     * Metodo que ejecuta pedir una pista, actualizando
     * las variales correspondientes y muestra la pista sobre el correspondiente
     * nivel
     * 
     */
    private void PistasEjecutar()
    {
        //Actualizo variables y sus TexMesh
		numAyuda--;
        //Aumentamos ayudas(v.interna)
        ayudas++;

        orgullo--;
        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
        UpdateTextMesh(AyudasTextMesh, ayudas.ToString());
		UpdateTextMesh(numAyudaT, numAyuda.ToString());


        /*
         * Muestro la pista a traves del panel.
         * Accedo a la lista de palabras que contiene todos 
         * los niveles (2 posicion )
         * Actualizamos el Textmesh para que se quede la pista en pantalla
         */
        _modalPanel.Elejir("Este personaje es " + ListaPalabras[contpantallas][2].ToString(),
                           _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
        UpdateTextMesh(cuadernilloTextMesh, ListaPalabras[contpantallas][2].ToString());
    }




    /*
     * Metodo encargado de llamar a un panel perteneciente
     * al script ModalPanel
     * Ademar propociona los elementos necesarios para 
     * el boton pedir ayuda
     * 
     */
    private void LevantarPanel()
    {
        //Obtengo los cuadros parte superior(respuesta)
        IEnumerable<GameObject> resp = GetRespObjs();

        /*
         * Consulta en linq para obtener los cuadros(objetos) donde no hay ninguna letra
         * de los cuadros respuesta y lo guardo en la lista
         */
        List<GameObject> posvacias =
            (from r in resp let t = r.GetComponentInChildren<TextMesh>() where t.text == "" select r).ToList();


        //Si no hay ningun cuadro vacio de la lista 
		if (!posvacias.Any() && numAyuda == 1)
        {
            //Cerramos el panel
            _modalPanel.CerrarPanel();

            /*
             * Pedimos que se desocupe un cuadro.
             * Botones aceptar (accion cerrar panel) y cancelar.
             */
            _modalPanel.Elejir("Desocupe un cuadro de respuesta.",
                               _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
        }
        else//Si hay alguno disponible
        {
            //Si ya ha usado la ayuda en este nivel actual
			if (numAyuda == 0)
            {

                /*Avisamos que ya usado su pista en este nivel
                 * Botones aceptar (accion cerrar panel) y cancelar.
                 */
                _modalPanel.Elejir("Ya ha usado su ayuda en este nivel",
                                   _modalPanel.CerrarPanel, aceptarButton, cancelarButton, true);
            }
            else
            {
                /*Avisamos lo que cuesta pedir una pista 
                 * Botones aceptar (accion poner ayuda) y cancelar.
                 */
                _modalPanel.Elejir("Una ayuda cuesta -1DIAMANTE.Continuar?",
                                   PonerAyuda, aceptarButton, cancelarButton, false);//Asignar los objetos al choise con el Poner Pista que es el evento 
            }
        }


    }

    /*
     * Pone una ayuda en la posible respuesta
     * para el nivel actual ( pone una letra en la solucion)
     */
    private void PonerAyuda()
    {

        //Obtengo los cuadros parte superior(respuesta)
        IEnumerable<GameObject> resp = GetRespObjs();
        //Selecciono los cuadros que no tienen letras
        List<GameObject> posvacias =
            (from r in resp let t = r.GetComponentInChildren<TextMesh>() where t.text == "" select r).ToList();

		//Incrementamos numAyuda(nos avisa que no podemos tener mas ayuda en este nivel)
		numAyuda--;
        //Aumentamos ayudas(v.interna)
        ayudas++;
        //Descontamos diamante (v.personaje)
        diamantesl--;
        //Actualizo TextMesh de las variables 
        UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());
        UpdateTextMesh(AyudasTextMesh, ayudas.ToString());
		UpdateTextMesh(numAyudaT, numAyuda.ToString());

        //Obtengo una posicion donde pondre la ayuda 
        int posrandom = Random.Range(0, posvacias.Count - 1);//Escojo una posicion al azar
        //Obtengo la pabra actual de la respuesta
        string palabraact = GetActualResp();
        //Aqui obtengo la letra que voy a poner
        string l = PosReal(palabraact, posrandom, ListaPalabras[contpantallas][0].ToString());

        //Obtengo el TextMesh y el collider donde va la ayuda
        TextMesh tt = posvacias[posrandom].GetComponentInChildren<TextMesh>();
        BoxCollider c = posvacias[posrandom].GetComponentInChildren<BoxCollider>();
        //Destruyo el collider para que el jugador no pueda quitar la ayuda
        Destroy(c);
        //Actuailizo el TextMesh
        tt.text = l;
        //Pinto la ayuda de verde
        tt.color = Color.green;
        //Letra que puse en la solucion quitarlas de las posibles letras
        AbajoAarriba(l);


        //Update y decrementar ptos total
        //Obtengo la posible respuesta
        var actual = GetActual(GetRespObjs());

        CheckandSet(actual, ListaPalabras[contpantallas][0].ToString());
        //Chequeo a ver si ya completo la palabra





        _modalPanel.CerrarPanel();
    }

    /*
     * Compruebo si la posible respuesta es igual
     * que la respuesta del nivel actual.
     */
    public bool Check(string orden)
    {
        return orden == arregloactual[0].ToString();

    }

    /*
     * Compruebo la palabra (posible respuesta) formada con la respuesta
     * correspondiente al nivel y aumento las variables correspondientes.
     * actual : String con posible respuesta
     * respuesta : String con la respuesta correspondiente del nivel
     */
    public void CheckandSet(string actual, string respuestaa)
    {
        /*
         * Compruebo las longitudes son iguales
         * para hacer las comparaciones
         */
        if (actual.Length == respuestaa.Length)
        {

            if (Check(actual))//Chequeo la palabra actual que se forma
            {
             
                //Aviso del acierto

                /*
                 * Aviso del acierto y pido
                 * ok para poder pasar al siguiente nivel
                 */
                _modalPanel.Elejir("Has acertado!. +1Orgullo de recompensa Pasar siguiente nivel",
                               siguientePantalla, aceptarButton, cancelarButton, true);
            }
            else//Si no es igual es que fallo en la adivinacion
            {
                //Incremento los fallos actualizo el TextMesh
                fallos++;
                UpdateTextMesh(FallosTextMesh, fallos.ToString());

            }
        }
    }

    /*
     * Metodo que se lanza cuando se ha pulsado
     * ok del aviso correspondient al acierto
     * del nivel
     */
    private void siguientePantalla()
    {



        //Si es igual significa que ha completado el reto
        //Incremento los aciertos y diamantes y actualizo el textMesh
        orgullo++;
        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
        aciertos++;
        UpdateTextMesh(AciertosTextMesh, aciertos.ToString());
        
        //No se ha rendido
        rindiendose = false;
        

        Nextpantalla();//cargo la imagen siguiente 


    }

    /*
  
     * Metodo que devuelve las posiciones vacias
     * -letras: Lista actua de objetos que respresentan los cuadrados de la respuesta
     */
    public string GetActual(IEnumerable<GameObject> letras)
    {
        /*
         * 
         * De la lista selecciono sus hijos (componentes)
         * y obtengo sus TextMesh para formar la palaba
         */
        return letras.Select(o => o.GetComponentInChildren<TextMesh>()).Aggregate("", (current, t) => current + t.text);
    }

    /* 
     * Metodo para pasar de nivel
     */
    private void PasarNivel()
    {
        rindiendose = true;
        

        /*
         * LLamamos al metodo elejir del scrip ModalPanel
         * para que ejecute la accion a convenir
         */
        _modalPanel.Elejir("Pasar de nivel 1ORGULLO. Continuar?",
                           Nextpantalla, aceptarButton, cancelarButton, false);//Asignar los o
    }


    /*
     * Metodo que encuentra los cuadrados de la parte
     * superior(respuesta )
     */
    private static IEnumerable<GameObject> GetRespObjs()
    {
        /*
         * Expresion linq para encontrar cuadrados superiores(respuesta)
         * estos cuadrados contienen el nombre con un * y luego los ordeno por la posicion x para 
         * formar correctamente la respuesta
         */
        return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("*")).OrderBy(a => a.transform.position.x);
    }

    /*
     * Metodo que encuentra los cuadrados de la parte
     * inferior(letras a elegir en la posible respuesta )
     */
    private static IEnumerable<GameObject> GetLetrasrand()
    {
        /*
        * Expresion linq para encontrar cuadrados inferior(letras posibles respuesta)
        * estos cuadrados contienen el nombre con un ! 
        */
        return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("!"));
    }



/*
	 * Metodo que permite pasar de nivel con sus correspondientes
	 * modificaciones en las variables del minijuego
	 */
	private void Nextpantalla()
{
		//Reinicio los objetos guardados 
		EstadopLetras = new List<GameObject>();
		//Avanzo el nivel
		contpantallas++; 

		/*
		Recorro la posible respuesta (solucion) de este nivel
		para destuir los objetos (cuadrados)
		 */
		foreach (var obj in GetRespObjs()) 
		{
			DestroyObject(obj);
		}
		/*
		Recorro las letras a elegir en la posible respuesta de este nivel
		para destuir los objetos (cuadrados)
		 */
		foreach (var obj in GetLetrasrand())
		{
			DestroyObject(obj);
		}
		

        /*
         * Si no hemos acertado un nivel
         * modificamos los datos del siguiente nivel
         */

        if (rindiendose)
        {
	        //Aumentamos variable interna
	        siguientes++;
	        UpdateTextMesh(SiguientesTextMesh, siguientes.ToString());
	        vecesrendidas++;
	        orgullo--;
	        UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
	        rindiendose = false;

        }

       


        
        _modalPanel.CerrarPanel();


		if (contpantallas > 2)
		{
		

            /*
             * Avisamos que se ha terminado
             * el minijuego
             * */
            _modalPanel.Elejir("Terminaste de ayudar a Juan en los deberes!. Continuar",
                               FinJuego, aceptarButton, cancelarButton, true);



		   
	
            /*
             AQUI SE TIENE IMPLEMENTAR CUANDO SE QUIERE RENDIRSE
             * EN LA ULTIMA PANTALLA
             * AQUI TENGO QUE TRATAR LAS VARIABLES CUANDO
             * SE VA TERMINAR EL JUEGO!
             */
			/*diamantesl += 1;
			
			UpdateTextMesh(OrgulloTextMesh, orgullo.ToString());
			UpdateTextMesh(DiamantesTextMesh, diamantesl.ToString());
			*/
		}
		else //Generar los cuadrados para el siguiente nivel (imagen,letras,respuesta)
		{
			//Actualizo TextMesh para siguiente nivel
			UpdateTextMesh(nivelT,(contpantallas+1) +" / "+ (ListaPalabras.Count()));
			
			//Compruebo si he pedido ayuda
			if (numAyuda == 0)
			{
				//Reinicio la ayuda para tenerla disponible en el proximo nivel
				numAyuda++;
				UpdateTextMesh(numAyudaT, numAyuda.ToString());

			}

			//Actualizar arreglo actual
			arregloactual = ListaPalabras[contpantallas];

			//Reseteando variables y generadores
			__incrementaletras = 0;
			_incrementader = 0;
			_incrementaizq = 0;
			//Quito la pista si la hubiera
			cuadernilloTextMesh.text = "";

			//Creo los generadores de cuadrados superior e inferior
			//respuesta y las letras de las posibles respuestas()
			GeneradorIzq.transform.position = new Vector3(-1.82f, -1.09f, 0f);
			GeneradorDer.transform.position = new Vector3(0.02999997f, -1.09f, 0f);
			GeneradorLetras.transform.position = new Vector3(-5.45f, -2.48f, 0f);
			//Genero otra vez los cuadros pero con el siguiente nivel
			PrepararGenerar(ListaPalabras[contpantallas], contpantallas);
		}
	}

    /*
     * 
     * Metodo que permite cambiar de escena cuando el minijuego
     * ha terminado
     * */
    private void FinJuego()
    {
		VariablesPersonaje.variablesPersonaje.UpdateOrgullo (orgullo);
		VariablesPersonaje.variablesPersonaje.UpdateDiamantes (diamantesl);


        Application.LoadLevel("MenuMain");
    }


	/*
	 * 
	 * Volteamos la palabra x. Es decir
	 * si se recibe PEPE devoleria EPEP
	 * palabra= palabra a voltear
	 */
	private static string Voltear(string palabra)//Darle la vuelta a una palabra o sea si es "PEPE" ponerla EPEP
	{

		string voltear = "";
		int cont = palabra.Length - 1;
		//Inserto por delante de voltear hasta recorrer 
		//la palabra completa
		while (cont != -1)
		{
			voltear += palabra[cont];
			cont--;
		}

		return voltear;
	}
	
	
	/*
	 * 
	 * Funcion para generar los respectivos cuadrados de la parte
	 * superior(posible solucion) y los cuadrados de la parte inferiror
	 * (letras a elegir para posible solucion)
	 * datos = Array del nivel referencia para generar cuadrados
	 * numero = numero de imagen del nivel a jugar 
	 */
	private void PrepararGenerar(object[] datos, int numero)
	{
		int inc = 0; // Pongo inc a 0 

		//Guardo la palabra de los datos de respuesta
		_palabra = datos[0].ToString(); 

		//Sin son par las letras estan emparejadas
		if (_palabra.Length%2 != 0)  
		{
			inc++;
		}
		
		/*
		 * Almaceno la mitad de la palabra (izquierda-centro) y le doy la
		 * la vuelta para que aparezca en el orden correcto , esto es asi porque estoy generando de derecha a izquiera 
		 */
		_palabrarespizq = Voltear(_palabra.Substring(0, _palabra.Length/2));

        /*
         * Tomo la mitad de la palabra a la derecha del centro.
         * No le doy la vuelta porque ahora si genero en el orden correcto
         * es decir de izquierda a derecha!
         */
        _palabrarespder = _palabra.Substring(_palabra.Length / 2, _palabra.Length / 2 + inc);
		
		//Borro el texto del cuadro
		TextorResp.text = "";

		/*
		 * obtengo el eje x y el eje y del generador(posicion inicial)
		 */
		_posinigen = GeneradorLetras.transform.position.x;
		_posinoy = GeneradorLetras.transform.position.y;


		//Obtengo el objeto "Imagen"(canvas) de la escena
		_spriteContainer = GameObject.Find("Imagen").GetComponentInChildren<Image>();

        /*
         * Cargo la imagen (sprites) desde resources ya que es la unica
         * forma que se lea en tiempo de ejecucion.
         * La imagen va variando a medida que se avanza de nivel
         */
        Sprite s = (Sprite)Resources.Load(numero.ToString(), typeof(Sprite));

		//Cambio el color y asigno la imagen correspondiente
		_spriteContainer.color = Color.white;
		_spriteContainer.sprite = s;

		//Asigno las letra de la palabra 
		_palabraletras = datos[1].ToString();// Asigno las letras
		
		//

		/*
		 * Inicializo variables para saber las letras que corres
		 * que hay en total y cuantas hay de izquierda a derecha
		 * Inicializo variablessdsadasdsaçsadç
		 */
		/*
		 * Numero de letras de la respuesta 
		 * de izquierda al centro
			*/
		_contizq = 0;
		/*
		 * Numero de letras de la respuesta 
		 * de centro a la derecha 
		*/
		_contder = 0;
		/*
		 * Numero de las letras de la 
		 * posible respuesta 
		 *
		*/
		_contletras = 0;

		GenerarIzq();
		GenerarDer();
		GenerarLetras();
		
	}

	

	/*
	 * Metodo que se encarga de generar los recuadros inferiores
	 * (letras para la posible respuesta)
	 * 
	 * 
	 * 
	 * 
	 * Este metodo se auto-invoca segun el numero de letras que contenga
	 * la _palabrarespizq (el metodo genera solo una letra)
	 */
	private void GenerarLetras()

	{


         float y = 0;
		//Posicion del generador en y
		y = GeneradorLetras.transform.position.y;

		/*
		 * Guardo la letra que voy generando, cambior el color
		 * y escribo la respectiva letra correspondiente al recuadro
		 * y asigno el nombre al cuadro inferior
		 */
		var letra = _palabraletras[_contletras].ToString();
		SpriteLetra.color = Color.red;
		TextoLetras.text = letra;
		CuadroLetra.name = _contletras + "!";


		//A mitad de la palabra actualizo eje y para segunda fila 
		if (_contletras == _palabraletras.Length / 2) 
		{
			//pongo la variable al principio
			__incrementaletras = 0f;
			//Actualizo eje y para que se dibuje en la segunda fila
			y = _posinoy - 1.45f;
			//Cambio de posicion el generador 
			GeneradorLetras.transform.position = new Vector3(_posinigen, _posinoy - 1.45f, GeneradorLetras.transform.position.z);
		}

		// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
		Instantiate(CuadroLetra, new Vector3(GeneradorLetras.transform.position.x + __incrementaletras, y),Quaternion.identity);
		__incrementaletras += 1.75f;

		/*
		 * LLamo al mismo metodo cada 0.01 segundos
		 * y aumento contador de letras para saber cuando parar
		 * de generar
		 */
		Invoke("GenerarLetras", 0.01f);
		_contletras++;

			/*
		 * Cuando el contador del generador alcanza el numero de 
		 * las posibles letras de la respuesta cancelamos el generador
		 */
		if (_contletras == _palabraletras.Length)
		{
			_contletras = 0;
			CancelInvoke("GenerarLetras");
			
		}
	}
	/*
	 * Metodo que se encarga de generar los recuadros superiores(respuesta)
	 * Solo genera la parte de izquierda al centro.
	 * Este metodo se auto-invoca segun el numero de letras que contenga
	 * la _palabrarespizq (el metodo genera solo una letra)
	 */
	private void GenerarIzq()
	{
		// Pinto el cuadrado de azul y borro el exto (aqui va la respuesta
		SpriteResp.color = Color.blue;
		TextorResp.text = "";
		
		// Asigno el nombre para guiarme a la hora de resolver el asertijo(respuesta)
		CuadroResp.name = "*" + _contizq + "i";
		
		// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
		Instantiate(CuadroResp, new Vector3(GeneradorIzq.transform.position.x + _incrementaizq, GeneradorIzq.transform.position.y),Quaternion.identity);
		_incrementaizq += -1.75f;
		
		//LLamo al mismo metodo cada 0.01 segundos 
		Invoke("GenerarIzq", 0.01f);
		_contizq++;// Aqui incremento cont para ir iterando por las letras
		
		/*
		 * Cuando el contador del generador alcanza el numero de letras
		 * de la parte izquierda de la palabra, cancelamos el generador
		 */
		if (_contizq == _palabrarespizq.Length){
			_contizq = 0;
			CancelInvoke("GenerarIzq");
			
		}
	}


	
	/*
	 * Metodo que se encarga de generar los recuadros superiores(respuesta)
	 * Solo genera la parte de centro a la derecha.
	 * Este metodo se auto-invoca segun el numero de letras que contenga
	 * la _palabrarespizq (el metodo genera solo una letra)
	 */
	private void GenerarDer()
	{
		// Pinto el cuadrado de azul y borro el exto (aqui va la respuesta)
		SpriteResp.color = Color.blue; 
		TextorResp.text = "";

		// Asigno el nombre para guiarme a la hora de resolver el asertijo(respuesta)
		CuadroResp.name = "*" + _contder + "d";

		// Una vez puesto los objetos en su lugar instanceo el cuadro para dibujar
		Instantiate(CuadroResp, new Vector3(GeneradorDer.transform.position.x + _incrementader, GeneradorDer.transform.position.y),Quaternion.identity);
		_incrementader += 1.75f;

		//LLamo al mismo metodo cada 0.01 segundos
		Invoke("GenerarDer", 0.01f);
		_contder++;// Aqui incremento cont para ir iterando por las letras

		/*
		 * Cuando el contador del generador alcanza el numero de letras
		 * de la parte derecha de la palabra, cancelamos el generador
		 */
		if (_contder == _palabrarespder.Length)	{
			_contder = 0;
			CancelInvoke("GenerarDer");
			
		}
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
     * Especifico un bloque de código que se puede expandir o contraer.
     * No tiene funcionalidad especifica, nos sirve para 
     * guiarnos en los metodos respectivos 
     * 
     */
	#region Metodos para pistas
	/*
     * Metodo que forma la palabra que esta en los
     * cuadros superiores(respuesta) eliminando las posiciones
     * vacias y dejando las que tienen letras
	 */
	private string GetActualResp()
	{
        /*
         * Expresion linq. Obtengo los cuadrados de la 
         * parte superior (de tipo respuesta y los ordeno
         * segun su eje x para matener el orden )
         */
		IEnumerable<GameObject> resp = FindObjectsOfType<GameObject>().Where(a => a.name.Contains("*")).OrderBy(a => a.transform.position.x);

        /*
         * Declaro una variable generica
         */
		var fin = "";
		bool bandera = false;
		
		foreach (var r in resp)
		{
           
            /*
             * Obtengo el TextMesh de cada cuadro 
        para comprobar su texto si es vacia marco 
             */
            var t = r.GetComponentInChildren<TextMesh>();
			if (t.text == "")
			{
				fin += "*";
			}
			
			
			else
			{
                //Si es de tipo rojo marco y guardo el texto
				if (t.color == Color.red)
				{
					fin += "#" + t.text;
				}
				else
				{
					fin += t.text;
				}
				//Sabemos que ese cuadro tiene letra
				bandera = true;
			}
		}
		if (bandera == false)
		{
            //Si no tiene letra reiniciamos
			fin = null;
		}
		else
		{
            /*
             * Creo una lista l
             */
			List<int> c = new Procesador().findposelemt(ListaPalabras[contpantallas][0].ToString());
			if (c.Count != 0)
			{
				fin = c.Aggregate(fin, (current, i) => current.Insert(i, "_"));
			}
			fin = new Procesador().Reindex_(fin);
		}
		
		
		return fin;
	}



    /*
     *
     * 
     * 
     * Obtengo una letra correspondiente a la respuesta(solucion)
     * del nivel actual
     * palabraact
     * pos
     * respuestaax
     */
    public string PosReal(string palabraact, int pos, string respuestaax)
	{
		int cont = 0;
		if (palabraact == null)
			cont = pos;
		else
		{
			palabraact = new Procesador().Removepista(palabraact);
			palabraact = new Procesador().Remove_(palabraact);
			for (int i = 0; i < palabraact.Length; i++)
			{
				if (palabraact[i] == '*')
				{
					
					if (cont == pos)
					{
						cont = i;
						
						break;
					}
					cont++;
				}
			}
		}
		
		return respuestaax[cont].ToString();
	}

	/*
	 * Metodo que quita una letra posible para las respuestas 
	 * que haya sido puesto en la respuesta 
	 */
	private void AbajoAarriba(string letra)
	{
		//Obtengo
		IEnumerable<GameObject> objs = Getactualrandomabajo(GetActualRandom());
		foreach (var o in objs)
		{
			var tresp = o.GetComponentInChildren<TextMesh>();
			if (tresp.text == letra)
			{
				o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 20f,
				                                   o.transform.position.z);
				EstadopLetras.Add(o);
				break;
			}
		}
	}

    /*
     * 
     *  
     * 
     * Metodo que devuelve el estado de las letras para la posible
     * respuesta  (cuadros inferiores)
     * 
     
     */
    private IEnumerable<GameObject> Getactualrandomabajo(IEnumerable<GameObject> todos)
	{
		List<GameObject> fin = new List<GameObject>();
        //Ordeno segun su posicion x (fila de los cuadros inferiores)

		todos = todos.OrderBy(a => a.transform.position.x);


		foreach (var l in todos)
		{
			if (l.transform.position.y - Marcador.transform.position.y < 0.2)
			{
				fin.Add(l);
			}
		}
		return fin;
	}
	
    /*
     * Metodo que devuelve una lista con los objetos
     * cuadros inferiores (letras para la posible respuesta)
     
     */
	private static IEnumerable<GameObject> GetActualRandom()
	{
		//Obtengo todos los objetos que este marcados ! (cuadros inferiores utilizan esta marca)
		return FindObjectsOfType<GameObject>().Where(a => a.name.Contains("!"));
	}
	#endregion

}

