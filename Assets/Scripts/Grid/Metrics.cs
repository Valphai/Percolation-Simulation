namespace Grid
{
    public static class Metrics
    {
        public const float DiskRadius = .1f;
        public const float DiskHeight = .01f;
        public const float BinHeight = .01f;
        public const float BinLabelHeight = BinHeight + DiskHeight * .25f;
        public const float DiskLabelHeight = BinHeight + DiskHeight * 2.25f;
        public const float BinFontSize = .3f;
        public const float DiskFontSize = .7f;
        public const int SpawnLower = 8443;
        public const int SpawnHigher = 14871;
        public const float MaxDist = 0.70710678118f; // 1/Mathf.Sqrt(2f);
    }
}