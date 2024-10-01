using System.Collections;
using System.Collections.Generic;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundMediator : EventMediator
  {
    [Inject]
    public BackgroundView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    private RectTransform _rectTransform;
    private RectTransform _groundRectTransform;
    private RectTransform _skyRectTransform;
    private RectTransform _treeContainerRectTransform;

    private readonly float[] _intervals = { 0.35f, 0.4f, 1f, 2f, 3f, 3.5f, 4f };
    private float _minWidth;
    private float _minHeight;
    private float _maxWidth;
    private float _maxHeight;

    private Sprite _lastImage;
    private IEnumerator _treeRoutine;
    private List<RectTransform> _treeRectTransforms;
    private List<RectTransform> _deleteList;

    private const float BaseParticleMinVelocity = -5f;
    private const float BaseParticleMaxVelocity = -25f;
    
    public override void OnRegister()
    {
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.DashFinished, UpdateParticle);
    }

    public override void OnInitialize()
    {
      _treeRectTransforms = new List<RectTransform>();
      _rectTransform = transform.GetComponent<RectTransform>();
      _skyRectTransform = view.skyRawImage.GetComponent<RectTransform>();
      _treeContainerRectTransform = view.treeContainerTransform.GetComponent<RectTransform>();
      _groundRectTransform = view.groundRawImage.GetComponent<RectTransform>();

      Vector2 rect = new(_rectTransform.rect.width, _rectTransform.rect.height);
      _minWidth = rect.x / 5;
      _minHeight = rect.y / 3;
      _maxWidth = rect.x / 3;
      _maxHeight = rect.y / 2;

      _treeRoutine = TreeRoutine();
      StartCoroutine(_treeRoutine);
    }

    public void FixedUpdate()
    {
      if (!playerModel.isAlive) return;
      
      ParallaxEffect(view.skyRawImage, _skyRectTransform, 0.2f);
      ParallaxEffect(view.groundRawImage, _groundRectTransform, 1f);

      for (int i = _treeRectTransforms.Count - 1; i >= 0; i--)
      {
        RectTransform tree = _treeRectTransforms[i];

        tree.Translate(new Vector2(-playerModel.currentGameSpeed / 2, 0), Space.World);

        if (!(tree.anchoredPosition.x <= -_treeContainerRectTransform.rect.width - tree.sizeDelta.x)) continue;

        _treeRectTransforms.RemoveAt(i);
        Destroy(tree.gameObject);
      }
    }

    private void ParallaxEffect(RawImage image, RectTransform rectTransform, float speedFactor)
    {
      Rect rect = image.uvRect;
      rect.x += playerModel.currentGameSpeed / rectTransform.lossyScale.x / rectTransform.rect.width * speedFactor;
      if (rect.x > 1)
      {
        rect.x -= Mathf.Floor(rect.x);
      }

      image.uvRect = rect;
    }

    private IEnumerator TreeRoutine()
    {
      while (playerModel.isAlive)
      {
        yield return new WaitForSeconds(_intervals[Random.Range(0, _intervals.Length)]);
        
        GameObject tree = Instantiate(view.treeObject, view.treeContainerTransform, true);
        tree.transform.localScale = Vector3.one;

        RectTransform treeRectTransform = tree.GetComponent<RectTransform>();
        treeRectTransform.anchoredPosition = Vector2.zero;
        treeRectTransform.sizeDelta = new Vector2(Random.Range(_minWidth, _maxWidth), Random.Range(_minHeight, _maxHeight));
        _treeRectTransforms.Add(treeRectTransform);

        Sprite newImage = view.treeImages[Random.Range(0, view.treeImages.Count)];
        tree.GetComponent<Image>().sprite = newImage;
        view.treeImages.Remove(newImage);
        if (_lastImage != null)
        {
          view.treeImages.Add(_lastImage);
        }

        _lastImage = newImage;

        
        UpdateParticle();
      }
    }

    private void UpdateParticle()
    {
      ParticleSystem.VelocityOverLifetimeModule velocityModule = view.ambientParticle.velocityOverLifetime;
      velocityModule.x = new ParticleSystem.MinMaxCurve(BaseParticleMinVelocity*playerModel.currentGameSpeed, BaseParticleMaxVelocity*playerModel.currentGameSpeed);
    }
    
    private void OnDied()
    {
      StopCoroutine(_treeRoutine);
      
      ParticleSystem.VelocityOverLifetimeModule velocityModule1 = view.ambientParticle.velocityOverLifetime;
      velocityModule1.x = new ParticleSystem.MinMaxCurve(0.1f, -0.1f);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);      
      dispatcher.RemoveListener(PlayerEvent.DashFinished, UpdateParticle);
    }
  }
}
