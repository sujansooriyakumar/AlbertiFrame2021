using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MagicRenderer
{
    RenderTexture GetRenderTexture(float canvasDistance, float canvasDiagonalSize, float dotProduct,
                                   int maxTextureSize, float resolution);
}
