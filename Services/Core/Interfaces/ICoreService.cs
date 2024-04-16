using KaibaSystem_Back_End.Models.ViewModels;

namespace KaibaSystem_Back_End.Services.Core.Interfaces
{
    public interface ICoreService
    {
        List<MenuViewModel> ExibirMenu(String IDCargo);

        void DebugInFile(String responseMessage, String place);
    }
}