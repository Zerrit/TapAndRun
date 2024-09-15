using Cysharp.Threading.Tasks;

namespace TapAndRun.MVP.CharacterCamera.Model
{
    public interface ICameraZoom
    {
        UniTaskVoid SetLoseZoomAsync();
        UniTaskVoid SetFreeViewZoomAsync();
    }
}