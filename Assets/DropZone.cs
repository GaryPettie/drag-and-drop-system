using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Draggable.CardType cardType;

	[SerializeField] private int m_cardLimit = 6;
	public int cardLimit { get { return m_cardLimit; } set { m_cardLimit = value; } }

	Draggable draggable;


	public void OnPointerEnter (PointerEventData pointer) {
		if (pointer.pointerDrag == null) {
			return;
		}

		draggable = pointer.pointerDrag.GetComponent<Draggable>();
		if (draggable != null) {
			if (cardType == draggable.cardtype && draggable.getPlaceholderParent.childCount -1 < cardLimit) {
				draggable.setPlaceholderParent = transform;
			}
		}
	}

	public void OnPointerExit (PointerEventData pointer) {
		if (pointer.pointerDrag == null) {
			return;
		}

		draggable = pointer.pointerDrag.GetComponent<Draggable>();
		if (draggable != null && draggable.getPlaceholderParent == transform) {
			draggable.setPlaceholderParent = draggable.getReturnParent;
		}
	}

	public void OnDrop (PointerEventData pointer) {

		draggable = pointer.pointerDrag.GetComponent<Draggable>();
		if (draggable != null) {
			if (cardType == draggable.cardtype && draggable.getPlaceholderParent.childCount - 1 < cardLimit) {
				draggable.setReturnParent = transform;

			}
		}
	}
}
