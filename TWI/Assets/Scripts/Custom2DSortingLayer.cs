using UnityEngine;
using System.Collections;

public class Custom2DSortingLayer : MonoBehaviour {

	[SerializeField]
	private string SortingLayer2D;
	[SerializeField]
	private bool ChildrenInherit;


	void Awake()
	{
		renderer.sortingLayerName = SortingLayer2D;
		if (ChildrenInherit)
		{

			Renderer[] childrenRenderers = GetComponentsInChildren<Renderer>() as Renderer[];
			foreach (Renderer childRenderer in childrenRenderers) 
			{
				childRenderer.sortingLayerName = SortingLayer2D;
			}
		}
	}
}
