//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Looks for VRPointerReceiver by either touch or laser pointer and calls
// Hover, Click and Activate methods.
// This script should be on the controller object in the camera rig with a VRInteracter
//
//===================Contact Email: Sam@MassGames.co.uk===========================
using UnityEngine.UI;
using UnityEngine.EventSystems;


#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

namespace VRUserInterfaces
{
	public class VRPointer : MonoBehaviour 
	{
		public const string pointerTransformName = "PointerTransform";

		public string pointerLayer = "UI";
		public Transform pointerTransform;
		public Material pointerMaterial;
		public GameObject hitPrefab;
		public float maxPointerRange = 100f;

		protected LayerMask mask;
		protected LineRenderer lineRenderer;
		protected VRInteractor _interactor;
		protected VRPointerReceiver currentPointerReceiver;
		protected GameObject hitInstance;
		protected RaycastHit _lastHit;
		protected VRPointer _otherPointer;

		public VRInteractor interactor
		{
			get 
			{
				if (_interactor == null) _interactor = GetComponent<VRInteractor>();
				return _interactor; 
			}
		}

		public Transform getPointerTransform
		{
			get 
			{
				if (pointerTransform == null)
				{
					GameObject pointerObject = new GameObject(VRPointer.pointerTransformName);
					pointerTransform = pointerObject.transform;
					pointerTransform.SetParent(transform);
					pointerTransform.localPosition = Vector3.zero;
					if (interactor.vrInput.isSteamVR())
						pointerTransform.localRotation = Quaternion.Euler(50f,0f,0f);
					else
						pointerTransform.localRotation = Quaternion.identity;
				}
				return pointerTransform;
			}
		}

		public RaycastHit lastHit
		{
			get { return _lastHit; }
		}

		public bool isPointerReceiver
		{
			get { return currentPointerReceiver != null; }
		}

		EventSystem eventSystem;

		virtual protected void Start() 
		{
			if (pointerLayer != "") mask = 1 << LayerMask.NameToLayer(pointerLayer);
			_interactor = GetComponent<VRInteractor>();

			eventSystem = FindObjectOfType<EventSystem>();

			CreateNewLineRenderer();
		}

		virtual protected void OnEnable()
		{
			VREvent.Listen("Drop", OnDrop);
		}

		virtual protected void OnDisable()
		{
			VREvent.Remove("Drop", OnDrop);
			if (lineRenderer != null) lineRenderer.enabled = false;
			if (hitInstance != null) hitInstance.SetActive(false);
		}

		virtual protected void OnDestroy()
		{
			Destroy(lineRenderer.gameObject);
		}

		void OnDrop(object[] arr)
		{
			VRInteractableItem item = (VRInteractableItem)arr[0];
			if (currentPointerReceiver == null || item == null || item.GetInstanceID() != currentPointerReceiver.GetInstanceID()) return;

			currentPointerReceiver = null;
		}

		virtual protected void FixedUpdate()
		{
			if (lineRenderer == null) CreateNewLineRenderer();

			Ray ray = new Ray(getPointerTransform.position, getPointerTransform.forward);
			bool hitSomething = Physics.Raycast(ray, out _lastHit, maxPointerRange, mask);
			lineRenderer.enabled = hitSomething;
			if (hitInstance != null) hitInstance.SetActive(hitSomething);
			if (hitSomething)
			{
				GraphicRaycaster gRaycaster = _lastHit.transform.GetComponent<GraphicRaycaster>();
				if (eventSystem != null && gRaycaster != null)
				{
					
					PointerEventData pointerEvent = new PointerEventData(eventSystem);
					pointerEvent.position = Camera.main.WorldToScreenPoint(_lastHit.point);
					List<RaycastResult> results = new List<RaycastResult>();
					//gRaycaster.Raycast(pointerEvent, results);
					eventSystem.RaycastAll(pointerEvent, results);
					Debug.Log("ray hit");
					foreach(RaycastResult result in results)
					{
						Debug.Log("hit " + result.gameObject.name);
					}
				}
				if (interactor.heldItem == null && interactor.hoverItem == null)
				{
					VRPointerReceiver receiver = _lastHit.transform.GetComponent<VRPointerReceiver>();
					if (receiver != null && receiver.heldBy == null)
					{
						if (currentPointerReceiver != receiver)
						{
							if (currentPointerReceiver != null && !OtherHandIsHovering(currentPointerReceiver))
							{
								currentPointerReceiver.DisableHover(interactor);
							}
							currentPointerReceiver = receiver;
							currentPointerReceiver.EnableHover(interactor);
						}
					} else if (currentPointerReceiver != null)
					{
						if (!OtherHandIsHovering(currentPointerReceiver))
							currentPointerReceiver.DisableHover(interactor);
						currentPointerReceiver = null;
					}
				}
				lineRenderer.SetPosition(0, getPointerTransform.position);
				lineRenderer.SetPosition(1, _lastHit.point);
				if (hitInstance != null)
				{
					hitInstance.transform.position = _lastHit.point+(_lastHit.normal*0.01f);
					hitInstance.transform.LookAt(_lastHit.point-_lastHit.normal);
				}
			} else if (currentPointerReceiver != null)
			{
				if (!OtherHandIsHovering(currentPointerReceiver))
					currentPointerReceiver.DisableHover(interactor);
				currentPointerReceiver = null;
			}
		}

		/// <summary>
		/// Called by VRInput using a SendMessage.
		/// </summary>
		/// <param name="message">Method name for receiving item</param>
		public void InputReceived(string method)
		{
			if (interactor.hoverItem != null || interactor.heldItem != null || currentPointerReceiver == null) return;
			interactor.hoverItem = currentPointerReceiver;
			currentPointerReceiver.gameObject.SendMessage(method, interactor, SendMessageOptions.DontRequireReceiver);
			interactor.hoverItem = null;
			//currentPointerReceiver = null;
		}

		bool OtherHandIsHovering(VRPointerReceiver receiver)
		{
			if (_otherPointer == null)
			{
				if (interactor == null || receiver == null) return false;
				VRInteractor otherHand = interactor.GetOtherController();
				if (otherHand == null) return false;
				_otherPointer = otherHand.GetComponent<VRPointer>();
			}
			if (_otherPointer == null || _otherPointer.currentPointerReceiver != receiver) return false;

			return true;
		}

		void CreateNewLineRenderer()
		{
			if (lineRenderer != null) return;

			// Create New Empty Object and parent to controller
			GameObject newLineRendererObject = new GameObject("VRInterfacePointer");
			newLineRendererObject.transform.SetParent(transform);
			newLineRendererObject.transform.localPosition = Vector3.zero;
			newLineRendererObject.transform.localRotation = Quaternion.identity;
			newLineRendererObject.transform.localScale = Vector3.one;

			// Add line render and assign material
			lineRenderer = newLineRendererObject.AddComponent<LineRenderer>();
			lineRenderer.material = pointerMaterial;
			float lineWidth = 0.001f*transform.parent.localScale.magnitude;
			lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
			lineRenderer.useWorldSpace = true;
			lineRenderer.enabled = false;

			if (hitPrefab != null)
			{
				// Create and disable hit prefab
				hitInstance = (GameObject)Instantiate(hitPrefab);
				hitInstance.transform.parent = newLineRendererObject.transform;
				hitInstance.SetActive(false);
			}
		}
	}
}
#endif