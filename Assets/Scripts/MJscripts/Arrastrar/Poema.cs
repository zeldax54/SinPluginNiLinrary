
using System.Collections.Generic;
using System.Linq;

/*
 * Script que contiene los elementos referentes a un poema
 * 1.Texto de un poema con sitios vacios, representados por  "*"
 * 2.Palabras verdaderas, las que son correctas para sitios vacios  "*"
 * 3.Palabras falsas, las que no son correctas para "*"
 * 
 * Contiene clase palabra
 * */
public class Poema {

	//Lineas del poema en donde hay * es porque falta la palabra
	//Lista con las lineas del poema
	public List<string> textoPoemaLineas { get; set; }
	//Lista de posibles palabras que faltan.Con su posicion dentro del texto
	public List<Palabra> palabrasP { get; set; } 
	//palabras que no van en el texto.Cuya posicion la asigno en -1
	public List<Palabra> falsaspalabras { get; set; } 
	
	
	//Construcor Vacio para usar fuera de la clase
	public Poema()
	{
		
	}

	//Constructor con parametros para usar dentro de la misma clase
	private Poema(List<string> pTextoPoemaLineas,List<Palabra>pPalabras,List<Palabra>pFalsasPalabras )
	{
		textoPoemaLineas = pTextoPoemaLineas;
		palabrasP = pPalabras;
		falsaspalabras = pFalsasPalabras;
	}

	/*
	 * Inicializa la lista que contendra un poema
	 * para cada nivel.
	 * Inicializa el texto del poema, las palabras
	 * posibles para cada hueco "_" del poema
	 * y las palabras falsas (las que no correctas
	 * para cada "_"
	 *
	 * */

	public List<Poema> InicializarListaPoemas()
	{
		var poemas = new List<Poema>();

		//Primer Poema
		var poemaTexto1 = new List<string>()
		{
			"Yo quiero cuando me *",
			"Sin Patria pero sin *",
			"Tener en mi Losa un ramo",
			"De flores y una *"
		};
		
		var poemaPalabras1 = new List<Palabra>()
		{new Palabra("Muera",0),new Palabra("Amo", 1),new Palabra("Bandera",3)};
		var poemaFalsas1 = new List<Palabra>()
		{new Palabra("Vaya",-1),new Palabra("Espada",-1)};
		//Agregamos el primer poema
		poemas.Add(new Poema(poemaTexto1,poemaPalabras1,poemaFalsas1));


		//Segundo Poema
		var poemaTexto2 = new List<string>()
		{
			"Si quieren que de este *",
			"Lleve una memoria grata,",
			"Llevaré * profundo,",
			"Tu cabellera de *"
		};
		var poemaPalabras2 = new List<Palabra>()
		{new Palabra("Mundo",0),new Palabra("Padre", 2),new Palabra("Plata",3)};
		var poemaFalsas2 = new List<Palabra>()
		{ new Palabra("Pais", -1), new Palabra("Oro", -1)};
		//Agregamos el segundo poema
		poemas.Add(new Poema(poemaTexto2, poemaPalabras2, poemaFalsas2));//Poema 2



		//Tercer Poema
		var poemaTexto3 = new List<string>()
		{
			"Estimo a quien de un revés,",
			"Echa por * a un tirano",
			"Lo estimo, si es un *",
			"Lo estimo, si es *"
		};
		var poemaPalabras3 = new List<Palabra>() { new Palabra("Tierra", 1), new Palabra("Cubano", 2), new Palabra("Aragonés", 3) };
		var poemaFalsas3 = new List<Palabra>() { new Palabra("Suelo", -1), new Palabra("Mexicano", -1) };
		//Agregamos el segundo poema
		poemas.Add(new Poema(poemaTexto3, poemaPalabras3, poemaFalsas3));//Poema 3
		return poemas;
	}


	/*
	 * Metodo que me devuelve la palabra que hay en la linea
	 * palabrasL = palabras donde busco una en concreto
	 * linea = linea semejante a la palabra que necesito
	 * */
	public Palabra GetPalabradeLinea(List<Palabra> palabrasL, int linea)
	{
		return palabrasL.FirstOrDefault(p => p.posicion == linea);
	}
}

/*
 * Clase que contiene 
 * el texto y la posicion
 * correspondiente a una palabra
 * 
 * */
public class Palabra
{
	public string palabra {get; set; }
	/*
	 * Posicion donde esta la palabra en las
	 * lineas del poema, empenzando 
	 * Linas del poema enumerar en 0
	 * */
	public int posicion { get; set; }
	
	public Palabra(string ppalabra,int pposicion)
	{
		palabra = ppalabra;
		posicion = pposicion;
	}
}

