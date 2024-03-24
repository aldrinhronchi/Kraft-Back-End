using KaibaSystem_Back_End.Models.Usuario;
using KaibaSystem_Back_End.Models.ViewModels;

namespace KaibaSystem_Back_End.Services.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        RequisicaoViewModel<Usuario> Listar(Int32 Pagina, Int32 RegistrosPorPagina, String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Boolean Salvar(Usuario userViewModel);

        Boolean Excluir(String ID);

        RequisicaoViewModel<Usuario> Autenticar(LoginViewModel Requisicao);
    }
}