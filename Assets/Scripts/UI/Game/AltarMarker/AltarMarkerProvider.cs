using UI.Markers;

namespace UI.Game.AltarMarker
{
    public class AltarMarkerProvider : MarkerProvider<AltarMarkerWidget, AltarMarkerData>
    {

        public bool Visible { get; private set; }

        public void SetVisible(bool visible)
        {
            Visible = visible;
        }
        
        public override bool GetVisibility()
        {
            return Visible;
        }
    }
}