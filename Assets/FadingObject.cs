using System;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{

	public List<Renderer> renderers = new List<Renderer>();
	private Vector3 _position;
	public List<Material> materials = new List<Material>();

	[HideInInspector] public float initialAlpha;

	private void Awake()
	{
		_position = transform.position;

		if (renderers.Count == 0)
		{
			renderers.AddRange(GetComponentsInChildren<Renderer>());
		}

		foreach (var renderer in renderers)
		{
			materials.AddRange(renderer.materials);
		}

		initialAlpha = materials[0].color.a;
	}

	public bool Equals(FadingObject other)
	{
		return other != null && _position.Equals(other._position);
	}

	public override int GetHashCode()
	{
		return _position.GetHashCode();
	}
	
}

