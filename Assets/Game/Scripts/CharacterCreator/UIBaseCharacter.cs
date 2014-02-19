using UnityEngine;
using System.Collections;

using UMA;

public class UIBaseCharacter : MonoBehaviour 
{
	private bool _IsInitialized = false;
	private bool _IsValueAssigned = false;

	private float _SkinToneValue;
	private UMADnaHumanoid _CharacterDna = null;

	private IEnumerator ChangeSkinTone(float tone)
	{
		yield return new WaitForSeconds(0.5f);

		UMACharacterCreator.ChangeCharacterSkinTone(tone);
	}

	private void FaceUpperDetail()
	{
		CameraZoom.onZoomFinished -= FaceUpperDetail;
		UICreatorManager.ChangeState(UICreatorState.FaceUpperDetail);
	}

	private void FaceMiddleDetail()
	{
		CameraZoom.onZoomFinished -= FaceMiddleDetail;
		UICreatorManager.ChangeState(UICreatorState.FaceMiddleDetail);
	}

	private void FaceLowerDetail()
	{
		CameraZoom.onZoomFinished -= FaceLowerDetail;
		UICreatorManager.ChangeState(UICreatorState.FaceLowerDetail);
	}

	private void Update()
	{
		if (!_IsInitialized)
		{
			if (!UMACharacterCreator.IsGeneratingCharacter())
			{
				_CharacterDna = UMACharacterCreator.GetCharacterDna();
				_SkinToneValue = UMACharacterCreator.GetCharacterSkinTone();

				_IsInitialized = true;
			}
		}
		else
		{
			if (!_IsValueAssigned && !UMACharacterCreator.IsGeneratingCharacter())
			{
				_CharacterDna = UMACharacterCreator.GetCharacterDna();
				_SkinToneValue = UMACharacterCreator.GetCharacterSkinTone();
				
				_IsValueAssigned = true;
			}
		}
	}

	private void OnGUI() 
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

		if (!_IsInitialized)
			return;

		GUILayout.BeginArea(boxInside);
		
		GUILayout.Space(20);
		
		GUILayout.BeginVertical();
		
		GUIStyle centeredTextStyle = new GUIStyle("label");
		centeredTextStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label("Base", centeredTextStyle);
		
		GUI.enabled = !UMACharacterCreator.IsGeneratingCharacter();
		
		if (GUILayout.Button("Change Character Slots", GUILayout.Height(25))) 
		{
			
		}
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Gender", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));

		bool isMale = UMACharacterCreator.IsCurrentCharacterMale();
		string gender = isMale ? "Male" : "Female";
		if (GUILayout.Button(gender, GUILayout.Height(25))) 
		{
			_IsValueAssigned = false;
			UMACharacterCreator.CreateCharacter(!isMale, false);
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Head Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHeadSize = GUILayout.HorizontalSlider(_CharacterDna.headSize, 0f, 1.0f, GUILayout.Height(10));
			if (newHeadSize != _CharacterDna.headSize)
			{
				_CharacterDna.headSize = newHeadSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Head Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHeadWidth = GUILayout.HorizontalSlider(_CharacterDna.headWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newHeadWidth != _CharacterDna.headWidth)
			{
				_CharacterDna.headWidth = newHeadWidth;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Upper Muscle", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newUpperMuscle = GUILayout.HorizontalSlider(_CharacterDna.upperMuscle, 0f, 1.0f, GUILayout.Height(10));
			if (newUpperMuscle != _CharacterDna.upperMuscle)
			{
				_CharacterDna.upperMuscle = newUpperMuscle;
				UMACharacterCreator.ChangeCharacterShape();
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Lower Weight", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLowerWeight = GUILayout.HorizontalSlider(_CharacterDna.lowerWeight, 0f, 1.0f, GUILayout.Height(10));
			if (newLowerWeight != _CharacterDna.lowerWeight)
			{
				_CharacterDna.lowerWeight = newLowerWeight;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Lower Muscle", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLowerMuscle = GUILayout.HorizontalSlider(_CharacterDna.lowerMuscle, 0f, 1.0f, GUILayout.Height(10));
			if (newLowerMuscle != _CharacterDna.lowerMuscle)
			{
				_CharacterDna.lowerMuscle = newLowerMuscle;
				UMACharacterCreator.ChangeCharacterShape();
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Belly", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newBelly = GUILayout.HorizontalSlider(_CharacterDna.belly, 0f, 1.0f, GUILayout.Height(10));
			if (newBelly != _CharacterDna.belly)
			{
				_CharacterDna.belly = newBelly;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Waist", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newWaist = GUILayout.HorizontalSlider(_CharacterDna.waist, 0f, 1.0f, GUILayout.Height(10));
			if (newWaist != _CharacterDna.waist)
			{
				_CharacterDna.waist = newWaist;
				UMACharacterCreator.ChangeCharacterShape();
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Arm Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newArmWidth = GUILayout.HorizontalSlider(_CharacterDna.armWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newArmWidth != _CharacterDna.armWidth)
			{
				_CharacterDna.armWidth = newArmWidth;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Hand Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newHandSize = GUILayout.HorizontalSlider(_CharacterDna.handsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newHandSize != _CharacterDna.handsSize)
			{
				_CharacterDna.handsSize = newHandSize;
				UMACharacterCreator.ChangeCharacterShape();
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Forearm Width", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newForearmWidth = GUILayout.HorizontalSlider(_CharacterDna.forearmWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newForearmWidth != _CharacterDna.forearmWidth)
			{
				_CharacterDna.forearmWidth = newForearmWidth;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Gluteus Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newGluteusSize = GUILayout.HorizontalSlider(_CharacterDna.gluteusSize, 0f, 1.0f, GUILayout.Height(10));
			if (newGluteusSize != _CharacterDna.gluteusSize)
			{
				_CharacterDna.gluteusSize = newGluteusSize;
				UMACharacterCreator.ChangeCharacterShape();
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
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Leg Size", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLegsSize = GUILayout.HorizontalSlider(_CharacterDna.legsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newLegsSize != _CharacterDna.legsSize)
			{
				_CharacterDna.legsSize = newLegsSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Leg Separation", GUILayout.Width(boxWidth / 4), GUILayout.Height(25));
			float newLegSeparation = GUILayout.HorizontalSlider(_CharacterDna.legSeparation, 0f, 1.0f, GUILayout.Height(10));
			if (newLegSeparation != _CharacterDna.legSeparation)
			{
				_CharacterDna.legSeparation = newLegSeparation;
				UMACharacterCreator.ChangeCharacterShape();
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginVertical();
			GUILayout.Label("Face Details", centeredTextStyle);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Upper", GUILayout.Height(22))) 
			{
				CameraZoom.Zoom("FacePosition");
				CameraZoom.onZoomFinished += FaceUpperDetail;
			}
			if (GUILayout.Button("Middle", GUILayout.Height(22))) 
			{
				CameraZoom.Zoom("FacePosition");
				CameraZoom.onZoomFinished += FaceMiddleDetail;
			}
			if (GUILayout.Button("Lower", GUILayout.Height(22))) 
			{
				CameraZoom.Zoom("FacePosition");
				CameraZoom.onZoomFinished += FaceLowerDetail;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
		
		GUILayout.FlexibleSpace();
		
		if (GUILayout.Button("Randomize DNA"))
		{
			_IsValueAssigned = false;
			UMACharacterCreator.CreateCharacter(UMACharacterCreator.IsCurrentCharacterMale(), true);
		}
		
		if (GUILayout.Button("Save Character", GUILayout.Height(28)))
		{
			
		}
		
		GUI.enabled = true;
		
		GUILayout.EndVertical();
		
		GUILayout.EndArea();
	}
}
