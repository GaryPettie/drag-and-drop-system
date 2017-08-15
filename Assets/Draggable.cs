using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public enum CardType { MINION, MANA, BUFF, ATTACK, SPELL };
	public CardType cardtype;

	Vector3 m_offset;
	GameObject placeholder = null;

	Transform m_placeholderParent = null;
	public Transform getPlaceholderParent { get { return m_placeholderParent; } }
	public Transform setPlaceholderParent { set { m_placeholderParent = value; } }

	Transform m_returnParent = null;
	public Transform getReturnParent { get { return m_returnParent; } }
	public Transform setReturnParent { set { m_returnParent = value; } }


	public void OnBeginDrag (PointerEventData pointer) {
		//Picks up the object from the pointer positon, rather than snapping the centre of the object to the pointer
		m_offset = transform.position - (Vector3) pointer.position;

		//Creates a placeholder object to allow the dragged card to return to a custom position in the dropzone.
		LayoutElement layoutElement = GetComponent<LayoutElement>();
		placeholder = new GameObject();
		placeholder.transform.SetParent(transform.parent);
		LayoutElement placeholderLayoutElement = placeholder.AddComponent<LayoutElement>();
		placeholderLayoutElement.preferredWidth = layoutElement.preferredWidth;
		placeholderLayoutElement.preferredHeight = layoutElement.preferredHeight;
		placeholderLayoutElement.flexibleWidth = 0;
		placeholderLayoutElement.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

		//Parents the object to the parent's parent 
		setReturnParent = transform.parent;
		setPlaceholderParent = getReturnParent;
		transform.SetParent(getReturnParent.parent);

		//Prevents the object from blocking raytcasts to the dropzone
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag (PointerEventData pointer) {
		//Moves the object being dragged
		transform.position = (Vector3) pointer.position + m_offset;

		//Changes the placeholder parent to the current dropzone
		if (placeholder.transform.parent != getPlaceholderParent) {
			placeholder.transform.SetParent(getPlaceholderParent);
		}

		//Moves the placeholder based on current drag location
		int newSiblingIndex = getPlaceholderParent.childCount;
		for (int i = 0; i < getPlaceholderParent.childCount; i++) {
			if (transform.position.x < getPlaceholderParent.GetChild(i).position.x) {
				newSiblingIndex = i;
				if (placeholder.transform.GetSiblingIndex() < newSiblingIndex ) {
					newSiblingIndex--;
				}
				break;
			}
		}
		placeholder.transform.SetSiblingIndex(newSiblingIndex);
	}

	public void OnEndDrag (PointerEventData pointer) {
		//Reparents the object to the one it came from - This may be overriden if a different valid dropzone is found
		transform.SetParent(getReturnParent);

		//Sets the object in the position of the placeholder
		transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		//Destroys the placeholder object when the current one is dropped
		Destroy(placeholder);

		//Allows the object to be selectable again
		string dropZone = transform.parent.GetComponent<DropZone>().tag;
		if(dropZone == "HandArea") {
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}
}
