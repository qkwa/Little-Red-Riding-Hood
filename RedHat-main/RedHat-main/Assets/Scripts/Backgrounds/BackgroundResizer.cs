using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    private Camera mainCamera;
    private Renderer backgroundRenderer;

    void Start()
    {
        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<Renderer>();

        // ������������ �������� ��� ������ Plane
        float textureAspect = (float)backgroundRenderer.material.mainTexture.width /
                              backgroundRenderer.material.mainTexture.height;
        float screenAspect = (float)Screen.width / Screen.height;

        // ���� �������� ��� ����������, ����� ����������
        if (Mathf.Abs(textureAspect - screenAspect) > 0.01f)
        {
            backgroundRenderer.material.mainTextureScale =
                new Vector2(screenAspect / textureAspect, 1f);
        }
    }

    
}