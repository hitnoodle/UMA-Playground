using UnityEngine;
using System.Collections;

public enum UICreatorState {
	None,
	BaseCharacter,
	FaceUpperDetail,
	FaceMiddleDetail,
	FaceLowerDetail,
	Slots,
};

public class UICreatorManager : MonoBehaviour 
{
	private static UICreatorManager s_Instance;

	protected UICreatorState _CurrentState = UICreatorState.None;

	void Awake()
	{
		s_Instance = this;

		ChangeState(UICreatorState.BaseCharacter);
	}

	public static bool ChangeState(UICreatorState state)
	{
		if (s_Instance == null)
			return false;

		UICreatorState currentState = s_Instance._CurrentState;
		if (currentState != state)
		{
			if (currentState != UICreatorState.None)
			{
				Component currentUIState = s_Instance.gameObject.GetComponent("UI" + currentState.ToString());
				if (currentUIState != null)
				{
					Destroy(currentUIState);
					s_Instance._CurrentState = UICreatorState.None;
				}
			}

			Component newUIState = s_Instance.gameObject.AddComponent("UI" + state);
			if (newUIState != null)
				s_Instance._CurrentState = state;
		}

		return false;
	}
}
