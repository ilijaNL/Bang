// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/radial-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Radial Blur")]
	public class RadialBlur : BaseEffect
	{
		public enum QualityPreset
		{
			Low = 4,
			Medium = 8,
			High = 12,
			Custom
		}

		[Range(0f, 1f), Tooltip("Blur strength.")]
		public float Strength = 0.1f;

		[Range(2, 32), Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 10;

		[Tooltip("Focus point.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Tooltip("Quality preset. Higher means better quality but slower processing.")]
		public QualityPreset Quality = QualityPreset.Medium;
		
		[Range(-100f, 100f), Tooltip("Smoothness of the vignette effect.")]
		public float Sharpness = 40f;

		[Range(0f, 100f), Tooltip("Amount of vignetting on screen.")]
		public float Darkness = 35f;

		[Tooltip("Should the effect be applied like a vignette ?")]
		public bool EnableVignette = true;

        private RenderTexture m_tex;
        private Camera m_camera;

        void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        void OnPreRender()
        {
            m_tex = RenderTexture.GetTemporary(m_camera.pixelWidth, m_camera.pixelHeight);
            m_camera.targetTexture = m_tex;
        }

        void OnPostRender()
        {
            m_camera.targetTexture = null;
            RenderTexture.ReleaseTemporary(m_tex);
        }


        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Strength <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			int samples = Quality == QualityPreset.Custom ? Samples : (int)Quality;

			Material.SetVector("_Center", Center);
			Material.SetVector("_Params", new Vector4(Strength, samples, Sharpness * 0.01f, Darkness * 0.02f));

			Graphics.Blit(source, destination, Material, EnableVignette ? 1 : 0);
		}
	}
}