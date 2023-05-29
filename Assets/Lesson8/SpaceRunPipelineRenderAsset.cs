using CustomRenderPipeline;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/" + nameof(SpaceRunPipelineRenderAsset))]
public class SpaceRunPipelineRenderAsset : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new SpaceRunPipelineRender();
    }
}