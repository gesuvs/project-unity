using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectBlockingObject: MonoBehaviour
{
	[SerializeField] 
	private LayerMask layerMask;

	[SerializeField]
	private Transform target;

	[SerializeField] private new Camera camera;

	[Range(0,1f)]
	[SerializeField]
	private float fadedAlpha = 0.33f;

	[SerializeField]
	private bool retailsShadows = true;

	[SerializeField]
	private Vector3 targetPositionOffset = Vector3.up;

	[SerializeField]
	private float fadeSpeed = 1f;

	[Header("Read Only Data")] 
	[SerializeField]
	private List<FadingObject> objectBlockingView = new();
	private readonly Dictionary<FadingObject, Coroutine> _runningCoroutines = new();

	private readonly RaycastHit[] _hits = new RaycastHit[10];
	
	private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
	private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
	private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
	private static readonly int Surface = Shader.PropertyToID("_Surface");

	private void OnEnable()
	{
		StartCoroutine(CheckForObjects());
	}

	private IEnumerator CheckForObjects()
	{
		while (true)
		{
			var cameraTransform = camera.transform;
			var cameraTransformPosition = cameraTransform.position;
			var targetTransform = target.transform;
			var targetTransformPosition = targetTransform.position;
			
			var hits = Physics.RaycastNonAlloc(
				cameraTransformPosition,
				(targetTransformPosition + targetPositionOffset - cameraTransformPosition)
				.normalized,
				_hits,
				Vector3.Distance(cameraTransformPosition, targetTransformPosition + targetPositionOffset),
				layerMask
			);

			if (hits > 0)
			{
				for (var i = 0; i < hits; i++)
				{
					var fadingObject  = GetFadingObjectFromHit(_hits[i]);

					if (fadingObject  != null && !objectBlockingView.Contains(fadingObject))
					{
						if (_runningCoroutines.ContainsKey(fadingObject))
						{
							if (_runningCoroutines[fadingObject] != null)
							{
								StopCoroutine(_runningCoroutines[fadingObject]);
							}
							
							_runningCoroutines.Remove(fadingObject);
						}
						
						_runningCoroutines.Add(fadingObject,StartCoroutine(FadeObjectOut(fadingObject)));
						objectBlockingView.Add(fadingObject);
					}
				}
			}

			FadeObjectsNoLongerBeingHit();
			ClearHits();
			
			yield return null;
		}
	}

	private void FadeObjectsNoLongerBeingHit()
	{
		var objectsToRemove = new List<FadingObject>(objectBlockingView.Count);
		
		foreach (var fadingObject in objectBlockingView)
		{
			var isObjectBeingHit = false;
			foreach (var hit in _hits)
			{
				FadingObject hitFadingObject = GetFadingObjectFromHit(hit);
				if (hitFadingObject != null && fadingObject == hitFadingObject)
				{
					isObjectBeingHit = true;
					break;
				}
			}

			if (!isObjectBeingHit)
			{
				if (_runningCoroutines.ContainsKey(fadingObject))
				{
					if (_runningCoroutines[fadingObject] != null)
					{
						StopCoroutine(_runningCoroutines[fadingObject]);
					}

					_runningCoroutines.Remove(fadingObject);
				}
				
				_runningCoroutines.Add(fadingObject,StartCoroutine(FadeObjectIn(fadingObject)));
				objectsToRemove.Add(fadingObject);
			}
		}

		foreach (var removeObject in objectsToRemove)
		{ 
			objectBlockingView.Remove(removeObject);
		}
	}



	private IEnumerator FadeObjectOut(FadingObject objectFader)
	{
		foreach (var material in objectFader.materials)
		{
			material.SetInt(SrcBlend,(int) UnityEngine.Rendering.BlendMode.SrcAlpha);
			material.SetInt(DstBlend,(int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt(ZWrite,0);
			material.SetInt(Surface,1);

			material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
			
			material.SetShaderPassEnabled("DepthOnly",false);
			material.SetShaderPassEnabled("SHADOWCASTER",retailsShadows);
			
			material.SetOverrideTag("RenderType","Transparent");
			
			material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
		}

		float time = 0;

		while (objectFader.materials[0].color.a > fadedAlpha)
		{
			foreach (var material in objectFader.materials)
			{
				if (material.HasProperty("_BaseColor"))
				{
					material.color = new Color(
						material.color.r,
						material.color.g,
						material.color.b,
						Mathf.Lerp(objectFader.initialAlpha, fadedAlpha, time * fadeSpeed));
				}
			}

			time += Time.deltaTime;
			yield return null;
		}

		if (_runningCoroutines.ContainsKey(objectFader))
		{
			StopCoroutine(_runningCoroutines[objectFader]);
			_runningCoroutines.Remove(objectFader);
		}
		
	}

	private IEnumerator FadeObjectIn(FadingObject objectFader)
	{
		
		float time = 0;

		while (objectFader.materials[0].color.a < objectFader.initialAlpha)
		{
			foreach (var material in objectFader.materials)
			{
				if (material.HasProperty("_BaseColor"))
				{
					material.color = new Color(
						material.color.r,
						material.color.g,
						material.color.b,
						Mathf.Lerp(fadedAlpha, objectFader.initialAlpha, time * fadeSpeed));
				}
			}

			time += Time.deltaTime;
			yield return null;
		}
		
		foreach (var material in objectFader.materials)
		{
			material.SetInt(SrcBlend,(int) UnityEngine.Rendering.BlendMode.One);
			material.SetInt(DstBlend,(int) UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt(ZWrite,1);
			material.SetInt(Surface,0);

			material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
			
			material.SetShaderPassEnabled("DepthOnly",true);
			material.SetShaderPassEnabled("SHADOWCASTER",true);
			
			material.SetOverrideTag("RenderType","Opaque");
			
			material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		}

		if (_runningCoroutines.ContainsKey(objectFader))
		{
			StopCoroutine(_runningCoroutines[objectFader]);
			_runningCoroutines.Remove(objectFader);
		}
		

	}
	
	private void ClearHits()
	{
		Array.Clear(_hits,0,_hits.Length);
	}
	
	private FadingObject GetFadingObjectFromHit(RaycastHit hit)
	{
		return hit.collider != null ? hit.collider.GetComponent<FadingObject>() : null;
	}
}
