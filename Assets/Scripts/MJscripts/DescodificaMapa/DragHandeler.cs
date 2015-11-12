using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/*

Maneja el drag de los objetos que contiene este scrip
 */

public class DragHandeler : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler {

	//Objeto del juego que esta siendo arrastrado
	public static GameObject itemInicio;
	//Vector para almacenar la posicion del obj arrastrado
	Vector3 startPos;
	//Para obtener el transform padre del objeto
	Transform startParent;


	#region IBeginDragHandler implementation

	/*Llamada antes de arrastrar, hace esto cuando 
	empieza arrastrar el elemento del script*/
	public void OnBeginDrag (PointerEventData eventData)
	{
		//Obtenemos el objeto del script
		itemInicio = gameObject;
		//Guardo la posicion de inicio del objeto
		startPos = transform.position;
		//Guardo la posicion parent del objeto
		startParent = transform.parent;
		//Grupo Raycast permite colision
		GetComponent<CanvasGroup> ().blocksRaycasts = false;

	}
	#endregion

	#region IDragHandler implementation

	/*
	Cuando se esta arrastrando ocurre esto cada vez que 
	el cursor se mueve
	 */
	public void OnDrag (PointerEventData eventData)
	{
		 //Colocamos el objeto(posicion) segun el cursosr
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	/*Hace esto cuando termina el arrastre*/
	public void OnEndDrag (PointerEventData eventData)
	{
		//Apunto a vacio
		itemInicio = null;

		//Grupo Raycast permite colision
		GetComponent<CanvasGroup> ().blocksRaycasts = true;



		/*
		 * Si el tranform padre actual es distinto
		 * a mi transform de inicio 
		 * Devuelvo el objeto a posicion de inicio
		 */
		if( transform.parent != startParent){

			transform.position = startPos;
		}



	}

	#endregion



}
