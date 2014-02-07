using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UMA;

public class UMACharacterCreator : MonoBehaviour 
{
	//Singleton
	private static UMACharacterCreator s_Instance;

	//Character sets (slots and overlays information)
	public UMACrowdRandomSet[] _CharacterSets;

	//UMA generator
	public UMAGenerator _Generator;

	//Atlas scale
	public float _AtlasResolutionScale = 1;

	//Libraries
	private RaceLibrary _RaceLibrary;
	private SlotLibrary _SlotLibrary;
	private OverlayLibrary _OverlayLibrary;

	//Current character
	private bool _IsMale = false;
	private bool _RandomDna = false;
	private bool _IsGenerating = true;

	private float _SkinToneValue;
	private UMAData _CharacterData = null;

	void Awake() 
	{
		//Assign instance
		s_Instance = this;

		//Find if libraries not assigned
		UMAContext context = UMAContext.FindInstance();
		if (context != null)
		{
			_RaceLibrary = context.raceLibrary;
			_SlotLibrary = context.slotLibrary;
			_OverlayLibrary = context.overlayLibrary;
		}

		//Create first one
		CreateUMA();
	}

	#region Static functions

	public static bool IsGeneratingCharacter()
	{
		if (s_Instance != null)
			return s_Instance._IsGenerating;
		else 
			return true;
	}

	public static bool IsCurrentCharacterMale()
	{
		if (s_Instance != null)
			return s_Instance._IsMale;
		else 
			return false;
	}

	public static void CreateCharacter(bool male, bool randomDna)
	{
		if (s_Instance != null)
		{
			s_Instance._IsMale = male;
			s_Instance._RandomDna = randomDna;

			s_Instance.CreateUMA();
		}
	}

	public static UMADnaHumanoid GetCharacterDna()
	{
		if (s_Instance != null && s_Instance._CharacterData != null)
		{
			UMADnaHumanoid umaDna = s_Instance._CharacterData.umaRecipe.umaDna[typeof(UMADnaHumanoid)] as UMADnaHumanoid;
			return umaDna;
		}

		return null;
	}

	public static float GetCharacterSkinTone()
	{
		if (s_Instance != null && s_Instance._CharacterData != null)
		{
			return s_Instance._SkinToneValue;
		}

		return 0f;
	}

	public static void ChangeCharacterSkinTone(float tone)
	{
		if (s_Instance != null)
			s_Instance.ChangeSkinTone(tone);
	}

	public static void ChangeCharacterShape()
	{
		if (s_Instance != null)
			s_Instance.ChangeShape();
	}

	#endregion

	#region UMA generation

	private void CreateUMA()
	{
		if (_CharacterData != null)
			Destroy(_CharacterData.gameObject);

		_IsGenerating = true;

		GameObject go = new GameObject("UMACharacter");
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

	private void ChangeSkinTone(float tone)
	{
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
			return;

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
	}

	private Color CalculateSkinTone(float tone)
	{
		float R = tone * 1.5f;
		float G = tone * 1.15f;
		
		if (R >= 255) R = 255f;
		
		Color skinColor = new Color(R / 255f, G / 255f, tone / 255f, 1);

		return skinColor;
	}

	private void ChangeShape()
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
