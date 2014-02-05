using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UMA;

public class UMACharacterCreator : MonoBehaviour 
{
	//Character sets (slots and overlays information)
	public UMACrowdRandomSet[] _CharacterSets;

	//Libraries
	public RaceLibrary _RaceLibrary;
	public SlotLibrary _SlotLibrary;
	public OverlayLibrary _OverlayLibrary;

	public UMAGenerator _Generator;

	public float _AtlasResolutionScale = 1;

	//Current character
	private bool _IsMale = false;
	private bool _RandomDna = false;
	private UMAData _CharacterData = null;
	private UMADnaHumanoid _CharacterDna = null;
	private bool _IsGenerating = true;
	private float _SkinToneValue;

	void Awake() 
	{
		//Find if libraries not assigned
		UMAContext context = UMAContext.FindInstance();
		if (context != null)
		{
			if (_RaceLibrary == null) 
				_RaceLibrary = context.raceLibrary;
			
			if (_SlotLibrary == null) 
				_SlotLibrary = context.slotLibrary;
			
			if (_OverlayLibrary == null) 
				_OverlayLibrary = context.overlayLibrary;
		}

		//Create first one
		CreateUMA(_IsMale);
	}

	void OnGUI() 
	{
		float startX = Screen.width / 18;
		float startY = Screen.height / 18;
		float width = Screen.width / 18 * 8;
		float height = Screen.height / 18 * 16;

		Rect box = new Rect(startX, startY, width, height);  
		GUI.Box(box, "Character Creator");

		float boxWidth = width - 20;
		float boxHeight = height - 8;

		Rect boxInside = new Rect(startX + 10, startY, boxWidth, boxHeight); 
		GUILayout.BeginArea(boxInside);

		GUILayout.Space(20);

		GUILayout.BeginVertical();

		GUIStyle centeredTextStyle = new GUIStyle("label");
		centeredTextStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label("Base", centeredTextStyle);

		GUI.enabled = !_IsGenerating;

		if (GUILayout.Button("Change Character Slots", GUILayout.Height(25))) 
		{
			
		}

		GUILayout.BeginHorizontal();

		GUILayout.Label("Gender", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));

		string gender = _IsMale ? "Male" : "Female";
		if (GUILayout.Button(gender, GUILayout.Height(25))) 
		{
			_IsMale = !_IsMale;

			_RandomDna = false;
			CreateUMA(_IsMale);
		}

		GUILayout.EndHorizontal();

		GUILayout.Label("Skin Tone", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));

		float toneValue = GUILayout.HorizontalSlider(_SkinToneValue, 30f, 200f, GUILayout.Height(10));
		if (toneValue != _SkinToneValue)
		{
			_SkinToneValue = toneValue;

			StopAllCoroutines();
			StartCoroutine(ChangeSkinTone(_SkinToneValue));
		}

		if (_CharacterDna != null)
		{
			GUILayout.Label("Height", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHeight = GUILayout.HorizontalSlider(_CharacterDna.height, 0f, 1.0f, GUILayout.Height(10));
			if (newHeight != _CharacterDna.height)
			{
				_CharacterDna.height = newHeight;
				ChangeCharacterShape();
			}

			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Head Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHeadSize = GUILayout.HorizontalSlider(_CharacterDna.headSize, 0f, 1.0f, GUILayout.Height(10));
			if (newHeadSize != _CharacterDna.headSize)
			{
				_CharacterDna.headSize = newHeadSize;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Head Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHeadWidth = GUILayout.HorizontalSlider(_CharacterDna.headWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newHeadWidth != _CharacterDna.headWidth)
			{
				_CharacterDna.headWidth = newHeadWidth;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Upper Muscle", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newUpperMuscle = GUILayout.HorizontalSlider(_CharacterDna.upperMuscle, 0f, 1.0f, GUILayout.Height(10));
			if (newUpperMuscle != _CharacterDna.upperMuscle)
			{
				_CharacterDna.upperMuscle = newUpperMuscle;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Upper Weight", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newUpperWeight = GUILayout.HorizontalSlider(_CharacterDna.upperWeight, 0f, 1.0f, GUILayout.Height(10));
			if (newUpperWeight != _CharacterDna.upperWeight)
			{
				_CharacterDna.upperWeight = newUpperWeight;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Lower Weight", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLowerWeight = GUILayout.HorizontalSlider(_CharacterDna.lowerWeight, 0f, 1.0f, GUILayout.Height(10));
			if (newLowerWeight != _CharacterDna.lowerWeight)
			{
				_CharacterDna.lowerWeight = newLowerWeight;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Lower Muscle", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLowerMuscle = GUILayout.HorizontalSlider(_CharacterDna.lowerMuscle, 0f, 1.0f, GUILayout.Height(10));
			if (newLowerMuscle != _CharacterDna.lowerMuscle)
			{
				_CharacterDna.lowerMuscle = newLowerMuscle;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Breast", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newBreast = GUILayout.HorizontalSlider(_CharacterDna.breastSize, 0f, 1.0f, GUILayout.Height(10));
			if (newBreast != _CharacterDna.breastSize)
			{
				_CharacterDna.breastSize = newBreast;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Belly", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newBelly = GUILayout.HorizontalSlider(_CharacterDna.belly, 0f, 1.0f, GUILayout.Height(10));
			if (newBelly != _CharacterDna.belly)
			{
				_CharacterDna.belly = newBelly;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Label("Waist", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newWaist = GUILayout.HorizontalSlider(_CharacterDna.waist, 0f, 1.0f, GUILayout.Height(10));
			if (newWaist != _CharacterDna.waist)
			{
				_CharacterDna.waist = newWaist;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Arm Length", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newArmLength = GUILayout.HorizontalSlider(_CharacterDna.armLength, 0f, 1.0f, GUILayout.Height(10));
			if (newArmLength != _CharacterDna.armLength)
			{
				_CharacterDna.armLength = newArmLength;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Arm Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newArmWidth = GUILayout.HorizontalSlider(_CharacterDna.armWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newArmWidth != _CharacterDna.armWidth)
			{
				_CharacterDna.armWidth = newArmWidth;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Hand Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHandSize = GUILayout.HorizontalSlider(_CharacterDna.handsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newHandSize != _CharacterDna.handsSize)
			{
				_CharacterDna.handsSize = newHandSize;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Forearm Length", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newForearmLength = GUILayout.HorizontalSlider(_CharacterDna.forearmLength, 0f, 1.0f, GUILayout.Height(10));
			if (newForearmLength != _CharacterDna.forearmLength)
			{
				_CharacterDna.forearmLength = newForearmLength;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Forearm Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newForearmWidth = GUILayout.HorizontalSlider(_CharacterDna.forearmWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newForearmWidth != _CharacterDna.forearmWidth)
			{
				_CharacterDna.forearmWidth = newForearmWidth;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Gluteus Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newGluteusSize = GUILayout.HorizontalSlider(_CharacterDna.gluteusSize, 0f, 1.0f, GUILayout.Height(10));
			if (newGluteusSize != _CharacterDna.gluteusSize)
			{
				_CharacterDna.gluteusSize = newGluteusSize;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Feet Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newFeetSize = GUILayout.HorizontalSlider(_CharacterDna.feetSize, 0f, 1.0f, GUILayout.Height(10));
			if (newFeetSize != _CharacterDna.feetSize)
			{
				_CharacterDna.feetSize = newFeetSize;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Leg Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLegsSize = GUILayout.HorizontalSlider(_CharacterDna.legsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newLegsSize != _CharacterDna.legsSize)
			{
				_CharacterDna.legsSize = newLegsSize;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Leg Separation", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLegSeparation = GUILayout.HorizontalSlider(_CharacterDna.legSeparation, 0f, 1.0f, GUILayout.Height(10));
			if (newLegSeparation != _CharacterDna.legSeparation)
			{
				_CharacterDna.legSeparation = newLegSeparation;
				ChangeCharacterShape();
			}
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();

			GUILayout.BeginVertical();
			GUILayout.Label("Face Details", centeredTextStyle);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Upper", GUILayout.Height(22))) 
			{
				
			}
			if (GUILayout.Button("Middle", GUILayout.Height(22))) 
			{
				
			}
			if (GUILayout.Button("Lower", GUILayout.Height(22))) 
			{
				
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		GUILayout.FlexibleSpace();

		if (GUILayout.Button("Randomize"))
		{
			_RandomDna = true;
			CreateUMA(_IsMale);
		}

		if (GUILayout.Button("Save Character", GUILayout.Height(28)))
		{

		}

		GUI.enabled = true;

		GUILayout.EndVertical();

		GUILayout.EndArea();
	}

	#region UMA generation

	private void CreateUMA(bool male)
	{
		if (_CharacterData != null)
			Destroy(_CharacterData.gameObject);

		_IsGenerating = true;

		GameObject go = new GameObject("Character");
		go.transform.parent = transform;

		UMADynamicAvatar umaDynamicAvatar = go.AddComponent<UMADynamicAvatar>();
		umaDynamicAvatar.Initialize();

		_CharacterData = umaDynamicAvatar.umaData;
		umaDynamicAvatar.umaGenerator = _Generator;
		_CharacterData.umaGenerator = _Generator;

		UMAData.UMARecipe umaRecipe = umaDynamicAvatar.umaData.umaRecipe;

		UMACrowdRandomSet.CrowdRaceData race = null;
		if (_CharacterSets != null && _CharacterSets.Length > 0)
		{
			int raceIndex = (_IsMale) ? 1 : 0;
			race = _CharacterSets[raceIndex].data;
			umaRecipe.SetRace(_RaceLibrary.GetRace(race.raceID));
		}

		SetUMAData();

		GenerateUMAShapes();
		
		if (race != null && race.slotElements.Length > 0)
		{
			DefineSlots(race);
		}
		
		umaDynamicAvatar.UpdateNewRace();
		umaDynamicAvatar.umaData.myRenderer.enabled = false;

		go.transform.position = new Vector3(0, 0, 0);
	}

	private void SetUMAData()
	{
		_CharacterData.OnUpdated += updateCharacterData;

		_CharacterData.atlasResolutionScale = _AtlasResolutionScale;
		_CharacterData.Dirty(true, true, true);
	}

	private void updateCharacterData(UMAData umaData){
		//Update its collider
		CapsuleCollider tempCollider = umaData.GetComponentInChildren<CapsuleCollider>();
		if (tempCollider)
		{
			UMADnaHumanoid umaDna = umaData.umaRecipe.umaDna[typeof(UMADnaHumanoid)] as UMADnaHumanoid;
			tempCollider.height = (umaDna.height + 0.5f) * 2 + 0.1f;
			tempCollider.center = new Vector3(0,tempCollider.height * 0.5f - 0.04f,0);

			_CharacterDna = umaDna;
		}

		//Add mouse rotate for demo
		CharacterRotate rotate = umaData.gameObject.GetComponent<CharacterRotate>();
		if (rotate == null)
			umaData.gameObject.AddComponent<CharacterRotate>();

		//Disable locomotion
		Locomotion loco = umaData.gameObject.GetComponentInChildren<Locomotion>();
		if (loco != null)
			loco.enabled = false;

		//Enable UI again
		_IsGenerating = false;
	}

	private void GenerateUMAShapes()
	{
		UMADnaHumanoid umaDna = new UMADnaHumanoid();
		_CharacterData.umaRecipe.umaDna.Add(umaDna.GetType(),umaDna);

		if(_RandomDna)
		{
			umaDna.height = Random.Range(0.3f,0.5f);
			umaDna.headSize = Random.Range(0.485f,0.515f);
			umaDna.headWidth = Random.Range(0.4f,0.6f);

			umaDna.neckThickness = Random.Range(0.495f,0.51f);
			
			if (_CharacterData.umaRecipe.raceData.raceName == "HumanMale")
			{
				umaDna.handsSize = Random.Range(0.485f,0.515f);
				umaDna.feetSize = Random.Range(0.485f,0.515f);
				umaDna.legSeparation = Random.Range(0.4f,0.6f);
				umaDna.waist = 0.5f;
			}
			else
			{
				umaDna.handsSize = Random.Range(0.485f,0.515f);
				umaDna.feetSize = Random.Range(0.485f,0.515f);
				umaDna.legSeparation = Random.Range(0.485f,0.515f);
				umaDna.waist = Random.Range(0.3f,0.8f);
			}
			
			umaDna.armLength = Random.Range(0.485f,0.515f);
			umaDna.forearmLength = Random.Range(0.485f,0.515f);
			umaDna.armWidth = Random.Range(0.3f,0.8f);
			umaDna.forearmWidth = Random.Range(0.3f,0.8f);
			
			umaDna.upperMuscle = Random.Range(0.0f,1.0f);
			umaDna.upperWeight = Random.Range(-0.2f,0.2f) + umaDna.upperMuscle;
			if(umaDna.upperWeight > 1.0){ umaDna.upperWeight = 1.0f;}
			if(umaDna.upperWeight < 0.0){ umaDna.upperWeight = 0.0f;}
			
			umaDna.lowerMuscle = Random.Range(-0.2f,0.2f) + umaDna.upperMuscle;
			if(umaDna.lowerMuscle > 1.0){ umaDna.lowerMuscle = 1.0f;}
			if(umaDna.lowerMuscle < 0.0){ umaDna.lowerMuscle = 0.0f;}
			
			umaDna.lowerWeight = Random.Range(-0.1f,0.1f) + umaDna.upperWeight;
			if(umaDna.lowerWeight > 1.0){ umaDna.lowerWeight = 1.0f;}
			if(umaDna.lowerWeight < 0.0){ umaDna.lowerWeight = 0.0f;}
			
			umaDna.belly = umaDna.upperWeight;
			umaDna.legsSize = Random.Range(0.4f,0.6f);
			umaDna.gluteusSize = Random.Range(0.4f,0.6f);
			
			umaDna.earsSize = Random.Range(0.3f,0.8f);
			umaDna.earsPosition = Random.Range(0.3f,0.8f);
			umaDna.earsRotation = Random.Range(0.3f,0.8f);
			
			umaDna.noseSize = Random.Range(0.3f,0.8f);
			
			umaDna.noseCurve = Random.Range(0.3f,0.8f);
			umaDna.noseWidth = Random.Range(0.3f,0.8f);
			umaDna.noseInclination = Random.Range(0.3f,0.8f);
			umaDna.nosePosition = Random.Range(0.3f,0.8f);
			umaDna.nosePronounced = Random.Range(0.3f,0.8f);
			umaDna.noseFlatten = Random.Range(0.3f,0.8f);
			
			umaDna.chinSize = Random.Range(0.3f,0.8f);
			umaDna.chinPronounced = Random.Range(0.3f,0.8f);
			umaDna.chinPosition = Random.Range(0.3f,0.8f);
			
			umaDna.mandibleSize = Random.Range(0.45f,0.52f);
			umaDna.jawsSize = Random.Range(0.3f,0.8f);
			umaDna.jawsPosition = Random.Range(0.3f,0.8f);
			
			umaDna.cheekSize = Random.Range(0.3f,0.8f);
			umaDna.cheekPosition = Random.Range(0.3f,0.8f);
			umaDna.lowCheekPronounced = Random.Range(0.3f,0.8f);
			umaDna.lowCheekPosition = Random.Range(0.3f,0.8f);
			
			umaDna.foreheadSize = Random.Range(0.3f,0.8f);
			umaDna.foreheadPosition = Random.Range(0.15f,0.65f);
			
			umaDna.lipsSize = Random.Range(0.3f,0.8f);
			umaDna.mouthSize = Random.Range(0.3f,0.8f);
			umaDna.eyeRotation = Random.Range(0.3f,0.8f);
			umaDna.eyeSize = Random.Range(0.3f,0.8f);
			umaDna.breastSize = Random.Range(0.3f,0.8f);
		}
	}

	private void DefineSlots(UMACrowdRandomSet.CrowdRaceData race)
	{
		float skinTone = Random.Range(30f, 200f);
		Color skinColor = CalculateSkinTone(skinTone);
		_SkinToneValue = skinTone;

		Color HairColor = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1);

		var slotParts = new HashSet<string>();
		_CharacterData.umaRecipe.slotDataList = new SlotData[race.slotElements.Length];
		for (int i = 0; i < race.slotElements.Length; i++)
		{
			var currentElement = race.slotElements[i];

			if (!string.IsNullOrEmpty(currentElement.requirement) && !slotParts.Contains(currentElement.requirement)) continue;
			if (currentElement.possibleSlots.Length == 0) continue;

			int randomResult = Random.Range(0, currentElement.possibleSlots.Length);

			var slot = currentElement.possibleSlots[randomResult];
			if (string.IsNullOrEmpty(slot.slotID)) continue;

			slotParts.Add(slot.slotID);
			SlotData slotData;

			if (slot.useSharedOverlayList && slot.overlayListSource >= 0 && slot.overlayListSource < i)
			{
				slotData = _SlotLibrary.InstantiateSlot(slot.slotID, _CharacterData.umaRecipe.slotDataList[slot.overlayListSource].GetOverlayList());
			}
			else
			{
				if (slot.useSharedOverlayList)
					Debug.LogError("UMA Crowd: Invalid overlayListSource for " + slot.slotID);
				slotData = _SlotLibrary.InstantiateSlot(slot.slotID);
			}

			_CharacterData.umaRecipe.slotDataList[i] = slotData;
			for (int overlayIdx = 0; overlayIdx < slot.overlayElements.Length; overlayIdx++)
			{
				var currentOverlayElement = slot.overlayElements[overlayIdx];
				randomResult = Random.Range(0, currentOverlayElement.possibleOverlays.Length);

				var overlay = currentOverlayElement.possibleOverlays[randomResult];
				if (string.IsNullOrEmpty(overlay.overlayID)) continue;

				slotParts.Add(overlay.overlayID);

				Color overlayColor;
				if (overlay.useSkinColor)
				{
					overlayColor = skinColor + new Color(Random.Range(overlay.minRGB.r, overlay.maxRGB.r), Random.Range(overlay.minRGB.g, overlay.maxRGB.g), Random.Range(overlay.minRGB.b, overlay.maxRGB.b), 1);
				}
				else if (overlay.useHairColor)
				{
					overlayColor = HairColor * overlay.hairColorMultiplier;
				}
				else
				{
					overlayColor = new Color(Random.Range(overlay.minRGB.r, overlay.maxRGB.r), Random.Range(overlay.minRGB.g, overlay.maxRGB.g), Random.Range(overlay.minRGB.b, overlay.maxRGB.b), 1);
				}
				slotData.AddOverlay(_OverlayLibrary.InstantiateOverlay(overlay.overlayID, overlayColor));				
			}

			if (_CharacterData.umaRecipe.slotDataList[i].GetOverlayList().Count == 0)
			{
				Debug.LogError("Slot without overlay: " + _CharacterData.umaRecipe.slotDataList[i].slotName+" at index "+i+" of race: "+race.raceID);
			}
		}
	}

	#endregion

	#region Base changer

	private IEnumerator ChangeSkinTone(float tone)
	{
		yield return new WaitForSeconds(0.5f);

		//Check the race first
		string name = _CharacterData.myRenderer.name;
		UMACrowdRandomSet characterSet = null;
		if (name.Contains("Human"))
		{
			if (name.Contains("Male"))
				characterSet = _CharacterSets[1];
			else if (name.Contains("Female"))
				characterSet = _CharacterSets[0];
		}

		//Return if non-exists
		if (characterSet == null)
			yield break;

		//New Color
		Color skinColor = CalculateSkinTone(tone);

		//Enable change
		_CharacterData.isTextureDirty = true;
		_CharacterData.Dirty();

		//Iterate overlay from slots
		foreach(SlotData slotData in _CharacterData.umaRecipe.slotDataList)
		{
			if (slotData != null)
			{
				UMACrowdRandomSet.CrowdSlotData crowdSlotData = SlotDataFromSet(characterSet, slotData.slotName);
				
				int len = slotData.OverlayCount;
				for(int i=0;i<len;i++)
				{
					OverlayData overlay = slotData.GetOverlay(i);
					UMACrowdRandomSet.CrowdOverlayData overlayData = OverlayDataFromSlot(crowdSlotData, overlay.overlayName);
					
					if (overlayData != null && overlayData.useSkinColor)
						overlay.SetColor(0, skinColor);
				}
			}
		}

		yield return null;
	}

	private Color CalculateSkinTone(float tone)
	{
		float R = tone * 1.5f;
		float G = tone * 1.15f;
		
		if (R >= 255) R = 255f;
		
		Color skinColor = new Color(R / 255f, G / 255f, tone / 255f, 1);

		return skinColor;
	}

	private void ChangeCharacterShape()
	{
		_CharacterData.isShapeDirty = true;
		_CharacterData.Dirty();
	}

	#endregion

	#region Getter from character set

	private UMACrowdRandomSet.CrowdSlotData SlotDataFromSet(UMACrowdRandomSet set, string slotid)
	{
		foreach(UMACrowdRandomSet.CrowdSlotElement crowdElement in set.data.slotElements)
		{
			if (crowdElement != null)
			{
				foreach(UMACrowdRandomSet.CrowdSlotData slotData in crowdElement.possibleSlots)
				{
					if (slotData != null && slotData.slotID == slotid)
						return slotData;
				}
			}
		}

		return null;
	}

	private UMACrowdRandomSet.CrowdOverlayData OverlayDataFromSlot(UMACrowdRandomSet.CrowdSlotData slot, string overlayid)
	{
		foreach(UMACrowdRandomSet.CrowdOverlayElement overlayElement in slot.overlayElements)
		{
			if (overlayElement != null)
			{
				foreach(UMACrowdRandomSet.CrowdOverlayData overlayData in overlayElement.possibleOverlays)
				{
					if (overlayData != null && overlayData.overlayID == overlayid)
						return overlayData;
				}
			}
		}

		return null;
	}

	#endregion
}
