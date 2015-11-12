using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Inventory : MonoBehaviour, IChanged {

	//Serializamos los  atributos
	[SerializeField] Transform slotsSuperior;

	[SerializeField] Text inventoryText;


	// Use this for initialization
	void Start () {
	
		//Llamo a la funcion que detecta cambio
		Changed ();
	}
	

	#region IChanged implementation
	/*
	 * Cada vez que hay un cambio en el slot, mostramos 
	 * el nombre del slot segun el orden correspondiente
	 */
	public void Changed ()
	{
		//Para crear la estructura StringBuilder
		System.Text.StringBuilder builder = new System.Text.StringBuilder ();

		//Apilo, inicio
		builder.Append (" - ");

		//Recorro los slot existentes
		foreach (Transform slotTransform in slotsSuperior){

			//Obtengo el slot a tratar
			GameObject item = slotTransform.GetComponent<Slot>().item;

			if(item)
			{
				//Apilo el nombre del item
				builder.Append(item.name);
				builder.Append (" - ");
			}

		}

		//Guardo el texto del builder
		inventoryText.text = builder.ToString ();

	}
	#endregion
}

/*
 * Agregamos funcionalidad al namespace de UnityEngine.EventSystems
 * 
 */
namespace UnityEngine.EventSystems{

	//Creo una intefaz para utilizar
	public interface IChanged : IEventSystemHandler{

		//Metodo a implementar
		void Changed();

	}
}