using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraZoomPosition
{
	public string Name;
	public Vector3 Position;
	public Vector3 Rotation;
}

public class CameraZoom : MonoBehaviour 
{
	private static CameraZoom s_Instance;

	public Camera _Camera;
	public float _CameraSmoothing = 5f;
	public CameraZoomPosition[] ZoomPositions;

	protected Transform _CameraTransform;
	protected bool _IsCameraZooming = false;
	protected Vector3 _TargetPosition = Vector3.zero;
	protected Vector3 _TargetRotation = Vector3.zero;

	//Function called when zoom finished
	public delegate void OnZoomFinished();
	protected OnZoomFinished _OnZoomFinished;

	public static OnZoomFinished onZoomFinished 
	{
		get { return s_Instance._OnZoomFinished;  }
		set { s_Instance._OnZoomFinished = value; }
	}
	
	void Awake() 
	{
		s_Instance = this;

		if (_Camera == null)
			_Camera = Camera.main;

		_CameraTransform = _Camera.transform;
	}

	protected IEnumerator Zooming()
	{
		float frameRate = 1.0f / 60f;
		Quaternion targetRotation = Quaternion.Euler(_TargetRotation);

		while(true)
		{
			float lerpX = Mathf.Lerp(_CameraTransform.localPosition.x, _TargetPosition.x, Time.deltaTime * _CameraSmoothing);
			float lerpY = Mathf.Lerp(_CameraTransform.localPosition.y, _TargetPosition.y, Time.deltaTime * _CameraSmoothing);
			float lerpZ = Mathf.Lerp(_CameraTransform.localPosition.z, _TargetPosition.z, Time.deltaTime * _CameraSmoothing);

			_CameraTransform.localPosition = new Vector3(lerpX, lerpY, lerpZ);
			_CameraTransform.rotation = Quaternion.Slerp(_CameraTransform.rotation, targetRotation, Time.deltaTime * _CameraSmoothing);

			bool doneMoving = Vector3.Distance(_CameraTransform.localPosition, _TargetPosition) < 0.001f;
			//bool doneRotating = Vector3.Distance(_CameraTransform.rotation.eulerAngles, _TargetRotation) < 0.001f;

			if (doneMoving)
			{
				_CameraTransform.localPosition = new Vector3(_TargetPosition.x, _TargetPosition.y, _TargetPosition.z);
				_CameraTransform.rotation = targetRotation;
				_IsCameraZooming = false;

				if (_OnZoomFinished != null)
					_OnZoomFinished();

				break;
			}

			yield return new WaitForSeconds(frameRate);
		}

		yield return null;
	}

	public static bool Zoom(string name)
	{
		if (s_Instance == null)
			return false;

		if (s_Instance._IsCameraZooming)
			return false;

		bool found = false;
		foreach(CameraZoomPosition camZoom in s_Instance.ZoomPositions)
		{
			if (camZoom.Name == name)
			{
				s_Instance._TargetPosition = camZoom.Position;
				s_Instance._TargetRotation = camZoom.Rotation;
				s_Instance._IsCameraZooming = true;

				s_Instance.StartCoroutine(s_Instance.Zooming());

				break;
			}
		}

		return found;
	}
}
