using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFResetToNormal : MonoBehaviour
{
	[SerializeField] private Image image;
	private Color color;

	private void Awake()
	{
		color = image.color;
	}
	private void OnDisable()
	{
		image.color = color;	
	}

}
