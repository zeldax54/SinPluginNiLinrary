using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


/*
*/
public class Slot : MonoBehaviour, IDropHandler {



	//Esta clase podra utilizar los siguientes metodos
	public GameObject item
	{
		//Devuelve 
		get{
			//El objeto del scrip devuelve el 1º hijo (si tiene)
			if(transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}

			//Si no tiene hijos devolvmeos vacio
			return null;



		}

	}





	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		if(!item)
		{
			//Establece el nuevo transform del padre del objeto itemInicio
			DragHandeler.itemInicio.transform.SetParent(transform);

			/*

			 */
			ExecuteEvents.ExecuteHierarchy<IChanged>(gameObject,null,(x,y) => x.Changed());
		}
	}
	#endregion
}
