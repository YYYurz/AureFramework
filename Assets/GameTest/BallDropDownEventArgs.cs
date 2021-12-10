using AureFramework.Event;
using GameTest;

public class BallDropDownEventArgs : AureEventArgs
{
    public static BallDropDownEventArgs Create() {
        var ballDropDownEventArgs = GameMain.ReferencePool.Acquire<BallDropDownEventArgs>();
        return ballDropDownEventArgs;
    }
    
    public override void Clear() {
        
    }
}
