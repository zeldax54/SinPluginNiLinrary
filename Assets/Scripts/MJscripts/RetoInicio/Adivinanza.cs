using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Adivinanza  {

    //Numero de adivinanza
    public string numero { get; set; }
    //Adivinanza(pregunta)
    public string adivinanza { get; set; }
    //Respuesta correcta
    public string respuesta { get; set; }
    //Vector con respuestas falsas
    public string[] falsasrespuestas { get; set; }


    /*
     * Devuelvo la adivinanza correspondiente al numero
     * que se encuentra en la lista
     * numeroPos : posicion de adivinanza que necesito
     * es decir el numero de la adivinanza
     * 
     * */
    public Adivinanza Getadivinazas(string numeroPos)
    {
        return _adivinanzas.FirstOrDefault(item => item.numero == numeroPos);
    }

    //Para almacenar los diferentes objetos(adivinanzas)
    private List<Adivinanza> _adivinanzas;


    /*
     * Devuelvo la longitud de la lista
     * es decir el numero de adivinanzas
     * 
     * */
    public int ContAdivinanzas()
    {
        return _adivinanzas.Count;
    }

    /*
     * 
     * Inicializa la lista insertando
     * 4 adivinanzas con su correspondientes
     * preguntas y respuesta verdadera y 2 respuestas
     * falsas
     * */
    public void InicializarLista()
    {
        //Creamos la lista que contendra la adivinanza
        _adivinanzas = new List<Adivinanza>();

        Adivinanza primeraAdivinanza = new Adivinanza();

        /*
         * Asignamos a la adivinanza
         * el numero, la adivinanza,
         * la respuesta verdadera y 
         * las otras 2 respuestas (falsas)
         * 
         * */
        primeraAdivinanza.numero = "1";
        primeraAdivinanza.adivinanza = "Oro parece plata no es quien no lo adivine bien tonto es";
        primeraAdivinanza.respuesta = "Platano";
        primeraAdivinanza.falsasrespuestas = new string[] { "La Plata", "Oro es" };


        Adivinanza segundaAdivinanza = new Adivinanza();
        segundaAdivinanza.numero = "2";
        segundaAdivinanza.adivinanza = "Ya vez Ya vez quien no lo adivine bien tonto es";
        segundaAdivinanza.respuesta = "Llaves";
        segundaAdivinanza.falsasrespuestas = new string[] { "Lo vi", "Un Ojo" };

        Adivinanza terceraAdivinanza = new Adivinanza();
        terceraAdivinanza.numero = "3";
        terceraAdivinanza.adivinanza = "Que tengo en el bolsillo?Dijo Bilbo.";
        terceraAdivinanza.respuesta = "Un anillo";
        terceraAdivinanza.falsasrespuestas = new string[] { "Una Piedra", "o Nada" };

        Adivinanza cuartaAdivinanza = new Adivinanza();
        cuartaAdivinanza.numero = "4";
        cuartaAdivinanza.adivinanza = "Canta sin voz, vuela sin alas sin dientes muerde sin boca habla.";
        cuartaAdivinanza.respuesta = "El Viento";
		cuartaAdivinanza.falsasrespuestas = new string[] { "Una Hoja", "El cielo" };

        //Agrego las adivinanzas a la lista
        _adivinanzas.Add(primeraAdivinanza); 
        _adivinanzas.Add(segundaAdivinanza);
        _adivinanzas.Add(terceraAdivinanza);
        _adivinanzas.Add(cuartaAdivinanza);
    }



}
