namespace AlexMod.Input {
    public static class PlayerInputManager {
        
        public static InputAsset Asset {
            get {
                if (_asset == null) {
                    _asset = new InputAsset();
                    _asset.Enable();
                }
                return _asset;
            }
        }        
        private static InputAsset _asset;

    }
}