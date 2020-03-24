using System.Threading.Tasks;

namespace XplicityApp.Services.BackgroundFunctions.Interfaces
{
    public interface IBackgroundInventoryUpdater
    {
        Task ApplyDepreciationToInventoryItems();
    }
}