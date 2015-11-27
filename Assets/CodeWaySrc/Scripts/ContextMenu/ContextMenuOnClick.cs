/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ContextMenuOnClick : MonoBehaviour, IPointerClickHandler
{
	// elements (array of some structure to store the name & link (possible improvement: icon)
	[System.Serializable]
	public struct ContextMenuButton
	{
		public string uniqueButtonText;
		public Button.ButtonClickedEvent buttonBehavior;
		public string isSubmenuOf;
	}
	public ContextMenuButton[] contextMenuButtons;
	private Dictionary<string,RectTransform> contextMenuButonsSeeker;
	private Dictionary<string,List<string>> submenusReference;
	private string activeSubmenu = "main";

	// design
	[Tooltip("RectTransform of Context Menu Panel")]
	public RectTransform contextMenuRT;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public bool isVertical = true;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public bool isHorizontal = false;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsHeight;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsWidth;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsPaddingY;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsPaddingX;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsSpacingY;
	[Tooltip("Ignored if RT already has Grid/Vertical/Horizontal Layout Group Component.")]
	public int buttonsSpacingX;

	// drag event detection
	private const int MOUSE_LEFT = 0; // mouse button left
	private const int MOUSE_RIGHT = 1; // mouse button right
	private const int MOUSE_MIDDLE = 2; // mouse button middle
	private bool mouseDown = false; // isDragging
	private Vector3 startPos; // drag start pos
	private double sensitivity = 0.07; // minimum drag to trigger the action

	// layout group types
	private const int TYPE_GRID = 0;
	private const int TYPE_VERTICAL = 1;
	private const int TYPE_HORIZONTAL = 2;
	//private int type = -1;
	
	// layout groups
	private GridLayoutGroup grid;
	private VerticalLayoutGroup vertical;
	private HorizontalLayoutGroup horizontal;

	// button prefab
	public Button modelButton;
	[Tooltip("Ignored if RT already has Model Button.")]
	public Color buttonColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	[Tooltip("Ignored if RT already has Model Button.")]
	public Sprite buttonSprite;
	[Tooltip("Ignored if RT already has Model Button.")]
	public Color textColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	[Tooltip("Ignored if RT already has Model Button.")]
	public Font textFont;
	[Tooltip("Ignored if RT already has Model Button.")]
	public int fontSize = 24;

	// animation
	public int animationLayer = 0;

	private bool initialized = false;

	void Awake () 
	{
		if (contextMenuRT == null)
			CreateContextMenuRT();
		if (contextMenuButonsSeeker == null)
			contextMenuButonsSeeker = new Dictionary<string, RectTransform>();
		if (submenusReference == null)
			submenusReference = new Dictionary<string, List<string>>();
		contextMenuRT.gameObject.SetActive(true);
		if (rtHasLayoutGroup() == false)
			CreateLayoutGroup();
		if (buttonsAlreadyExist() == false)
		{
			if (modelButton == null)
				EnsureModelButton();
			CreateButtons();
		}
		//TODO: else registerLegacyButtonsInSeeker() && createSubmenusHierarchy();
		initialized = true;
		contextMenuRT.gameObject.SetActive(false);
	}

	void Update ()
	{
		TriggerMouseDragCallbacks();
	}
	
	private void OnDragToRight ()
	{
		Transform objT = this.gameObject.GetComponent<Transform>();
		objT.localScale = new Vector3(objT.localScale.x + 5.0f, objT.localScale.y + 5.0f, objT.localScale.z + 5.0f);
	}
	
	private void OnDragToLeft ()
	{
		Transform objT = this.gameObject.GetComponent<Transform>();
		objT.localScale = new Vector3(objT.localScale.x + 5.0f, objT.localScale.y + 5.0f, objT.localScale.z + 5.0f);
	}

	public void ContextMenuButtonClicked ()
	{
		if (!contextMenuRT.gameObject.activeSelf)
		{
			contextMenuRT.gameObject.SetActive(true);
			OpenSubmenu("main");
			return;
		}
		Animator animator = contextMenuRT.gameObject.GetComponent<Animator>();
		if (animator != null)
			StartCoroutine(AnimateAndDeactivate(animator,"MenuExitAnimation"));
		else
			contextMenuRT.gameObject.SetActive(false);
	}
	
	public virtual void OnPointerClick (PointerEventData eventData)
	{
		ContextMenuButtonClicked();
	}

	public bool isInitialized ()
	{
		return initialized;
	}

	public List<RectTransform> GetContextMenuButtonsRTs()
	{
		List<RectTransform> list = new List<RectTransform>();
		foreach (string key in contextMenuButonsSeeker.Keys)
			list.Add(contextMenuButonsSeeker[key]);
		return list;
	}

	private IEnumerator AnimateAndDeactivate (Animator animator, string animName)
	{
		animator.Play(animName,animationLayer);
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(animationLayer).length);
		animator.gameObject.SetActive(false);
	}

	private IEnumerator WaitForAnimation (Animation animation)
	{
		do
		{
			yield return null;
		} while (animation.isPlaying);
	}

	private void CreateContextMenuRT ()
	{
		GameObject contextMenuGO = new GameObject();
		contextMenuGO.name = "DynamicContextMenu";
		contextMenuRT = contextMenuGO.AddComponent<RectTransform>();
		Transform thisObjectTransform = this.gameObject.GetComponent<Transform>();
		if (thisObjectTransform != null)
		{
			contextMenuRT.SetParent(thisObjectTransform);
			contextMenuRT.anchorMin = new Vector2(0.0f,0.0f);
			contextMenuRT.anchorMax = new Vector2(1.0f,1.0f);
			contextMenuRT.offsetMin = new Vector2(0.0f,0.0f);
			contextMenuRT.offsetMax = new Vector2(0.0f,0.0f);
		}
		else
		{
			Debug.LogError("CreateContextMenu must be attached at element with Transform component!");
		}
		contextMenuRT.localScale = new Vector3(1.0f,1.0f,1.0f);
	}

	private void TriggerMouseDragCallbacks ()
	{
		if (Input.GetMouseButtonDown(MOUSE_LEFT) && PositionIsInsideRT(Input.mousePosition,contextMenuRT))
		{
			startPos = Input.mousePosition;
			mouseDown = true;
		}
		if (Input.GetMouseButtonUp(MOUSE_LEFT) && mouseDown)
		{
			mouseDown = false;
			if ((Input.mousePosition.x - startPos.x) > sensitivity)
				OnDragToRight();
			else if ((startPos.x - Input.mousePosition.x) > sensitivity)
				OnDragToLeft();
		}
	}
	
	private bool PositionIsInsideRT (Vector3 position, RectTransform rt)
	{
		if (position.x < (rt.position.x - rt.sizeDelta.x/2))
			return false;
		if (position.x > (rt.position.x + rt.sizeDelta.x/2))
			return false;
		if (position.y < (rt.position.y - rt.sizeDelta.y/2))
			return false;
		if (position.y > (rt.position.y + rt.sizeDelta.y/2))
			return false;
		return true;
	}

	private bool rtHasLayoutGroup ()
	{
		GridLayoutGroup grid = contextMenuRT.gameObject.GetComponent<GridLayoutGroup> ();
		if (grid != null)
		{
			//type = TYPE_GRID;
			return true;
		}
		VerticalLayoutGroup vertical = contextMenuRT.gameObject.GetComponent<VerticalLayoutGroup> ();
		if (vertical != null)
		{
			//type = TYPE_VERTICAL;
			return true;
		}
		HorizontalLayoutGroup horizontal = contextMenuRT.gameObject.GetComponent<HorizontalLayoutGroup> ();
		if (horizontal != null)
		{
			//type = TYPE_HORIZONTAL;
			return true;
		}
		return false;
	}

	private void CreateLayoutGroup()
	{
		if (isVertical) 
		{
			if (isHorizontal)
			{
				grid = contextMenuRT.gameObject.AddComponent<GridLayoutGroup>();
				//type = TYPE_GRID;
				grid.spacing = new Vector2(buttonsSpacingX,buttonsSpacingY);
				grid.padding = new RectOffset(buttonsPaddingX, buttonsPaddingX, buttonsPaddingY, buttonsPaddingY);
				grid.cellSize = new Vector2(buttonsWidth,buttonsHeight);
				return;
			}
			vertical = contextMenuRT.gameObject.AddComponent<VerticalLayoutGroup>();
			//type = TYPE_VERTICAL;
			vertical.spacing = buttonsSpacingY;
			vertical.padding = new RectOffset(buttonsPaddingX, buttonsPaddingX, buttonsPaddingY, buttonsPaddingY);
			vertical.childForceExpandWidth = true;
			vertical.childForceExpandHeight = false;
			return;
		}
		if (isHorizontal)
		{
			horizontal = contextMenuRT.gameObject.AddComponent<HorizontalLayoutGroup>();
			//type = TYPE_HORIZONTAL;
			vertical.spacing = buttonsSpacingX;
			horizontal.padding = new RectOffset(buttonsPaddingX, buttonsPaddingX, buttonsPaddingY, buttonsPaddingY);
			horizontal.childForceExpandWidth = true;
			horizontal.childForceExpandHeight = false;
			return;
		}
	}

	private bool buttonsAlreadyExist ()
	{
		Button[] buttons = contextMenuRT.gameObject.GetComponentsInChildren<Button>();
		if (buttons.Length >= 2)
			return true;
		if (buttons.Length == 1 && buttons[0].gameObject != this.gameObject && !buttons[0].gameObject.name.Contains("ModelButton"))
			return true;
		return false;
	}
	
	private void EnsureModelButton () 
	{
		modelButton = contextMenuRT.gameObject.GetComponentInChildren<Button>();
		if (modelButton == null)
		{
			GameObject modelButtonGO = new GameObject();
			modelButtonGO.name = "ModelButton";
			// Configure Rect Transform.
			RectTransform modelButtonRT = modelButtonGO.AddComponent<RectTransform>();
			Transform ContextMenuObjectTransform = contextMenuRT.gameObject.GetComponent<Transform>();
			if (ContextMenuObjectTransform != null) {
				modelButtonRT.SetParent(ContextMenuObjectTransform);
				modelButtonRT.anchorMin = new Vector2(0.0f, 0.0f);
				modelButtonRT.anchorMax = new Vector2(1.0f, 1.0f);
				modelButtonRT.offsetMin = new Vector2(0.0f, 0.0f);
				modelButtonRT.offsetMax = new Vector2(0.0f, 0.0f);
				modelButtonRT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				float x = modelButtonRT.localPosition.x;
				float y = modelButtonRT.localPosition.y;
				modelButtonRT.localPosition = new Vector3(x, y, 0);
			} else {
				Debug.LogError ("ModelButton must be attached at element with Transform component!");
			}
			modelButton = modelButtonGO.AddComponent<Button>();
			// Configure Button Image.
			Image buttonImg = modelButtonGO.AddComponent<Image>();
			buttonImg.type = Image.Type.Sliced;
			buttonImg.color = buttonColor;
			//TODO: ask Gui how to load UISprite dinamically.
			//Sprite sp = Resources.Load("Images/icon_menu") as Sprite;
			//if (sp == null) Debug.Log("sp null");
			//else Debug.Log("ok");
			//Object obj = Resources.Load("Images/icon_menu");
			//if (obj == null) Debug.Log("obj null");
			//else Debug.Log("obj ok");
			//if (buttonSprite == null)
			//buttonSprite = Resources.GetBuiltinResource(typeof(Sprite), "UISprite") as Sprite;
			buttonImg.sprite = buttonSprite;
			//Configure Text.
			GameObject textGO = new GameObject();
			textGO.name = "ModelButtonText";
			RectTransform textRT = textGO.AddComponent<RectTransform>();
			textRT.SetParent(modelButtonRT);
			textRT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			textRT.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
			textRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
			textRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
			Text textTxt = textGO.AddComponent<Text>();
			textTxt.text = "Button";
			if (textFont == null)
				textFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			textTxt.font = textFont;
			textTxt.fontSize = fontSize;
			textTxt.alignment = TextAnchor.MiddleCenter;
			textTxt.fontStyle = FontStyle.Bold;
			textTxt.color = textColor;
		}
		if ((!isVertical) && (!isHorizontal))
			return;
		if (modelButton == null)
			Debug.Log("Model Button is null!");
		if (modelButton.gameObject == null)
			Debug.Log("Model Button Game Object is null!");
		LayoutElement layoutElement = modelButton.gameObject.AddComponent<LayoutElement>();
		if (isVertical) 
			layoutElement.preferredHeight = buttonsHeight;
		if (isHorizontal) 
			layoutElement.preferredWidth = buttonsWidth;
	}
	
	private void CreateButtons ()
	{
		modelButton.gameObject.SetActive(true);
		RectTransform modelButtonRT = modelButton.GetComponent<RectTransform>();
		foreach (ContextMenuButton cmButton in contextMenuButtons)
		{
			GameObject GO = Instantiate(modelButton.gameObject) as GameObject;
			RectTransform RT = GO.GetComponent<RectTransform>();
			RT.SetParent(modelButtonRT.parent);
			RT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			RT.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
			Text TXT = GO.GetComponentInChildren<Text>();
			TXT.text = cmButton.uniqueButtonText;
			Button BTN = GO.GetComponent<Button>();
			BTN.onClick = cmButton.buttonBehavior;
			if (cmButton.uniqueButtonText.Length == 0)
				Debug.LogError("ContextMenuOnClick(): uniqueButtonText cannot be empty.");
			else
				contextMenuButonsSeeker.Add(cmButton.uniqueButtonText,RT);
			if (cmButton.isSubmenuOf.Length > 0)
			{
				RegisterButtonInSubmenu(cmButton.uniqueButtonText,cmButton.isSubmenuOf);
				if (!cmButton.isSubmenuOf.Equals("main"))
					RT.gameObject.SetActive(false);
			}
			else
				RegisterButtonInSubmenu(cmButton.uniqueButtonText,"main");
		}
		if (submenusReference.ContainsKey("main"))
			GenerateOpenSubmenuTriggers("main");
		else
			Debug.LogError("ContextMenuOnClick(): All menus are submenus, main menu does not exist.");
		modelButton.gameObject.SetActive(false);
	}

	private void RegisterButtonInSubmenu (string buttonName, string submenuName)
	{
		if (!submenusReference.ContainsKey(submenuName))
		{
			submenusReference.Add(submenuName, new List<string>());
		}
		List<string> submenuButons = submenusReference[submenuName];
		submenuButons.Add(buttonName);
	}
	
	private void GenerateOpenSubmenuTriggers (string submenuName)
	{
		if (!submenusReference.ContainsKey (submenuName))
			return;
		UnityAction openSubmenu = () => OpenSubmenu(submenuName);
		if (!submenuName.Equals("main"))
			contextMenuButonsSeeker[submenuName].GetComponent<Button>().onClick.AddListener(openSubmenu);
		foreach (string subButton in submenusReference[submenuName])
			GenerateOpenSubmenuTriggers(subButton);
	}

	private void OpenSubmenu (string submenuName)
	{
		if (!submenusReference.ContainsKey(submenuName))
		{
			Debug.LogError("Submenu " + submenuName + " does not exist.");
			return;
		}
		foreach (string toDeactivate in submenusReference[activeSubmenu])
			contextMenuButonsSeeker[toDeactivate].gameObject.SetActive(false);
		foreach (string toActivate in submenusReference[submenuName])
			contextMenuButonsSeeker[toActivate].gameObject.SetActive(true);
		activeSubmenu = submenuName;
	}
}

//TODO: implement constraints for GridLayoutGroup.
//TODO: implement uniqueButtonName redundancy safety.
//TODO: implement a better oredernation solution.
//TODO: uniqueButton -> uniqueName or uniqueButtonName.