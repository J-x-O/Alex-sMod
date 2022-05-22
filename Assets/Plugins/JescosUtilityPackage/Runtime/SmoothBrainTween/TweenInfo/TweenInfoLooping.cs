namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class TweenInfo {

        public int LoopCount { get; private set; }  = 1;
        public bool PingPong { get; private set; } = false;

        public TweenInfo SetLoopPingPong(int loopCount) {
            LoopCount = loopCount + 1;
            PingPong = true;
            return this;
        }
        
        public TweenInfo SetLoop(int loopCount) {
            LoopCount = loopCount + 1;
            PingPong = false;
            return this;
        }
    }
}