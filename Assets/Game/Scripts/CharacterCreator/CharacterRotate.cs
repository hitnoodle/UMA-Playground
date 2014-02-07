using UnityEngine;
using System.Collections;

public class CharacterRotate : MonoBehaviour 
{
	public float Speed = 120.0f;

	private Transform _Transform;

	// Use this for initialization
	void Start () 
	{
		_Transform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x = Input.GetAxis("Horizontal");
		if (x != 0)
			_Transform.Rotate(0, x * -Speed * Time.deltaTime, 0);

		/*
#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			if(Input.GetAxis("Mouse X") > 0) 
				_Transform.Rotate(0, -Speed * Time.deltaTime, 0);
			else if(Input.GetAxis("Mouse X") < 0) 
				_Transform.Rotate(0, Speed * Time.deltaTime, 0);
		}
		else 
		{
			float x = Input.GetAxis("Horizontal");
			if (x != 0)
				_Transform.Rotate(0, x * -Speed * Time.deltaTime, 0);
		}
#elif UNITY_IPHONE
		if (Input.touches.Length > 0) 
		{
			Touch t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Moved)
			{
				float delta = t.deltaPosition.x;
				Debug.Log(delta);

				if (delta > 0)
					_Transform.Rotate(0, -Speed * Time.deltaTime, 0);
				else if (delta < 0)
					_Transform.Rotate(0, Speed * Time.deltaTime, 0);
			}
		}
#endif
		*/
	}
}
