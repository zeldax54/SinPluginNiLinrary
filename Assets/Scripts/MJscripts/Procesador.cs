using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Script que contiene funciones para manejar cadenas(string)
 */
public class Procesador  {

    /*
     * Metodo para manejar string(palabras)
     * que tienen espacios de por medio,
     * Los espacios se representan con : _
     * Los # representa que una letra
     * ha sido dada como ayuda 
     * 
     * 
     * */
    public string Reindex_(string fin)
    {
        if (!fin.Contains("_")) return fin;
        for (int i = 0; i < fin.Count(); i++)
        {
            if (fin[i] != '_') continue;
            if (fin[i - 1] != '#') continue;
            fin = fin.Remove(i, 1);
            fin = fin.Insert(i + 1, "_");
        }

        return fin;
    }


    /*
     * Este metodo encuentra el caracter # (letras
     * que han sido puestas por la ayuda) 
     * para
     * 
     * 
     * Metodo que se encarga  de encontras las posiciones
     * de la palabra que tengan espacio (_) y los guardo
     * 
     */
    public List<int> findposelemt(string x)
    {
        var posiciones = new List<int>();
        var cont = 0;
        foreach (var c in x)
        {
            if (c == '_')
            {

                posiciones.Add(cont);

            }
            cont++;
        }

        return posiciones;
    }

    /*
     * Este metodo encuentra el caracter # (letras
     * que han sido puestas por la ayuda) 
     * para borrar las pistas
     * 
     * */
        public string Removepista(string palabraa)
        {
            return palabraa.Where(l => l != '#').Aggregate("", (current, l) => current + l);
        }

        /*
          * Este metodo encuentra el caracter _ (espacios
         * en una palabra) y los borra
          * 
          * */
        public string Remove_(string pal)
        {
            return pal.Where(variable => variable != '_').Aggregate("", (current, variable) => current + variable);
        }
}
