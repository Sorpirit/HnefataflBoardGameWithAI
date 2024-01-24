namespace Game.Network
{
    public struct NetworkHostCookie
    {
        public readonly string JoinCode;
        
        public NetworkHostCookie(string joinCode)
        {
            JoinCode = joinCode;
        }
    }
}