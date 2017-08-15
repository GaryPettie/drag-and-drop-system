using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawCard : MonoBehaviour, IPointerClickHandler {

	[SerializeField] private List<GameObject> cardList;
	DropZone hand;


	void Start ()  {
		hand = GameObject.FindGameObjectWithTag("HandArea").GetComponent<DropZone>();
	}

	public void OnPointerClick (PointerEventData pointer) {
		Debug.Log(hand.transform.childCount);
		if (hand.transform.childCount < hand.cardLimit && cardList.Count != 0) {
			int rand = Random.Range(0,cardList.Count);
			Debug.Log(rand);
			GameObject newCard = Instantiate(cardList[rand]);
			newCard.transform.SetParent(hand.transform);
			cardList.Remove(cardList[rand]);
		}
	}
}
