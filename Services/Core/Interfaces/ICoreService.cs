using Kraft_Back_CS.Models.ViewModels;

namespace Kraft_Back_CS.Services.Core.Interfaces
{
    public interface ICoreService
    {
        List<MenuViewModel> ExibirMenu(String IDCargo);

        void DebugInFile(String responseMessage, String place);
    }
}