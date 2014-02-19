using UnityEngine;
using System.Collections;

using UMA;

public class UIFaceUpperDetail : MonoBehaviour 
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
		GUILayout.Label("Face Upper Detail", centeredTextStyle);

		#endregion

		GUI.enabled = !UMACharacterCreator.IsGeneratingCharacter();

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Middle Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceMiddleDetail);
		}

		if (GUILayout.Button("Lower Detail", GUILayout.Height(25))) 
		{
			UICreatorManager.ChangeState(UICreatorState.FaceLowerDetail);
		}

		GUILayout.EndHorizontal();

		if (_CharacterDna != null)
		{
			GUILayout.Label("Forehead Size", GUILayout.Height(25));
			float newForeheadSize = GUILayout.HorizontalSlider(_CharacterDna.foreheadSize, 0f, 1.0f, GUILayout.Height(10));
			if (newForeheadSize != _CharacterDna.foreheadSize)
			{
				_CharacterDna.foreheadSize = newForeheadSize;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Forehead Position", GUILayout.Height(25));
			float newForeheadPosition = GUILayout.HorizontalSlider(_CharacterDna.foreheadPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newForeheadPosition != _CharacterDna.foreheadPosition)
			{
				_CharacterDna.foreheadPosition = newForeheadPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Ear Size", GUILayout.Height(25));
			float newEarSize = GUILayout.HorizontalSlider(_CharacterDna.earsSize, 0f, 1.0f, GUILayout.Height(10));
			if (newEarSize != _CharacterDna.earsSize)
			{
				_CharacterDna.earsSize = newEarSize;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Ear Position", GUILayout.Height(25));
			float newEarPosition = GUILayout.HorizontalSlider(_CharacterDna.earsPosition, 0f, 1.0f, GUILayout.Height(10));
			if (newEarPosition != _CharacterDna.earsPosition)
			{
				_CharacterDna.earsPosition = newEarPosition;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Ear Rotation", GUILayout.Height(25));
			float newEarRotation = GUILayout.HorizontalSlider(_CharacterDna.earsRotation, 0f, 1.0f, GUILayout.Height(10));
			if (newEarRotation != _CharacterDna.earsRotation)
			{
				_CharacterDna.earsRotation = newEarRotation;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Eye Size", GUILayout.Height(25));
			float newEyeSize = GUILayout.HorizontalSlider(_CharacterDna.eyeSize, 0f, 1.0f, GUILayout.Height(10));
			if (newEyeSize != _CharacterDna.eyeSize)
			{
				_CharacterDna.eyeSize = newEyeSize;
				UMACharacterCreator.ChangeCharacterShape();
			}

			GUILayout.Label("Eye Rotation", GUILayout.Height(25));
			float newEyeRotation = GUILayout.HorizontalSlider(_CharacterDna.eyeRotation, 0f, 1.0f, GUILayout.Height(10));
			if (newEyeRotation != _CharacterDna.eyeRotation)
			{
				_CharacterDna.eyeRotation = newEyeRotation;
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
