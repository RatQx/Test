namespace Aukcionas.Utils
{
    public interface IDailycoService
    {
        Task<string> StartLivestream();
        Task StopLivestream(string auctionid, string roomid);
    }
}
