using UnityEngine;
using System.Collections;

using UMA;

public class UIFaceLowerDetail : MonoBehaviour 
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
		GUILayout.Label("Face Lower Detail", centeredTextStyle);
		
		#endregion
		
		GUI.enabled = !UMACharacterCreator.IsGeneratingCharacter();
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Upper Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceUpperDetail);
		}
		
		if (GUILayout.Button("Middle Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceMiddleDetail);
		}
		
		GUILayout.EndHorizontal();

		if (_CharacterDna != null)
		{
			GUILayout.Label("Mouth Size", GUILayout.Height(25));
			float newMouthSize = GUILayout.HorizontalSlider(_CharacterDna.mouthSize, 0f, 1.0f, GUILayout.Height(10));
			if (newMouthSize != _CharacterDna.mouthSize)
			{
				_CharacterDna.mouthSize = newMouthSize;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Lip Size", GUILayout.Height(25));
			float newLipSize = GUILayout.HorizontalSlider(_CharacterDna.lipsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newLipSize != _CharacterDna.lipsSize)
			{
				_CharacterDna.lipsSize = newLipSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Mandible Size", GUILayout.Height(25));
			float newMandibleSize = GUILayout.HorizontalSlider(_CharacterDna.mandibleSize, 0f, 1.0f, GUILayout.Height(10));
			if (newMandibleSize != _CharacterDna.mandibleSize)
			{
				_CharacterDna.mandibleSize = newMandibleSize;
				UMACharacterCreator.ChangeCharacterShape();
			}	
			
			GUILayout.Label("Chin Size", GUILayout.Height(25));
			float newChinSize = GUILayout.HorizontalSlider(_CharacterDna.chinSize, 0f, 1.0f, GUILayout.Height(10));
			if (newChinSize != _CharacterDna.chinSize)
			{
				_CharacterDna.chinSize = newChinSize;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Chin Position", GUILayout.Height(25));
			float newChinPosition = GUILayout.HorizontalSlider(_CharacterDna.chinPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newChinPosition != _CharacterDna.chinPosition)
			{
				_CharacterDna.chinPosition = newChinPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Chin Pronounced", GUILayout.Height(25));
			float newChinPronounced = GUILayout.HorizontalSlider(_CharacterDna.chinPronounced, 0f, 1.0f, GUILayout.Height(10));
			if (newChinPronounced != _CharacterDna.chinPronounced)
			{
				_CharacterDna.chinPronounced = newChinPronounced;
				UMACharacterCreator.ChangeCharacterShape();
			}
			
			GUILayout.Label("Jaw Size", GUILayout.Height(25));
			float newJawSize = GUILayout.HorizontalSlider(_CharacterDna.jawsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newJawSize != _CharacterDna.jawsSize)
			{
				_CharacterDna.jawsSize = newJawSize;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Jaw Position", GUILayout.Height(25));
			float newJawPosition = GUILayout.HorizontalSlider(_CharacterDna.jawsPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newJawPosition != _CharacterDna.jawsPosition)
			{
				_CharacterDna.jawsPosition = newJawPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Neck Thickness", GUILayout.Height(25));
			float newNeckThickness = GUILayout.HorizontalSlider(_CharacterDna.neckThickness, 0f, 1.0f, GUILayout.Height(10));
			if (newNeckThickness != _CharacterDna.neckThickness)
			{
				_CharacterDna.neckThickness = newNeckThickness;
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
