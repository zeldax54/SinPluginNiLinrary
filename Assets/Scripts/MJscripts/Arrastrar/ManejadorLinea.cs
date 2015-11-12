using UnityEngine;
using System.Collections;

/*
 * Manejador de linea que trata lo referido
 * a la posicion vacia ( el hueco = "_" )
 * que aparece en la linea del poema en 
 * la Ui de la escena
 * */
public class ManejadorLinea : MonoBehaviour {
	
	
	private TextMesh t;
	void Awake()
	{
		t = GetComponent<TextMesh>();
	}


	/*
	 * Busca el primer elemento vacio
	 * correspondiente a una palabra y
	 * devuelve la posicion
	 * En caso de no encontrar ( _ ) devuelve -1
	 * x = palabra donde se busca el primer hueco vacio (_)
	 * 
	 * */
	public int FindFirst_()
	{
		for (int i = 0; i < t.text.Length; i++)
		{
			if (t.text[i] == '_')
			{
				return i;
			}
		}
		return -1;
	}

	/*
	 * Metodo que comprueba si el TextMesh
	 * contiene un hueco "_" 
	 * 
	 * */
	public bool LineaDisponible()
	{
		if (t.text.Contains("_"))
			return true;
		return false;
	}

	/*
	 * Metodo que devuelve la palabra
	 * que tiene puesta
	 * 
	 * */
	public string GetActualPuesta()
	{
		string x = t.text;

		//Devolvemos vacio si es _ vacio
		if (x.Contains("_"))
			return "";


		string olddword = "";
		int posprimercierre = 0;

		//Recorro el texto del TextMesh
		for (int i = 0; i < x.Length; i++)
		{
			//encontrar primer cierre del color
			if (x[i] == '>') 
			{
				posprimercierre = i + 1;
				break;
			}
		}
		
		for (int i = posprimercierre; i < x.Length; i++)
		{
			//Abre segundo marcador
			if (x[i] == '<')
			{
				break;
			}
			
			olddword += x[i];
		}
		return olddword;
	}
	
	public void SetCorrectWord(string newcolor)
	{
		t.text = t.text.Replace("#ff0000ff", newcolor);//Cambio el color y eso hace que no aparezca como disponible
		
	}

	public string PintarPalabra(string palabra, string color)
	{
		palabra = palabra.Replace("#ff0000ff", color);
		return palabra;
	}

	/*
	 * Metodo para saber si hay una palabra en 
	 * rojo, es decir si hay una palabra puesta
	 * 
	 * */
	public bool ContienePalabraDeResp()
	{
		if ( t.text.Contains("008000ff"))
			return true;
		return false;
	}

    
    //Para saber si hay una palabra en rojo ahi
	public bool GetPalabraInsegura()
	{
		if (t.text.Contains("ff0000ff"))
			return true;
		return false;
	}

	/*
	 * Metodo que me permite agregar una palabra
	 * al TextMesh que contiene a este script
	 * Se pone el palabra en la TextMesh y se colorea
	 * 
	 * palabra = TextMesh que sera añadido al texto
	 * pos = posicion donde debe de ser puesta
	 * 
	 * */

	public void PutText(TextMesh palabra, int pos)
	{
		t.text = t.text.Insert(pos, "<color=#ff0000ff>" + palabra.text + "</color>");
	}


	/*
	 * 
	 * Metodo que remueve _ (vacio) de la 
	 * linea de texto
	 * 
	 * palabra: texto que contiene _ (vacio)
	 * 
	 * 
	 * */
	public void Remove_()
	{
		//Contendra la linea completa (borrado = _ )
		string fin = "";

		for (int i = 0; i < t.text.Length; i++)
		{
			if (t.text[i] != '_')
				fin += t.text[i];
		}
		t.text= fin;
	}

    //Saber si la palabra puesta es correcta
 



    /*
	 * 
	 * Metodo encargado de encontra la palabra antigua (TextMesh)
	 * (es decir la primera que ha sido puesta-color rojo-)
	 * y colocar la nueva palabra.
	 * Devuelve la palabra antigua
	 * 
	 * newPalabra = palabra nueva que sera puesto en el texto (linea)
	 * 
	 * 
	 * /Encontrar la palabra en rojo reemplazarla por
	 * la nueva y devolver la vieja para activar el gameobject en la pantalla
	 * 
	 * 
	 * */

	public string FindandReplaceRedWord(string newpalabra)
	{
		//Obtengo el texto de la palabra
		string x = t.text;
		//Palabra antigua
		string olddword = "";


		/*
		 * Al utilizar una etiqueta <> Para
		 * dar color a la palabra vieja
		 * Recorremos el texto X y obtenemos
		 * la posicion del primer cierre de 
		 * etiqueta
		 * 
		 * */
		int posprimercierre = 0;
		for (int i = 0; i < x.Length; i++)
		{
			if (x[i] == '>') //encontrar primer cierre del color
			{
				posprimercierre = i + 1;
				break;
			}
		}



		/*
		 * Al utilizar una etiqueta <> Para
		 * dar color a la palabra vieja
		 * Recorremos el texto X y obtenemos
		 * la posicion del segundo marcador de 
		 * etiqueta
		 * 
		 * */
		int posultimocierre = 0;
		for (int i = posprimercierre; i < x.Length; i++)
		{
			if (x[i] == '<') //Abre segundo marcador
			{
				posultimocierre = i;
				break;
			}

			//Vamos formando la palabra vieja hasta que llegemos al marcador de etiqueta
			olddword += x[i];
		}

		/*
		 * Removemos la cadena eliminando desde posprimercierre
		 * hata la posicion donde termina la palabra vieja
		 * es decir : posultimocierre - posprimercierre 
		 * posiciones de inicio y cierre de etiqueta de color
		 * rojo (palabra antigua)
		 * */
		x = x.Remove(posprimercierre, posultimocierre - posprimercierre);
		/*
		 * Insertamos newPalabra en el texto X
		 * a partir de la posicion posprimercierre
		 * que es donde empieza la etiqueta de color 
		 * es decir la palabra que ha sido puesta en _
		 * */
		x = x.Insert(posprimercierre, newpalabra);
		t.text = x;
		return olddword;
	}
	
}
