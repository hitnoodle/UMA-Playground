using UnityEngine;
using System.Collections;

using UMA;

public class UIFaceMiddleDetail : MonoBehaviour 
{
	private bool _IsInitialized = false;
	
	private UMADnaHumanoid _CharacterDna = null;
	
	private void ZoomBack()
	{
		CameraZoom.onZoomFinished -= ZoomBack;
		UICreatorManager.ChangeState(UICreatorState.BaseCharacter);
	}

	// Update is called once per frame
	private void Update () 
	{
		if (!_IsInitialized)
		{
			if (!UMACharacterCreator.IsGeneratingCharacter())
			{
				_CharacterDna = UMACharacterCreator.GetCharacterDna();
				
				_IsInitialized = true;
			}
		}
	}
	
	private void OnGUI() 
	{
		#region Header
		
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
		GUILayout.Label("Face Middle Detail", centeredTextStyle);
		
		#endregion
		
		GUI.enabled = !UMACharacterCreator.IsGeneratingCharacter();
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Upper Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceUpperDetail);
		}
		
		if (GUILayout.Button("Lower Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceLowerDetail);
		}
		
		GUILayout.EndHorizontal();

		if (_CharacterDna != null)
		{
			GUILayout.Label("Cheek Size", GUILayout.Height(25));
			float newCheekSize = GUILayout.HorizontalSlider(_CharacterDna.cheekSize, 0f, 1.0f, GUILayout.Height(10));
			if (newCheekSize != _CharacterDna.cheekSize)
			{
				_CharacterDna.cheekSize = newCheekSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Cheek Position", GUILayout.Height(25));
			float newCheekPosition = GUILayout.HorizontalSlider(_CharacterDna.cheekPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newCheekPosition != _CharacterDna.cheekPosition)
			{
				_CharacterDna.cheekPosition = newCheekPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Low Cheek Pronounced", GUILayout.Height(25));
			float newLowCheekPronounced = GUILayout.HorizontalSlider(_CharacterDna.lowCheekPronounced, 0f, 1.0f, GUILayout.Height(10));
			if (newLowCheekPronounced != _CharacterDna.lowCheekPronounced)
			{
				_CharacterDna.lowCheekPronounced = newLowCheekPronounced;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Low Cheek Position", GUILayout.Height(25));
			float newLowCheekPosition = GUILayout.HorizontalSlider(_CharacterDna.lowCheekPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newLowCheekPosition != _CharacterDna.lowCheekPosition)
			{
				_CharacterDna.lowCheekPosition = newLowCheekPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Nose Size", GUILayout.Height(25));
			float newNoseSize = GUILayout.HorizontalSlider(_CharacterDna.noseSize, 0f, 1.0f, GUILayout.Height(10));
			if (newNoseSize != _CharacterDna.noseSize)
			{
				_CharacterDna.noseSize = newNoseSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Nose Width", GUILayout.Height(25));
			float newNoseWidth = GUILayout.HorizontalSlider(_CharacterDna.noseWidth, 0f, 1.0f, GUILayout.Height(10));
			if (newNoseWidth != _CharacterDna.noseWidth)
			{
				_CharacterDna.noseWidth = newNoseWidth;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Nose Position", GUILayout.Height(25));
			float newNosePosition = GUILayout.HorizontalSlider(_CharacterDna.nosePosition, 0f, 1.0f, GUILayout.Height(10));
			if (newNosePosition != _CharacterDna.nosePosition)
			{
				_CharacterDna.nosePosition = newNosePosition;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Nose Pronounced", GUILayout.Height(25));
			float newNosePronounced = GUILayout.HorizontalSlider(_CharacterDna.nosePronounced, 0f, 1.0f, GUILayout.Height(10));
			if (newNosePronounced != _CharacterDna.nosePronounced)
			{
				_CharacterDna.nosePronounced = newNosePronounced;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Nose Flatten", GUILayout.Height(25));
			float newNoseFlatten = GUILayout.HorizontalSlider(_CharacterDna.noseFlatten, 0f, 1.0f, GUILayout.Height(10));
			if (newNoseFlatten != _CharacterDna.noseFlatten)
			{
				_CharacterDna.noseFlatten = newNoseFlatten;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Nose Inclination", GUILayout.Height(25));
			float newNoseInclination = GUILayout.HorizontalSlider(_CharacterDna.noseInclination, 0f, 1.0f, GUILayout.Height(10));
			if (newNoseInclination != _CharacterDna.noseInclination)
			{
				_CharacterDna.noseInclination = newNoseInclination;
				UMACharacterCreator.ChangeCharacterShape();
			}
		}

		GUI.enabled = true;
		
		#region Footer
		
		GUILayout.FlexibleSpace();
		
		if (GUILayout.Button("Back", GUILayout.Height(28)))
		{
			CameraZoom.Zoom("ZeroPosition");
			CameraZoom.onZoomFinished += ZoomBack;
		}
		
		GUILayout.EndVertical();
		
		GUILayout.EndArea();
		
		#endregion
	}
}
